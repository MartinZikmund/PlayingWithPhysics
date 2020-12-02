using Physics.ElectricParticle.Dialogs;
using Physics.ElectricParticle.Logic;
using Physics.ElectricParticle.Models;
using Physics.ElectricParticle.UserControls;
using Physics.ElectricParticle.ValuesTable;
using Physics.ElectricParticle.ViewInteractions;
using Physics.ElectricParticle.ViewModels.Inputs;
using Physics.ElectricParticle.Views;
using Physics.Shared.UI.Infrastructure.Topics;
using Physics.Shared.UI.Localization;
using Physics.Shared.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace Physics.ElectricParticle.ViewModels
{
    public class MainViewModel : SimulationViewModelBase<SimulationNavigationModel>
    {
        private IMainViewInteraction _interaction;

        public MainViewModel()
        {
        }
        public Visibility RadiationVisibility { get; set; }
        public Visibility EasyDifficultyInputsVisibility { get; set; }
        public Visibility AdvancedDifficultyInputsVisibility { get; set; }

        public ICommand ShowValuesTableCommand => GetOrCreateAsyncCommand(ShowValuesTableAsync);

        private async Task ShowValuesTableAsync()
        {
            var newWindow = await AppWindow.TryCreateAsync();
            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(ValuesTablePage));
            var physicsService = new PhysicsService(Setup as MotionSetup);
            var valuesTableService = new TableService(physicsService);
            var valuesTableViewModel = new ValuesTableDialogViewModel(valuesTableService);
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
        }

        private DifficultyOption _difficulty;
        private int _selectedVariantIndex;

        public override void Prepare(SimulationNavigationModel parameter)
        {
            _difficulty = parameter.Difficulty;
            if (_difficulty == DifficultyOption.Easy)
            {
                //Allow for basic variants enable inputs
                EasyDifficultyInputsVisibility = Visibility.Visible;
                AdvancedDifficultyInputsVisibility = Visibility.Collapsed;
                RadiationVisibility = Visibility.Collapsed;
            }
            else
            {
                //Allow for advanced variants, enable advanced inputs
                EasyDifficultyInputsVisibility = Visibility.Collapsed;
                AdvancedDifficultyInputsVisibility = Visibility.Visible;
            }
        }

        public int SelectedVariantIndex
        {
            get => _selectedVariantIndex;

            set
            {
                _selectedVariantIndex = value;
            }
        }
        PlaneOrientation Variant => (PlaneOrientation)(_difficulty == DifficultyOption.Advanced ? _selectedVariantIndex + 3 : _selectedVariantIndex);
        public IInputViewModel InputViewModel { get; set; }

        public ICommand AddTrajectoryCommand => GetOrCreateAsyncCommand(async () =>
        {
            var dialog = new AddOrUpdateMovementDialog(new MainInputViewModel(Variant));
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Setup = dialog.Setup;
				var res = new PhysicsService(Setup as MotionSetup).PrimaryAxisCoordinate(0);
                Motion = new MotionViewModel(Setup);
                RestartSimulation();
            }
        });

        public MotionViewModel Motion { get; set; }

        public IMotionSetup Setup { get; set; }

        internal void SetViewInteraction(IMainViewInteraction interaction)
        {
            _interaction = interaction;
        }

        public ICommand DrawCommand => GetOrCreateCommand(DrawMotion);


        public Visibility ShowCurrentValues => (Setup != null) ? Visibility.Visible : Visibility.Collapsed;
        public void DrawMotion()
        {
        }
        public string DrawingContent { get; set; }

        private void RestartSimulation()
        {
            //if (_interaction == null) return;
            //var controller = _interaction.PrepareController(SelectedVariant);
            //SimulationPlayback.SetController(controller);
            //controller.StartSimulation(Motions.Select(m => m.Motion).ToArray());
        }

        //public string LocalizedAddMotionText => (SelectedVariant == VelocityVariant.Radiation) ? Localizer.Instance["AddMotionRadiation"] : Localizer.Instance["AddMotion"];
        //internal void Delete(RadiationVariantStateViewModel radiationVariantStateViewModel)
        //{
        //    Motions.Remove(radiationVariantStateViewModel);
        //    RestartSimulation();
        //}
    }
}
