﻿using System;
using MvvmCross;
using MvvmCross.ViewModels;
using Physics.CompoundOscillations.Logic;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.CompoundOscillations.ViewModels
{
	public class AddOrUpdateOscillationViewModel : MvxNotifyPropertyChanged
	{
		private const string EditOscillationKey = "EditOscillation";
		private const string AddOscillationKey = "AddOscillation";

		private string[] _existingNames;

		public AddOrUpdateOscillationViewModel(OscillationInfo oscillationInfo, DifficultyOption difficulty, params string[] existingNames) : this(difficulty, existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(EditOscillationKey);
			Label = oscillationInfo.Label;
			Color = ColorHelper.ToColor(oscillationInfo.Color);
			Frequency = oscillationInfo.Frequency;
			Amplitude = oscillationInfo.Amplitude;
			PhaseInPiRad = oscillationInfo.PhaseInRad / (float)Math.PI;
			IsEasyVariant = difficulty == DifficultyOption.Easy;
			Result = oscillationInfo;
		}

		public AddOrUpdateOscillationViewModel(DifficultyOption difficulty, params string[] existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(AddOscillationKey);
			_existingNames = existingNames;
			IsEasyVariant = difficulty == DifficultyOption.Easy;
			Difficulty = difficulty;
			SetLocalizedAndNumberedLabelName();
		}

		public bool IsEasyVariant { get; }

		public string DialogTitle { get; }

		public Color[] AvailableColors { get; } = new Color[]
		{
			ColorHelper.ToColor("#0063B1"),
			ColorHelper.ToColor("#2D7D9A"),
			ColorHelper.ToColor("#E81123"),
			ColorHelper.ToColor("#881798"),
			ColorHelper.ToColor("#498205"),
			ColorHelper.ToColor("#515C6B"),
		};

		public DifficultyOption Difficulty { get; }

		public string Label { get; set; }

		public Color Color { get; set; } = ColorHelper.ToColor("#0063B1");

		public float Frequency { get; set; } = 1;

		public float Period => 1 / Frequency;

		public string AngularSpeedInRad => PhysicsHelpers.FrequencyToAngularSpeedInRad(Frequency).ToString("0.0");

		public string AngularSpeedInDeg => PhysicsHelpers.FrequencyToAngularSpeedInDeg(Frequency).ToString("0.0");

		public float Amplitude { get; set; } = 1;

		public string PhaseInDeg => MathHelpers.RadiansToDegrees(PhaseInPiRad * (float)Math.PI).ToString("0.0");

		public float PhaseInPiRad { get; set; }

		public async void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			var deferral = args.GetDeferral();
			try
			{
				PrepareMotion();
			}
			catch (ArgumentException)
			{
				string errorMessage = Localizer.Instance.GetString("ArgumentExceptionErrorMessage");
				var contentDialogHelper = Mvx.IoCProvider.Resolve<IContentDialogHelper>();
				await contentDialogHelper.ShowAsync(Localizer.Instance.GetString("InvalidInput"), errorMessage);
				args.Cancel = true;
			}
			finally
			{
				deferral.Complete();
			}
		}

		public OscillationInfo Result { get; private set; }

		private void PrepareMotion()
		{
			if (Result == null)
			{
				Result = new OscillationInfo(
					Label,
					Amplitude,
					Frequency,
					PhaseInPiRad * (float)Math.PI,
					ColorHelper.ToHex(Color));
			}
			else
			{
				Result.Label = Label;
				Result.Amplitude = Amplitude;
				Result.Frequency = Frequency;
				Result.PhaseInRad = PhaseInPiRad * (float)Math.PI;
				Result.Color = ColorHelper.ToHex(Color);
			}
		}

		private void SetLocalizedAndNumberedLabelName()
		{
			var movementTypeName = Localizer.Instance.GetString("Oscillation");
			var generatedName = UniqueNameGenerator.Generate(movementTypeName, _existingNames);
			Label = generatedName;
		}
	}
}
