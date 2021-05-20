using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.ElectricParticle.Rendering;
using Physics.ElectricParticle.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Physics.ElectricParticle.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			this.InitializeComponent();
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, GameCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GameCanvasController CreateController(ISkiaCanvas canvas) => new GameCanvasController(canvas);
	}
}
