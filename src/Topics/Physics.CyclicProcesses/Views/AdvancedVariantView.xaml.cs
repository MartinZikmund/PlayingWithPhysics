using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Physics.CyclicProcesses.Rendering;
using Physics.CyclicProcesses.ViewModels;
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

namespace Physics.CyclicProcesses.Views
{
	public sealed partial class AdvancedVariantView : AdvancedVariantViewBase
	{
		public AdvancedVariantView() => InitializeComponent();
	}

	public class AdvancedVariantViewBase : BaseSkiaView<AdvancedVariantViewModel, CyclicProcessesCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override CyclicProcessesCanvasController CreateController(ISkiaCanvas canvas) =>
			new CyclicProcessesCanvasController(canvas);
	}
}
