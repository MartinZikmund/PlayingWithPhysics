using System;
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
		private float _frequency = 1;
		private float _angularSpeedInDeg = PhysicsHelpers.FrequencyToAngularSpeedInDeg(1);
		private float _angularSpeedInRad = PhysicsHelpers.FrequencyToAngularSpeedInRad(1);
		private bool _avoidRecalc = false;
		private float _phaseInDeg = 0;
		private float _phaseInPiRad = 0;

		public AddOrUpdateOscillationViewModel(OscillationInfo oscillationInfo, DifficultyOption difficulty, params string[] existingNames) : this(difficulty, existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(EditOscillationKey);
			Label = oscillationInfo.Label;
			Color = ColorHelper.ToColor(oscillationInfo.Color);
			Frequency = oscillationInfo.Frequency;
			Amplitude = oscillationInfo.Amplitude;
			PhaseInDeg = MathHelpers.RadiansToDegrees(oscillationInfo.PhaseInRad);
		}

		public AddOrUpdateOscillationViewModel(DifficultyOption difficulty, params string[] existingNames)
		{
			DialogTitle = Localizer.Instance.GetString(AddOscillationKey);
			_existingNames = existingNames;
			Difficulty = difficulty;
			SetLocalizedAndNumberedLabelName();
		}

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

		public OscillationInfo Result { get; private set; }

		public string Label { get; set; }

		public Color Color { get; set; } = ColorHelper.ToColor("#0063B1");

		public float Frequency
		{
			get => _frequency;
			set
			{
				if (!_frequency.AlmostEqualTo(value))
				{
					_frequency = value;
					RaisePropertyChanged();
					if (!_avoidRecalc)
					{
						_avoidRecalc = true;
						AngularSpeedInDeg = PhysicsHelpers.FrequencyToAngularSpeedInDeg(value);
						AngularSpeedInRad = PhysicsHelpers.FrequencyToAngularSpeedInRad(value);
						_avoidRecalc = false;
					}
				}
			}
		}

		public float AngularSpeedInRad
		{
			get => _angularSpeedInRad;
			set
			{
				if (!_angularSpeedInRad.AlmostEqualTo(value))
				{
					_angularSpeedInRad = value;
					RaisePropertyChanged();
					if (!_avoidRecalc)
					{
						_avoidRecalc = true;
						Frequency = PhysicsHelpers.AngularSpeedInRadToFrequency(value);
						AngularSpeedInDeg = PhysicsHelpers.FrequencyToAngularSpeedInDeg(PhysicsHelpers.AngularSpeedInRadToFrequency(value));
						_avoidRecalc = false;
					}
				}
			}
		}
		public float AngularSpeedInDeg
		{
			get => _angularSpeedInDeg;
			set
			{
				if (!_angularSpeedInDeg.AlmostEqualTo(value))
				{
					_angularSpeedInDeg = value;
					RaisePropertyChanged();
					if (!_avoidRecalc)
					{
						_avoidRecalc = true;
						Frequency = PhysicsHelpers.AngularSpeedInDegToFrequency(value);
						AngularSpeedInRad = PhysicsHelpers.FrequencyToAngularSpeedInRad(Frequency);
						_avoidRecalc = false;
					}
				}
			}
		}

		public float Amplitude { get; set; } = 1;

		public float PhaseInDeg
		{
			get => _phaseInDeg;
			set
			{
				if (!_phaseInDeg.AlmostEqualTo(value))
				{
					_phaseInDeg = value;
					RaisePropertyChanged();
					if (!_avoidRecalc)
					{
						_avoidRecalc = true;
						PhaseInPiRad = MathHelpers.DegreesToRadians(value) / (float)Math.PI;
						_avoidRecalc = false;
					}
				}
			}
		}

		public float PhaseInPiRad
		{
			get => _phaseInPiRad;
			set
			{
				if (!_phaseInPiRad.AlmostEqualTo(value))
				{
					_phaseInPiRad = value;
					RaisePropertyChanged();
					if (!_avoidRecalc)
					{
						_avoidRecalc = true;
						PhaseInDeg = MathHelpers.RadiansToDegrees(value * (float)Math.PI);
						_avoidRecalc = false;
					}
				}
			}
		}

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
				PhaseInPiRad * (float)Math.PI,
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
