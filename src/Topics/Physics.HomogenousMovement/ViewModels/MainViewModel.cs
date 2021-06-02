using Physics.HomogenousMovement.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Physics.HomogenousMovement.PhysicsServices;
using Physics.HomogenousMovement.Rendering;
using Windows.UI.Xaml.Controls;
using Physics.Shared.ViewModels;
using Windows.ApplicationModel.Resources;
using Physics.HomogenousMovement.Dialogs;
using Physics.HomogenousMovement.Logic.PhysicsServices;
using Windows.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using Physics.HomogenousMovement.Models;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using MvvmCross.Base;
using Physics.HomogenousMovement.ViewInteractions;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Physics.Shared.Services.Preferences;
using Physics.HomogenousMovement.Services.Preferences;

namespace Physics.HomogenousMovement.ViewModels
{
    public class MainViewModel : ProjectileMotionSimulationViewModelBase
    {
        private IMainViewInteraction _mainViewInteraction;

        public async void SetViewInteraction(IMainViewInteraction mainViewInteraction)
        {
            _mainViewInteraction = mainViewInteraction;
            _controller = _mainViewInteraction.Initialize(Difficulty);
            SimulationPlayback.SetController(_controller);
			IsPaused = PauseAfterChanges;
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
