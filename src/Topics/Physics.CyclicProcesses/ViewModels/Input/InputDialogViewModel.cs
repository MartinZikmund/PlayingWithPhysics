using System;
using System.Text;
using MvvmCross.ViewModels;
using Physics.CyclicProcesses.Logic;
using Physics.CyclicProcesses.Logic.Input;
using Physics.CyclicProcesses.Logic.Input.Dialog;
using Physics.Shared.UI.Localization;
using Windows.UI.Xaml.Controls;

namespace Physics.CyclicProcesses.ViewModels.Input
{
	public class InputDialogViewModel : MvxNotifyPropertyChanged
	{
		private readonly ProcessType _processType;

		public InputDialogViewModel(ProcessType processType, IInputConfiguration inputConfiguration)
		{
			_processType = processType;
			DialogConfiguration = InputDialogConfigurations.Configurations[processType];
			if (inputConfiguration != null)
			{
				PresetValues(inputConfiguration);
			}
		}

		public InputDialogConfiguration DialogConfiguration { get; }

		public float N { get; set; } = 1;

		public float VInDm
		{
			get => V * 1000;
			set => V = value / 1000;
		}

		public float V { get; set; } = 0.03f;

		public float V1InDm
		{
			get => V1 * 1000;
			set => V1 = value / 1000;
		}

		public float V1 { get; set; } = 0.023f;

		public float V2InDm
		{
			get => V2 * 1000;
			set => V2 = value / 1000;
		}

		public float V2 { get; set; } = 0.046f;

		public float T { get; set; } = 300;

		public float T1 { get; set; } = 300;

		public float T2 { get; set; } = 400;

		public float T12 { get; set; } = 300;

		public float T34 { get; set; } = 400;

		public float PInkPa
		{
			get => P / 1000;
			set => P = value * 1000;
		}

		public float P { get; set; } = 100000;

		public float P1InkPa
		{
			get => P1 / 1000;
			set => P1 = value * 1000;
		}

		public float P1 { get; set; } = 100000;

		public string ErrorMessage { get; set; }

		public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

		public IInputConfiguration Result { get; private set; }

		public void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			//if (args.Cancel = !Validate())
			//{
			//	return;
			//}

			Result = CreateResult();
		}

		private IInputConfiguration CreateResult() =>
			_processType switch
			{
				ProcessType.Isotermic => new IsotermicInputConfiguration(N, V1, V2, T),
				ProcessType.Isochoric => new IsochoricInputConfiguration(N, V, T1, T2),
				ProcessType.Isobaric => new IsobaricInputConfiguration(N, P, V1, V2),
				ProcessType.Adiabatic => new AdiabaticInputConfiguration(N, P1, V1, V2),
				ProcessType.StirlingEngine => new StirlingEngineInputConfiguration(N, V1, V2, T12, T34),
				_ => throw new InvalidOperationException("Unknown process type"),
			};

		private bool Validate()
		{
			var errorMessageBuilder = new StringBuilder();

			switch (_processType)
			{
				case ProcessType.Isotermic:
					if (V2 <= V1)
					{
						errorMessageBuilder.Append(Localizer.Instance.GetString("ErrorMessage_V2MustBeLargerThanV1"));
					}
					break;
				case ProcessType.Isochoric:
					if (T2 <= T1)
					{
						errorMessageBuilder.Append(Localizer.Instance.GetString("ErrorMessage_T2MustBeLargerThanT1"));
					}
					break;
				case ProcessType.Isobaric:
					if (V2 <= V1)
					{
						errorMessageBuilder.Append(Localizer.Instance.GetString("ErrorMessage_V2MustBeLargerThanV1"));
					}
					break;
				case ProcessType.Adiabatic:
					if (V2 <= V1)
					{
						errorMessageBuilder.Append(Localizer.Instance.GetString("ErrorMessage_V2MustBeLargerThanV1"));
					}
					break;
				case ProcessType.StirlingEngine:
					if (V2 <= V1)
					{
						errorMessageBuilder.Append(Localizer.Instance.GetString("ErrorMessage_V2MustBeLargerThanV1"));
					}
					if (T34 <= T12)
					{
						errorMessageBuilder.AppendLine("");
						errorMessageBuilder.Append(Localizer.Instance.GetString("ErrorMessage_T34MustBeLargerThanT12"));
					}
					break;
			}

			ErrorMessage = errorMessageBuilder.ToString();
			return string.IsNullOrEmpty(ErrorMessage);
		}

		private void PresetValues(IInputConfiguration inputConfiguration)
		{
			N = inputConfiguration.N;
			switch (inputConfiguration)
			{
				case IsotermicInputConfiguration input:
					T = input.T;
					V1 = input.V1;
					V2 = input.V2;
					break;
				case IsobaricInputConfiguration input:
					P = input.P;
					V1 = input.V1;
					V2 = input.V2;
					break;
				case IsochoricInputConfiguration input:
					V = input.V;
					T1 = input.T1;
					T2 = input.T2;
					break;
				case AdiabaticInputConfiguration input:
					P1 = input.P1;
					V1 = input.V1;
					V2 = input.V2;
					break;
				case StirlingEngineInputConfiguration input:
					V1 = input.V1;
					V2 = input.V2;
					T12 = input.T12;
					T34 = input.T34;
					break;
			}
		}
	}
}
