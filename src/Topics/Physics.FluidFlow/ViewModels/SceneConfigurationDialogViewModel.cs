﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MvvmCross;
using MvvmCross.ViewModels;
using Physics.FluidFlow.Logic;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.Services.Dialogs;
using Physics.Shared.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Physics.FluidFlow.ViewModels
{
	public class SceneConfigurationDialogViewModel : ViewModelBase
	{
		public SceneConfigurationDialogViewModel(InputVariant inputVariant, SceneConfiguration sceneConfiguration)
		{
			InputVariant = inputVariant;
			InitializeDiameterRelationPicker(inputVariant);
			UpdateInputConfiguration();
			if (sceneConfiguration != null)
			{
				SelectedDiameterRelationTypeIndex = DiameterRelationTypes.IndexOf(sceneConfiguration.DiameterRelationType);

				DiameterInM = sceneConfiguration.Diameter1;
				Diameter1InM = sceneConfiguration.Diameter1;
				Diameter2InM = sceneConfiguration.Diameter2;
				Length = sceneConfiguration.Length;
				HeightDecreaseInM = sceneConfiguration.HeightDecrease;
				Velocity = sceneConfiguration.Velocity;
			}

			Fluids = FluidDefinitions.Definitions.Select(f => new FluidDefinitionViewModel(f)).ToArray();
			SelectedFluid = Fluids.First();
			//Label = oscillationInfo.Label;
			//Color = ColorHelper.ToColor(oscillationInfo.Color);
			//Frequency = oscillationInfo.Frequency;
			//Amplitude = oscillationInfo.Amplitude;
			//PhaseInDeg = oscillationInfo.PhaseInDeg;
		}

		public event EventHandler InputConfigurationChanged;

		public SceneConfiguration InputSceneConfiguration { get; }

		public InputVariant InputVariant { get; }

		public InputConfiguration InputConfiguration { get; private set; }

		public bool ShowDiameterRelationPicker { get; private set; } = true;

		public DiameterRelationType[] DiameterRelationTypes { get; private set; }

		public int SelectedDiameterRelationTypeIndex { get; set; }

		public DiameterRelationType SelectedDiameterRelationType => SelectedDiameterRelationTypeIndex >= 0 ?
			DiameterRelationTypes[SelectedDiameterRelationTypeIndex] : default;

		public FluidDefinitionViewModel[] Fluids { get; }

		public FluidDefinitionViewModel SelectedFluid { get; set; }

		public float DiameterInCm { get; set; }

		public float DiameterInM
		{
			get => DiameterInCm / 100;
			set => DiameterInCm = value * 100;
		}

		public float Diameter1InCm { get; set; }

		public float Diameter1InM
		{
			get => Diameter1InCm / 100;
			set => Diameter1InCm = value * 100;
		}

		public float Diameter2InCm { get; set; }

		public float Diameter2InM
		{
			get => Diameter2InCm / 100;
			set => Diameter2InCm = value * 100;
		}

		public float Velocity { get; set; }

		public float Length { get; set; }

		public float HeightDecreaseInCm { get; set; }

		public float HeightDecreaseInM
		{
			get => HeightDecreaseInCm / 100;
			set => HeightDecreaseInCm = value * 100;
		}

		public string ErrorMessage { get; set; }

		public bool HasErrors => !string.IsNullOrEmpty(ErrorMessage);

		public SceneConfiguration Result { get; set; }

		public async void Save(ContentDialog dialog, ContentDialogButtonClickEventArgs args)
		{
			var deferral = args.GetDeferral();
			try
			{
				if (
					SelectedDiameterRelationType == DiameterRelationType.S1Larger &&
					Diameter1InM <= Diameter2InM)
				{
					ErrorMessage = Localizer.Instance.GetString("S1MustBeLargerMessage");
					args.Cancel = true;
				}
				else if (
					SelectedDiameterRelationType == DiameterRelationType.S2Larger &&
					Diameter2InM <= Diameter1InM)
				{
					ErrorMessage = Localizer.Instance.GetString("S2MustBeLargerMessage");
					args.Cancel = true;
				}

				if (args.Cancel)
				{
					return;
				}

				Result = PrepareConfiguration();
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

		private SceneConfiguration PrepareConfiguration()
		{
			return null;
		}

		private void InitializeDiameterRelationPicker(InputVariant inputVariant)
		{
			DiameterRelationTypes = InputConfigurations.Configurations.Where(c => c.InputVariant == inputVariant).Select(c => c.DiameterRelationType).ToArray();
			SelectedDiameterRelationTypeIndex = (int)DiameterRelationTypes[0];
			ShowDiameterRelationPicker = DiameterRelationTypes.Length > 1;
		}

		internal void OnSelectedDiameterRelationTypeIndexChanged()
		{
			if (SelectedDiameterRelationTypeIndex < 0)
			{
				return;
			}

			UpdateInputConfiguration();
		}

		private async void UpdateInputConfiguration()
		{
			ErrorMessage = "";

			InputConfiguration = InputConfigurations.Configurations.FirstOrDefault(c =>
				c.InputVariant == InputVariant &&
				c.DiameterRelationType == SelectedDiameterRelationType);

			InputConfigurationChanged?.Invoke(this, EventArgs.Empty);

			await Task.Yield();

			// Ensure the current values are valid

			if (SelectedDiameterRelationType == DiameterRelationType.S1Larger &&
				Diameter1InCm <= Diameter2InCm)
			{
				Diameter1InCm = Diameter2InCm + 1;
			}
			else if (SelectedDiameterRelationType == DiameterRelationType.S2Larger &&
			   Diameter2InCm <= Diameter1InCm)
			{
				Diameter2InCm = Diameter1InCm + 1;
			}

			//DiameterInM = InputConfiguration.DiameterConfiguration.DefaultValue ?? 1;
			//Diameter1InM = InputConfiguration.Diameter1Configuration.DefaultValue ?? 1;
			//Diameter2InM = InputConfiguration.Diameter2Configuration.DefaultValue ?? 1;
			//Length = InputConfiguration.LengthConfiguration.DefaultValue ?? 1;
			//Velocity = InputConfiguration.VelocityConfiguration.DefaultValue ?? 1;
			//HeightDecreaseInM = InputConfiguration.HeightDecreaseConfiguration.DefaultValue ?? 1;
		}
	}
}