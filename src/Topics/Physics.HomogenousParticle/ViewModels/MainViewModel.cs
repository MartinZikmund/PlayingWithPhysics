using Microsoft.Toolkit.Uwp.Helpers;
using Physics.HomogenousMovement.ViewModels;
using Physics.HomogenousParticle.Dialogs;
using Physics.HomogenousParticle.Models;
using Physics.HomogenousParticle.Services;
using Physics.HomogenousParticle.ValuesTable;
using Physics.HomogenousParticle.ViewInteractions;
using Physics.HomogenousParticle.ViewModels.Inputs;
using Physics.HomogenousParticle.ViewModels.State;
using Physics.HomogenousParticle.Views;
using Physics.HomongenousParticle.Logic.PhysicsServices;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Physics.HomogenousParticle.ViewModels
{
    public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>
    {
        private IMainViewInteraction _interaction;

        public MainViewModel()
        {            
        }

        public Visibility RadiationVisibility { get; set; }

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

        private async Task ShowValuesTableAsync()
        {
            //TODO: FIX!!!!
            //if (_tableWindowIds.TryGetValue(viewModel, out var window))
            //{
            //    await window.TryShowAsync();
            //    return;
            //};
            var newWindow = await AppWindow.TryCreateAsync();
            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(ValuesTablePage));
            var physicsService = new PerpendicularPhysicsService(Motions[0] as PerpendicularMotionSetup);
            var valuesTableService = new PerpendicularTableService(physicsService);
            var valuesTableViewModel = new PerpendicularTableDialogViewModel(valuesTableService);
            valuesTableViewModel.TimeInterval = (float)(physicsService.MaxT / 20);
            (appWindowContentFrame.Content as ValuesTablePage).Initialize(valuesTableViewModel);
            // Attach the XAML content to the window.
            ElementCompositionPreview.SetAppWindowContent(newWindow, appWindowContentFrame);
            newWindow.Title = "Table";

            newWindow.TitleBar.BackgroundColor = (Color)Application.Current.Resources["AppThemeColor"];
            newWindow.TitleBar.ForegroundColor = Colors.White;
            newWindow.TitleBar.InactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.InactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.TitleBar.ButtonBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.ButtonForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.TitleBar.ButtonInactiveBackgroundColor = newWindow.TitleBar.BackgroundColor;
            newWindow.TitleBar.ButtonInactiveForegroundColor = newWindow.TitleBar.ForegroundColor;
            newWindow.RequestSize(new Size(600, 400));
            var shown = await newWindow.TryShowAsync();
            //if (shown)
            //{
            //    _tableWindowIds.Add(viewModel, newWindow);
            //}
        }

        public override void Prepare(SimulationNavigationModel parameter)
        {
            OnSelectedVariantIndexChanged();
            if (parameter.Difficulty == DifficultyOption.Easy)
            {
                RadiationVisibility = Visibility.Collapsed;
            }
        }

        public int SelectedVariantIndex { get; set; }

        public VelocityVariant SelectedVariant => (VelocityVariant)SelectedVariantIndex;

        public void OnSelectedVariantIndexChanged()
        {
            switch (SelectedVariant)
            {
                case VelocityVariant.Zero:
                    VariantInputViewModel = new ZeroVariantInputViewModel();
                    break;
                case VelocityVariant.Parallel:
                    VariantInputViewModel = new ParallelVariantInputViewModel();
                    break;
                case VelocityVariant.Perpendicular:
                    VariantInputViewModel = new PerpendicularVariantInputViewModel();
                    break;
                case VelocityVariant.Radiation:
                    VariantInputViewModel = new RadiationVariantInputViewModel();
                    break;
            }
        }

        public IVariantInputViewModel VariantInputViewModel { get; set; }

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
        {
            var dialog = new AddOrUpdateMotionDialog(VariantInputViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {                                
                if (SelectedVariant == VelocityVariant.Zero || 
                    SelectedVariant == VelocityVariant.Parallel || 
                    SelectedVariant == VelocityVariant.Perpendicular)
                {
                    Motions.Clear();
                }
                if (SelectedVariant == VelocityVariant.Radiation)
                {
                    if (Motions.Any(m=>!(m.Motion is RadiationMotionSetup)))
                    {
                        Motions.Clear();
                    }   
                }
                Motions.Add(VariantStateViewModelFactory.Create(this, dialog.Setup));
                RestartSimulation();
            }
        });

        internal void SetViewInteraction(IMainViewInteraction interaction)
        {
            _interaction = interaction;
        }

        public ObservableCollection<IVariantStateViewModel> Motions { get; } = new ObservableCollection<IVariantStateViewModel>();

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);

        public void DrawMotion()
        {
        }

        public string DrawingContent { get; set; }

        public Visibility IsSecondStepVisible { get; set; } = Visibility.Visible;

        private void RestartSimulation()
        {
            if (_interaction == null) return;
            var controller = _interaction.PrepareController(SelectedVariant);
            SimulationPlayback.SetController(controller);
            controller.StartSimulation(Motions.Select(m=>m.Motion).ToArray());
        }

        public string LocalizedAddMotionText => (SelectedVariant == VelocityVariant.Radiation) ? Localizer.Instance["AddMotionRadiation"] : Localizer.Instance["AddMotion"];
        internal void Delete(RadiationVariantStateViewModel radiationVariantStateViewModel)
        {
            Motions.Remove(radiationVariantStateViewModel);
            RestartSimulation();
        }
    }
}
