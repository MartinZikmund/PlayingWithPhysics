﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.LawOfConservationOfMomentum.Dialogs;
using Physics.LawOfConservationOfMomentum.Logic;
using Physics.LawOfConservationOfMomentum.Rendering;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Models.Navigation;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.UI.Views.Interactions;
using Windows.UI.Xaml.Controls;

namespace Physics.LawOfConservationOfMomentum.ViewModels
{
	public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>, IReceiveController<LawOfConservationOfMomentumCanvasController>
	{
		private DifficultyOption _difficulty;
		private LawOfConservationOfMomentumCanvasController _controller;

		public CollisionType[] CollisionTypes { get; set; }

		public int SelectedCollisionTypeIndex { get; set; } = 0;

		public CollisionType SelectedCollisionType => (CollisionType)SelectedCollisionTypeIndex;

		public MotionViewModel Motion { get; set; }

		public MotionSetup Setup { get; set; }

		public ICommand SetParametersCommand => GetOrCreateAsyncCommand(async () =>
		{
			SetupCollisionDialogViewModel viewModel;
			if (Motion != null && SelectedCollisionType == Motion.Setup.Type)
			{
				viewModel = new SetupCollisionDialogViewModel(SelectedCollisionType, Motion.Setup);
			}
			else
			{
				viewModel = new SetupCollisionDialogViewModel(SelectedCollisionType);
			}

			var dialog = new SetupCollisionDialog();
			dialog.DataContext = viewModel;
			var result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				Setup = viewModel.Result;
				Motion = new MotionViewModel(Setup);
				await StartSimulationAsync();
			}
		});

		public override void Prepare(SimulationNavigationModel parameter)
		{
			_difficulty = parameter.Difficulty;

			var collisionTypes = Enum.GetValues(typeof(CollisionType)).OfType<CollisionType>();

			if (_difficulty == DifficultyOption.Easy)
			{
				CollisionTypes = collisionTypes.Except(new[] { CollisionType.ImperfectlyElastic }).ToArray();
			}
			else
			{
				CollisionTypes = collisionTypes.ToArray();
			}
		}

		public void SetController(LawOfConservationOfMomentumCanvasController controller)
		{
			if (controller is null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			_controller = controller;
			SimulationPlayback.SetController(_controller);
		}

		protected async Task StartSimulationAsync()
		{
			_controller.StartSimulation(Setup);
			//_timer.Start();
		}
	}
}
