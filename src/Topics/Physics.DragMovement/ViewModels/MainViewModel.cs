using MvvmCross.Base;
using Physics.DragMovement.Models;
using Physics.DragMovement.ViewInteractions;
using Physics.DragMovement.Views;
using Physics.Shared.Services.Preferences;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.DragMovement.ViewModels
{
    public class MainViewModel : DragMovementSimulationViewModelBase
    {
        private IMainViewInteraction _mainViewInteraction;

        public async void SetViewInteraction(IMainViewInteraction mainViewInteraction)
        {
            _mainViewInteraction = mainViewInteraction;
            _controller = _mainViewInteraction.Initialize(Difficulty);
            SimulationPlayback.SetController(_controller);
            if (_startWithController)
            {
                await Task.Delay(1000);
                await StartSimulationAsync();
            }
        }

        public MainViewModel(IMvxMainThreadAsyncDispatcher dispatcher, IPreferences preferences) : base(dispatcher, preferences)
        {
        }
    }
}
