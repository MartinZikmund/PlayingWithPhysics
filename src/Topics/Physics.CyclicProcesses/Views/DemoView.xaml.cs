using System;
using Physics.CyclicProcesses.ViewModels;
using Physics.Shared.Views;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;

namespace Physics.CyclicProcesses.Views
{
	public sealed partial class DemoVariantView : DemoVariantViewBase
	{
		private AppBarButton _playbackRateButton;

		public DemoVariantView()
		{
			InitializeComponent();
			DataContextChanged += ViewContextChanged;
			Loaded += DemoVariantView_Loaded;
		}

		private void OnFlyoutChanged(DependencyObject sender, DependencyProperty dp)
		{
			var flyout = _playbackRateButton.Flyout as MenuFlyout;
			if (flyout == null)
			{
				return;
			}

			flyout.Opening += Flyout_Opening;
		}

		private void Flyout_Opening(object sender, object e)
		{
			var flyout = (MenuFlyout)sender;
			flyout.Items[0].Visibility = Visibility.Collapsed;
			flyout.Items[1].Visibility = Visibility.Collapsed;
		}

		private void DemoVariantView_Loaded(object sender, RoutedEventArgs e)
		{
			AnimationSelection_SelectionChanged(null, null);

			_playbackRateButton = MediaTransport.FindDescendant("PlaybackRateButton") as AppBarButton;
			_playbackRateButton.RegisterPropertyChangedCallback(Button.FlyoutProperty, OnFlyoutChanged);
		}

		private void ViewContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			if (args.NewValue is DemoVariantViewModel model)
			{
				Model = model;
			}
		}

		public DemoVariantViewModel Model { get; private set; }

		private async void AnimationSelection_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
		{
			if (Player == null || AnimationSelection.SelectedIndex < 0)
			{
				return;
			}

			Uri mediaUri = null;
			var processType = Model.ProcessTypes[AnimationSelection.SelectedIndex];
			switch (processType)
			{
				case Logic.ProcessType.Adiabatic:
					mediaUri = new Uri("ms-appx:///Assets/Demo/adiabat.mp4");
					break;
				case Logic.ProcessType.Isobaric:
					mediaUri = new Uri("ms-appx:///Assets/Demo/izobar.mp4");
					break;
				case Logic.ProcessType.Isotermic:
					mediaUri = new Uri("ms-appx:///Assets/Demo/izoterm.mp4");
					break;
				case Logic.ProcessType.Isochoric:
					mediaUri = new Uri("ms-appx:///Assets/Demo/izochor.mp4");
					break;
				case Logic.ProcessType.StirlingEngine:
					mediaUri = new Uri("ms-appx:///Assets/Demo/motor.mp4");
					break;
			}
			if (mediaUri != null)
			{
				var source = MediaSource.CreateFromStorageFile(await StorageFile.GetFileFromApplicationUriAsync(mediaUri));
				Player.Source = source;
			}
		}
	}

	public class DemoVariantViewBase : BaseView
	{
	}
}
