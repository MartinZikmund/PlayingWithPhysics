﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Physics.RotationalInclinedPlane.Logic;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Models.Input;
using Physics.Shared.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Popups;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.RotationalInclinedPlane.ViewModels
{
	public class InputDialogViewModel : ViewModelBase
	{
		private readonly MotionSetup[] _otherSetups;
		private readonly string[] _existingNames;
		private bool _autogenerateLabel = true;

		public InputDialogViewModel(DifficultyOption difficulty, MotionSetup[] otherSetups)
		{
			_existingNames = otherSetups.Select(s => s.Label).ToArray();
			Difficulty = difficulty;
			_otherSetups = otherSetups;
			OnGravityChanged();
			OnSelectedBodyTypeIndexChanged();
		}

		public InputDialogViewModel(MotionSetup setup, DifficultyOption difficulty, MotionSetup[] otherSetups) : this(difficulty, otherSetups)
		{
			_otherSetups = otherSetups;
			_autogenerateLabel = true; // Do not autogenerate for edit mode.
			BodyType = setup.BodyType;
			SelectedBodyTypeIndex = (int)setup.BodyType;
			Radius = setup.Radius;
			InclinedAngle = setup.InclinedAngle;
			InclinedLength = setup.InclinedLength;
			Gravity = setup.Gravity;
			Color = ColorHelper.ToColor(setup.Color);
			Mass = setup.Mass;
		}

		internal async Task<bool> ValidateAsync()
		{
			var violatingTen = new List<string>();
			var violatingHundred = new List<string>();
			foreach (var otherSetup in _otherSetups)
			{
				if (otherSetup.Radius * 10 > InclinedLength)
				{
					violatingTen.Add(otherSetup.Label);
				}
				if (otherSetup.Radius * 100 < InclinedLength)
				{
					violatingHundred.Add(otherSetup.Label);
				}
			}

			if (Radius * 10 > InclinedLength)
			{
				violatingTen.Add(Label);
			}
			if (Radius * 100 < InclinedLength)
			{
				violatingHundred.Add(Label);
			}

			if (violatingTen.Count > 0)
			{
				var dialog = new MessageDialog(string.Format(Localizer.Instance.GetString("PlaneLengthValidationTenMessage"), string.Join(",", violatingTen)), Localizer.Instance.GetString("PlaneLengthValidationTitle"));
				await dialog.ShowAsync();
				return false;
			}

			if (violatingHundred.Count > 0)
			{
				var dialog = new MessageDialog(string.Format(Localizer.Instance.GetString("PlaneLengthValidationHundredMessage"), string.Join(",", violatingHundred)), Localizer.Instance.GetString("PlaneLengthValidationTitle"));
				await dialog.ShowAsync();
				return false;
			}

			return true;
		}

		public DifficultyOption Difficulty { get; }

		public bool IsAdvanced => Difficulty == DifficultyOption.Advanced;

		public Color[] AvailableColors { get; } = new Color[]
		{
			ColorHelper.ToColor("#0063B1"),
			ColorHelper.ToColor("#2D7D9A"),
			ColorHelper.ToColor("#E81123"),
			ColorHelper.ToColor("#881798"),
			ColorHelper.ToColor("#498205"),
			ColorHelper.ToColor("#515C6B"),
		};

		public ObservableCollection<GravityDefault> GravityDefaults { get; } = new ObservableCollection<GravityDefault>()
		{
			new GravityDefault(Localizer.Instance["ManualInput"], null),
			new GravityDefault(Localizer.Instance["Gravity_EarthEquatorSeaLevel"], 9.78f),
			new GravityDefault(Localizer.Instance["Gravity_Earth45"], 9.80665f),
			new GravityDefault(Localizer.Instance["Gravity_EarthPole"], 9.832f),
			new GravityDefault(Localizer.Instance["Gravity_EarthPrague"], 9.81373f),
			new GravityDefault(Localizer.Instance["Gravity_Sun"], 274f),
			new GravityDefault(Localizer.Instance["Gravity_Mercury"], 3.7f),
			new GravityDefault(Localizer.Instance["Gravity_Venus"], 8.87f),
			new GravityDefault(Localizer.Instance["Gravity_Mars"], 3.71f),
			new GravityDefault(Localizer.Instance["Gravity_Jupiter"], 23.12f),
			new GravityDefault(Localizer.Instance["Gravity_Saturn"], 8.96f),
			new GravityDefault(Localizer.Instance["Gravity_Uranus"], 8.69f),
			new GravityDefault(Localizer.Instance["Gravity_Neptune"], 11f),
			new GravityDefault(Localizer.Instance["Gravity_Moon"], 1.62f)
		};

		public float Mass { get; set; } = 1;

		public float InclinedLength { get; set; } = 5;

		public float InclinedAngle { get; set; } = 45;

		public int MaximumAngle => 45;

		public float Gravity { get; set; } = 9.806f;

		public void OnGravityChanged()
		{
			if (GravityDefaults.FirstOrDefault(d => d.HasValue && Math.Abs(d.Value.Value - Gravity) < 0.001f) is { } gravity)
			{
				SelectedGravity = gravity;
			}
			else
			{
				SelectedGravity = GravityDefaults[0];
			}
		}

		public int SelectedBodyTypeIndex { get; set; }

		public void OnSelectedBodyTypeIndexChanged()
		{
			if (SelectedBodyTypeIndex >= 0)
			{
				BodyType = (BodyType)SelectedBodyTypeIndex;
				var startsWithKnownPrefix = Enum.GetNames(typeof(BodyType)).Any(nameKey => Label?.StartsWith(ResourceLoader.GetForCurrentView().GetString($"BodyType_{BodyType.ToString("g")}"), StringComparison.InvariantCultureIgnoreCase) == true);
				if (_autogenerateLabel || startsWithKnownPrefix)
				{
					SetLocalizedAndNumberedLabelName();
				}
			}
		}

		private void SetLocalizedAndNumberedLabelName()
		{
			var movementTypeName = ResourceLoader.GetForCurrentView().GetString($"BodyType_{BodyType.ToString("g")}");
			var generatedName = UniqueNameGenerator.Generate(movementTypeName, _existingNames);
			Label = generatedName;
		}

		public BodyType[] BodyTypes { get; } = Enum.GetValues(typeof(BodyType)).OfType<BodyType>().ToArray();

		public BodyType BodyType { get; private set; }

		public string Label { get; set; }

		public float Radius { get; set; } = 0.1f;

		public GravityDefault SelectedGravity { get; set; }

		public void OnSelectedGravityChanged()
		{
			if (SelectedGravity != GravityDefaults[0])
			{
				Gravity = SelectedGravity?.Value ?? 9.81f;
			}
		}

		public Color Color { get; set; } = ColorHelper.ToColor("#0063B1");

		public MotionSetup CreateMotionSetup()
		{
			return new MotionSetup(
				Label,
				BodyType,
				Mass,
				Gravity,
				Radius,
				InclinedLength,
				InclinedAngle,
				0,
				ColorHelper.ToHex(Color));
		}
	}
}
