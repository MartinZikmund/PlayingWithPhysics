using Physics.ElectricParticle.Game;
using Physics.ElectricParticle.Rendering;
using Physics.ElectricParticle.ViewModels;
using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace Physics.ElectricParticle.Views
{
	public sealed partial class GameView : GameViewBase
	{
		public GameView()
		{
			InitializeComponent();
		}

		private void SliderGettingFocus(Windows.UI.Xaml.UIElement sender, GettingFocusEventArgs args)
		{
			args.Cancel = true;
		}
	}

	public class GameViewBase : BaseSkiaView<GameViewModel, GameCanvasController>
	{
		public GameViewBase()
		{
			Loaded += PageLoaded;
		}

		private void PageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			CoreWindow.GetForCurrentThread().KeyDown += PageKeyDown;
			CoreWindow.GetForCurrentThread().KeyUp += PageKeyUp;
			Focus(Windows.UI.Xaml.FocusState.Programmatic);
		}

		private void PageKeyDown(CoreWindow sender, KeyEventArgs args)
		{
			switch (args.VirtualKey)
			{
				case VirtualKey.Up:
				case VirtualKey.W:
				case VirtualKey.GamepadDPadUp:
					KeyboardState.IsUpPressed = true;
					break;
				case VirtualKey.Down:
				case VirtualKey.S:
				case VirtualKey.GamepadDPadDown:
					KeyboardState.IsDownPressed = true;
					break;
				case VirtualKey.Left:
				case VirtualKey.A:
				case VirtualKey.GamepadDPadLeft:
					KeyboardState.IsLeftPressed = true;
					break;
				case VirtualKey.Right:
				case VirtualKey.D:
				case VirtualKey.GamepadDPadRight:
					KeyboardState.IsRightPressed = true;
					break;
			}
		}

		private void PageKeyUp(CoreWindow sender, KeyEventArgs args)
		{
			switch (args.VirtualKey)
			{
				case VirtualKey.Up:
				case VirtualKey.W:
				case VirtualKey.GamepadDPadUp:
					KeyboardState.IsUpPressed = false;
					break;
				case VirtualKey.Down:
				case VirtualKey.S:
				case VirtualKey.GamepadDPadDown:
					KeyboardState.IsDownPressed = false;
					break;
				case VirtualKey.Left:
				case VirtualKey.A:
				case VirtualKey.GamepadDPadLeft:
					KeyboardState.IsLeftPressed = false;
					break;
				case VirtualKey.Right:
				case VirtualKey.D:
				case VirtualKey.GamepadDPadRight:
					KeyboardState.IsRightPressed = false;
					break;
			}
		}

		public KeyboardState KeyboardState { get; } = new KeyboardState();

		protected override ISkiaCanvas CreateSkiaCanvas() => new SkiaCanvas();

		protected override GameCanvasController CreateController(ISkiaCanvas canvas) => new GameCanvasController(canvas, KeyboardState);
	}
}
