using System;
using MvvmCross;
using MvvmCross.ViewModels;
using Physics.WaveInterference.Logic;
using Physics.Shared.Helpers;
using Physics.Shared.UI.Helpers;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using System.Collections.Generic;

namespace Physics.WaveInterference.ViewModels
{
	public class AddOrUpdateOscillationViewModel : MvxNotifyPropertyChanged
	{
		public EditWaveViewModel[] WaveEdits { get; } = new EditWaveViewModel[2];
		private const string EditOscillationKey = "EditWave";
		private const string AddOscillationKey = "AddWave";

		private string[] _existingNames;

		public AddOrUpdateOscillationViewModel(WaveInfo wave1, WaveInfo wave2, float sourceDistance, DifficultyOption difficulty, params string[] existingNames) : this(difficulty, existingNames)
		{
			SourceDistance = sourceDistance;

			DialogTitle = Localizer.Instance.GetString(EditOscillationKey);
			
			WaveEdits[0] = new EditWaveViewModel(wave1, difficulty);
			WaveEdits[0].Label = Localizer.Instance.GetString("Wave") + " 1";

			WaveEdits[1] = new EditWaveViewModel(wave2, difficulty);
			WaveEdits[1].Label = Localizer.Instance.GetString("Wave") + " 2";
		}

		public AddOrUpdateOscillationViewModel(DifficultyOption difficulty, params string[] existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(AddOscillationKey);
			WaveEdits[0] = new EditWaveViewModel();
			WaveEdits[0].Label = Localizer.Instance.GetString("Wave") + " 1";

			WaveEdits[1] = new EditWaveViewModel();
			WaveEdits[1].Label = Localizer.Instance.GetString("Wave") + " 2";

			IsEasyVariant = difficulty == DifficultyOption.Easy;
			Difficulty = difficulty;
		}

		public List<WaveDirection> WaveDirections = new (){ WaveDirection.Left, WaveDirection.Right };

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

		public float Amplitude { get; set; }

		public string PhaseInDeg => MathHelpers.RadiansToDegrees(PhaseInPiRad * (float)Math.PI).ToString("0.0");

		public float PhaseInPiRad { get; set; }
		public float StartPhase { get; set; }

		public float WaveLength { get; set; }
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
			foreach(var wave in WaveEdits)
			{
				wave.PrepareMotion();
			}
			WaveEdits[1].Result.SourceDistance = SourceDistance;
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
