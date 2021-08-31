using System;
using System.Collections.Generic;
using MvvmCross;
using MvvmCross.ViewModels;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Physics.WaveInterference.Logic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Physics.WaveInterference.ViewModels
{
	public class AddOrUpdateOscillationViewModel : MvxNotifyPropertyChanged
	{
		public EditWaveViewModel[] WaveEdits { get; } = new EditWaveViewModel[2];
		private const string EditOscillationKey = "EditWave";
		private const string AddOscillationKey = "AddWave";

		private string[] _existingNames;

		public AddOrUpdateOscillationViewModel(WaveInfo wave1, WaveInfo wave2, float sourceDistance, DifficultyOption difficulty) : this(difficulty)
		{
			SourceDistance = sourceDistance;

			DialogTitle = Localizer.Instance.GetString(EditOscillationKey);

			WaveEdits[0] = new EditWaveViewModel(wave1, difficulty);
			WaveEdits[0].Label = Localizer.Instance.GetString("Wave") + " 1";

			WaveEdits[1] = new EditWaveViewModel(wave2, difficulty);
			WaveEdits[1].Label = Localizer.Instance.GetString("Wave") + " 2";
		}

		public EditWaveViewModel SelectedWave { get; set; }

		private EditWaveViewModel GetTheOtherWave(EditWaveViewModel wave) => wave == WaveEdits[0] ? WaveEdits[1] : WaveEdits[0];

		internal void OnSelectedWaveChanged()
		{
			// Sync values from previously edited wave in case of Easy difficulty
			if (SelectedWave == null || Difficulty == DifficultyOption.Advanced)
			{
				return;
			}

			var sourceWave = GetTheOtherWave(SelectedWave);
			SyncEasyModeValues(sourceWave, SelectedWave);
		}

		private void SyncEasyModeValues(EditWaveViewModel from, EditWaveViewModel to)
		{
			to.Frequency = from.Frequency;
			to.WaveLength = from.WaveLength;
		}

		public AddOrUpdateOscillationViewModel(DifficultyOption difficulty)
		{
			DialogTitle = Localizer.Instance.GetString(AddOscillationKey);
			WaveEdits[0] = new EditWaveViewModel(difficulty);
			WaveEdits[0].Label = Localizer.Instance.GetString("Wave") + " 1";
			WaveEdits[0].Color = WaveEdits[0].AvailableColors[0];

			WaveEdits[1] = new EditWaveViewModel(difficulty);
			WaveEdits[1].Label = Localizer.Instance.GetString("Wave") + " 2";
			WaveEdits[1].Color = WaveEdits[1].AvailableColors[1];

			IsEasyVariant = difficulty == DifficultyOption.Easy;
			Difficulty = difficulty;
		}

		public List<WaveDirection> WaveDirections = new() { WaveDirection.Left, WaveDirection.Right };

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

		public float SourceDistance { get; set; }

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

		public EditWaveViewModel[] Result { get; private set; }

		private void PrepareMotion()
		{
			if (SelectedWave != null && Difficulty == DifficultyOption.Easy)
			{
				// Ensure values of current wave sync to the other wave
				var targetWave = GetTheOtherWave(SelectedWave);
				SyncEasyModeValues(SelectedWave, targetWave);
			}
			//if (Result == null)
			//{
			//	Result = new WaveInfo(
			//		Label,
			//		Amplitude,
			//		Frequency,
			//		WaveLength,
			//		PhaseInPiRad * (float)Math.PI,
			//		ColorHelper.ToHex(Color));
			//}
			//else
			//{
			//	Result.Label = Label;
			//	Result.Amplitude = Amplitude;
			//	Result.Frequency = Frequency;
			//	Result.PhaseInRad = PhaseInPiRad * (float)Math.PI;
			//	Result.Color = ColorHelper.ToHex(Color);
			//}
			foreach (var wave in WaveEdits)
			{
				wave.PrepareMotion();
			}
			WaveEdits[1].Result.OriginX = SourceDistance;
			Result = WaveEdits;
		}

		private void SetLocalizedAndNumberedLabelName()
		{
			var movementTypeName = Localizer.Instance.GetString("Wave");
			var generatedName = UniqueNameGenerator.Generate(movementTypeName, _existingNames);
			Label = generatedName;
		}
	}
}
