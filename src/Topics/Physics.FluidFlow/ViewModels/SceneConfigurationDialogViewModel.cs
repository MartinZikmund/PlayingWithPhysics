using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MvvmCross;
using Physics.FluidFlow.Logic;
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
				SelectedFluid = Fluids.FirstOrDefault(f => f.FluidDefinition == sceneConfiguration.Fluid) ?? Fluids.First();
				SelectedDiameterRelationTypeIndex = DiameterRelationTypes.IndexOf(sceneConfiguration.DiameterRelationType);

				DiameterInM = sceneConfiguration.Diameter1;
				Diameter1InM = sceneConfiguration.Diameter1;
				Diameter2InM = sceneConfiguration.Diameter2;
				Length = sceneConfiguration.Length;
				Velocity = sceneConfiguration.Velocity;
				HeightDecreaseInM = sceneConfiguration.HeightDecrease;
				Pressure = sceneConfiguration.Pressure;
			}
			else
			{
				SelectedFluid = Fluids.First();
				SelectedDiameterRelationTypeIndex = 0;

				DiameterInCm = DiameterConfiguration.DefaultValue ?? 1;
				Diameter1InCm = Diameter1Configuration.DefaultValue ?? 1;
				Diameter2InCm = Diameter2Configuration.DefaultValue ?? 1;
				Length = InputConfiguration.LengthConfiguration.DefaultValue ?? 1;
				Velocity = VelocityConfiguration.DefaultValue ?? 1;
				HeightDecreaseInCm = InputConfiguration.HeightDecreaseConfiguration.DefaultValue ?? 1;
				Pressure = InputConfiguration.PressureConfiguration.DefaultValue ?? 1;
			}
		}

		public event EventHandler InputConfigurationChanged;

		public SceneConfiguration InputSceneConfiguration { get; }

		public InputVariant InputVariant { get; }

		public InputConfiguration InputConfiguration { get; private set; }

		public FieldConfiguration DiameterConfiguration { get; private set; }

		public FieldConfiguration Diameter1Configuration { get; private set; }

		public FieldConfiguration Diameter2Configuration { get; private set; }

		public FieldConfiguration VelocityConfiguration { get; private set; }

		public bool ShowDiameterRelationPicker { get; private set; } = true;

		public DiameterRelationType[] DiameterRelationTypes { get; private set; }

		public int SelectedDiameterRelationTypeIndex { get; set; }

		public DiameterRelationType SelectedDiameterRelationType => SelectedDiameterRelationTypeIndex >= 0 ?
			DiameterRelationTypes[SelectedDiameterRelationTypeIndex] : default;

		public FluidDefinitionViewModel[] Fluids { get; private set; }

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

		public float Pressure { get; set; }

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
			var diameter1 = DiameterConfiguration.IsVisible ? DiameterInM : Diameter1InM;

			// Calculate forced values
			if (SelectedDiameterRelationType == DiameterRelationType.Equal && InputVariant == InputVariant.ContinuityEquation)
			{
				if (SelectedFluid.FluidDefinition == FluidDefinitions.Oil)
				{
					Velocity = 0.4f * 4f / (3.14f * diameter1 * diameter1);
				}
				else
				{
					Velocity = 0.005f * 4f / (3.14f * diameter1 * diameter1);
				}
			}

			return new SceneConfiguration(
				InputVariant,
				SelectedDiameterRelationType,
				SelectedFluid.FluidDefinition,
				Velocity,
				diameter1,
				Diameter2InM,
				Length,
				HeightDecreaseInM,
				Pressure);
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
			UpdateFieldConfigurations();
		}

		internal void OnSelectedFluidChanged()
		{
			if (SelectedFluid != null)
			{
				UpdateFieldConfigurations();
			}
		}

		private void UpdateFieldConfigurations()
		{
			DiameterConfiguration = GetFieldConfiguration(InputConfiguration.DiameterConfigurations);
			Diameter1Configuration = GetFieldConfiguration(InputConfiguration.Diameter1Configurations);
			Diameter2Configuration = GetFieldConfiguration(InputConfiguration.Diameter2Configurations);
			VelocityConfiguration = GetFieldConfiguration(InputConfiguration.VelocityConfigurations);
			InputConfigurationChanged?.Invoke(this, EventArgs.Empty);
		}

		private FieldConfiguration GetFieldConfiguration(Dictionary<FluidDefinition, FieldConfiguration> source)
		{
			if (source.TryGetValue(SelectedFluid.FluidDefinition, out var configuration))
			{
				return configuration;
			}
			return FieldConfiguration.CreateInvisible();
		}

		private async void UpdateInputConfiguration()
		{
			ErrorMessage = "";

			InputConfiguration = InputConfigurations.Configurations.FirstOrDefault(c =>
				c.InputVariant == InputVariant &&
				c.DiameterRelationType == SelectedDiameterRelationType);

			var selectedFluid = SelectedFluid?.FluidDefinition;
			Fluids = InputConfiguration.FluidDefinitions.Select(f => new FluidDefinitionViewModel(f)).ToArray();
			SelectedFluid = Fluids.FirstOrDefault(f => f.FluidDefinition == selectedFluid) ?? Fluids.First();
		}
	}
}
