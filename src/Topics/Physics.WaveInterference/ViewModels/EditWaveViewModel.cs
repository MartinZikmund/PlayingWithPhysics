﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using MvvmCross.ViewModels;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Physics.WaveInterference.Logic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Physics.Shared.Helpers;
using MvvmCross;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.UI.Helpers;

namespace Physics.WaveInterference.ViewModels
{
	public class EditWaveViewModel : MvxNotifyPropertyChanged
	{
		private const string EditOscillationKey = "EditWave";
		private const string AddOscillationKey = "AddWave";

		private string[] _existingNames;

		public EditWaveViewModel(WaveInfo oscillationInfo, DifficultyOption difficulty) : this(difficulty)
		{
			Label = oscillationInfo.Label;
			Color = ColorHelper.ToColor(oscillationInfo.Color);
			Frequency = oscillationInfo.Frequency;
			Amplitude = oscillationInfo.Amplitude;
			WaveLength = oscillationInfo.WaveLength;
			SelectedDirection = oscillationInfo.Direction;
			//PhaseInPiRad = oscillationInfo.PhaseInRad / (float)Math.PI;
			Result = oscillationInfo;
		}

		public EditWaveViewModel(DifficultyOption difficulty)
		{
			IsEasyVariant = difficulty == DifficultyOption.Easy;
		}

		public List<WaveDirection> WaveDirections = new() { WaveDirection.Left, WaveDirection.Right };

		public WaveDirection SelectedDirection { get; set; } = WaveDirection.Right;

		public bool IsEasyVariant { get; }

		public string DialogTitle { get; }

		public Color[] AvailableColors { get; } = new Color[]
		{
			ColorHelper.ToColor("#0063B1"),
			ColorHelper.ToColor("#E81123"),
			ColorHelper.ToColor("#2D7D9A"),
			ColorHelper.ToColor("#881798"),
			ColorHelper.ToColor("#498205"),
			ColorHelper.ToColor("#515C6B"),
		};

		public DifficultyOption Difficulty { get; }

		public string Label { get; set; }

		public Color Color { get; set; }

		public float Frequency { get; set; } = 1;

		public float Period => 1 / Frequency;

		public string AngularSpeedInRad => PhysicsHelpers.FrequencyToAngularSpeedInRad(Frequency).ToString("0.0");

		public string AngularSpeedInDeg => PhysicsHelpers.FrequencyToAngularSpeedInDeg(Frequency).ToString("0.0");

		public float Amplitude { get; set; } = 1.0f;

		//public string PhaseInDeg => MathHelpers.RadiansToDegrees(PhaseInPiRad * (float)Math.PI).ToString("0.0");
		//public float PhaseInPiRad { get; set; }
		//public float StartPhase { get; set; }
		public float WaveLength { get; set; } = 5f;

		public float SourceDistance { get; set; } = 0;

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

		public WaveInfo Result { get; private set; }

		public void PrepareMotion()
		{
			if (Result == null)
			{
				Result = new WaveInfo(
					Label,
					Amplitude,
					Frequency,
					WaveLength,
					SelectedDirection,
					SourceDistance,
					ColorHelper.ToHex(Color));
			}
			else
			{
				Result.Label = Label;
				Result.Amplitude = Amplitude;
				Result.Frequency = Frequency;
				Result.WaveLength = WaveLength;
				Result.Direction = SelectedDirection;
				Result.OriginX = SourceDistance;
				Result.Color = ColorHelper.ToHex(Color);
			}
		}
	}
}
