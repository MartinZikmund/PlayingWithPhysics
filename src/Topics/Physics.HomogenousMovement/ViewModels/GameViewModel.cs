using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Base;
using Physics.HomogenousMovement.Gamification;
using Physics.HomogenousMovement.Rendering;
using Physics.HomogenousMovement.ViewInteractions;

namespace Physics.HomogenousMovement.ViewModels
{
    public class GameViewModel : SimulationViewModelBase
    {
        private IGameViewInteraction _gameViewInteraction;
        private GamificationCanvasController _gameController;
        private Random _randomizer = new Random();

        public GameViewModel(IMvxMainThreadAsyncDispatcher dispatcher) : base(dispatcher)
        {
        }

        public ICommand NewGameCommand => GetOrCreateAsyncCommand(StartNewGameAsync);
        
        public async void SetViewInteraction(IGameViewInteraction gameViewInteraction)
        {
            _gameViewInteraction = gameViewInteraction;
            _gameController = _gameViewInteraction.Initialize(Difficulty);
            _controller = _gameController;
            if (_startWithController)
            {
                await Task.Delay(1000);
                await StartSimulationAsync();
            }
        }

        private async Task StartNewGameAsync()
        {
            var castleDistance = _randomizer.Next(25, 50);
            var treeCount = _randomizer.Next(2, 6);
            var treeDistances = new List<int>();
            for (int treeId = 0; treeId < treeCount; treeId++)
            {
                treeDistances.Add(_randomizer.Next(0, castleDistance));
            }
            await _gameController.StartNewGameAsync(new GameSetup(castleDistance, _randomizer.Next(5, castleDistance), treeDistances.ToArray()));
        }
    }
}
