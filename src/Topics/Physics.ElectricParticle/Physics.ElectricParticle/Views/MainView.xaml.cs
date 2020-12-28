﻿using Physics.ElectricParticle.Rendering;
using Physics.ElectricParticle.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;

namespace Physics.ElectricParticle.Views
{
	public sealed partial class MainView : MainViewBase
	{
		public MainView()
		{
			this.InitializeComponent();
		}
	}

	public class MainViewBase : BaseSkiaView<MainViewModel, ElectricParticleCanvasController>
	{
		protected override ISkiaCanvas CreateSkiaCanvas() => new NonUiSkiaCanvas();

		protected override ElectricParticleCanvasController CreateController(ISkiaCanvas canvas) => new ElectricParticleCanvasController(canvas);
	}
}
