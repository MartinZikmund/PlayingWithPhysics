﻿using System;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Physics.FluidFlow.Rendering;
using System.Windows.Input;
using System.Threading.Tasks;
using Physics.FluidFlow.Logic;
using Physics.FluidFlow.Controls;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

namespace Physics.FluidFlow.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<FluidFlowCanvasController>
	{
		private DifficultyOption _difficulty;
		private FluidFlowCanvasController _controller;

		public override void Prepare(SimulationNavigationModel parameter)
		{
			IsLoading = true;
			_difficulty = parameter.Difficulty;
			if (_difficulty == DifficultyOption.Easy)
			{
				InputVariants = new[]
				{
					InputVariant.ContinuityEquation,
					InputVariant.BernoulliEquationWithoutHeightChange,
				};
				SelectedVariantIndex = (int)InputVariant.ContinuityEquation;
			}
			else
			{
				InputVariants = new[]
				{
					InputVariant.BernoulliEquationWithHeightChange,
					InputVariant.RealFluidMovement,
				};
				SelectedVariantIndex = (int)InputVariant.ContinuityEquation;
			}
			IsLoading = false;
		}

		public bool IsLoading { get; set; }

		public void SetController(FluidFlowCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		public bool HasConfiguration => SceneConfiguration != null;

		public InputVariant[] InputVariants { get; private set; }

		public int SelectedVariantIndex { get; set; }

		public InputVariant SelectedVariant => SelectedVariantIndex >= 0 ? InputVariants[SelectedVariantIndex] : default;

		public SceneConfigurationViewModel SceneConfiguration { get; set; }

		public DisplayViewModel Display { get; set; }

		internal async void OnSelectedVariantIndexChanged()
		{
			if (IsLoading || SelectedVariantIndex < 0)
			{
				// Ignore initial set value
				return;
			}

			try
			{
				await SetParametersAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(SetParametersAsync);

		private async Task SetParametersAsync()
		{
			var sceneConfigurationDialog = new SceneConfigurationDialog(SelectedVariant, SceneConfiguration?.Configuration);
			if (await sceneConfigurationDialog.ShowAsync() == ContentDialogResult.Primary)
			{
				SceneConfiguration = new SceneConfigurationViewModel(sceneConfigurationDialog.Model.Result);
				Display = new DisplayViewModel(sceneConfigurationDialog.Model.Result);
				StartSimulation();
			}
		}

		private void StartSimulation()
		{
			if (_controller == null || SceneConfiguration == null)
			{
				return;
			}

			IsLoading = true;
			switch (SceneConfiguration.Configuration.InputVariant)
			{
				case InputVariant.ContinuityEquation:
					_controller.SetVariantRenderer(new ContinuityEquationRenderer(_controller));
					break;
				case InputVariant.BernoulliEquationWithoutHeightChange:
					_controller.SetVariantRenderer(new BernoulliEquationWithoutHeightChangeRenderer(_controller));
					break;
				case InputVariant.BernoulliEquationWithHeightChange:
					_controller.SetVariantRenderer(new BernoulliEquationWithHeightChangeRenderer(_controller));
					break;
				case InputVariant.RealFluidMovement:
					_controller.SetVariantRenderer(new RealFluidMovementRenderer(_controller));
					break;
			}

			_controller.StartSimulation(SceneConfiguration.Configuration);

			IsLoading = false;
		}
	}
}
