﻿using System;
using MvvmCross;
using MvvmCross.ViewModels;
using Physics.CompoundOscillations.Logic;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Windows.ApplicationModel.Resources;
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
		private bool _autogenerateLabel;

		public AddOrUpdateOscillationViewModel(OscillationInfo oscillationInfo, DifficultyOption difficulty, params string[] existingNames) : this(difficulty, existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(EditOscillationKey);
			_autogenerateLabel = false; // Do not autogenerate for edit mode.
			Label = oscillationInfo.Label;
			Color = ColorHelper.ToColor(oscillationInfo.Color);
			Frequency = oscillationInfo.Frequency;
			Amplitude = oscillationInfo.Amplitude;
			Phase = oscillationInfo.Phase;
		}

		public AddOrUpdateOscillationViewModel(DifficultyOption difficulty, params string[] existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(AddOscillationKey);
			_existingNames = existingNames;
			Difficulty = difficulty;
			SetLocalizedAndNumberedLabelName();
		}

		public Color[] AvailableColors { get; } = new Color[]
		{
			ColorHelper.ToColor("#0063B1"),
			ColorHelper.ToColor("#2D7D9A"),
			ColorHelper.ToColor("#E81123"),
			ColorHelper.ToColor("#881798"),
			ColorHelper.ToColor("#498205"),
			ColorHelper.ToColor("#515C6B"),
		};

		public string DialogTitle { get; set; }

		public string Label { get; set; }

		public Color Color { get; set; }

		public double Frequency { get; set; }

		public double Amplitude { get; set; }

		public double Phase { get; set; }

		public DifficultyOption Difficulty { get; private set; }

		public OscillationInfo Result { get; private set; }

		public async void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			var deferral = args.GetDeferral();
			try
			{
				Result = PrepareMotion();
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

		private OscillationInfo PrepareMotion()
		{
			return new OscillationInfo(
				Label,
				Amplitude,
				Frequency,
				Phase,
				ColorHelper.ToHex(Color));
		}

		private void SetLocalizedAndNumberedLabelName()
		{
			var movementTypeName = Localizer.Instance.GetString("Oscillation");
			var generatedName = UniqueNameGenerator.Generate(movementTypeName, _existingNames);
			Label = generatedName;
		}
	}
}
