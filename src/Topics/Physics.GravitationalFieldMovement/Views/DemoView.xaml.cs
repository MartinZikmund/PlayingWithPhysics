using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.GravitationalFieldMovement.Rendering;
using Physics.GravitationalFieldMovement.ViewModels;
using Physics.Shared.UI.Helpers;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Physics.GravitationalFieldMovement.Views
{
	public sealed partial class DemoView : DemoViewBase
	{
		public DemoView()
		{
			InitializeComponent();
			SetupNumberBoxFormatting();
		}
		public void SetupNumberBoxFormatting()
		{
			DeltaInput.SetupFormatting(0.001, 1, 3, 0.001, 0.001);
		}
	}

	public class DemoViewBase : BaseSkiaView<DemoViewModel, GravitationalFieldMovementCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GravitationalFieldMovementCanvasController CreateController(ISkiaCanvas canvas) =>
			new GravitationalFieldMovementCanvasController(canvas);
	}
}
