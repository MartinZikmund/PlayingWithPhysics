using System;
using Physics.WaveInterference.Rendering;
using Physics.WaveInterference.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Physics.Shared.UI.Controls;

namespace Physics.WaveInterference.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView()
		{
			this.InitializeComponent();
		}

		internal void FocusSimulationControls() => SimulationControls.Focus(Windows.UI.Xaml.FocusState.Programmatic);
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, CompoundOscillationsController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override CompoundOscillationsController CreateController(ISkiaCanvas canvas) => new CompoundOscillationsController(canvas);
	}
}
