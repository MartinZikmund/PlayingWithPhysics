using System;
using Physics.CyclicProcesses.ViewModels;
using Physics.Shared.Views;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Physics.CyclicProcesses.Views
{
	public sealed partial class EasyVariantView : EasyVariantViewBase
	{
		public EasyVariantView()
		{
			InitializeComponent();
			DataContextChanged += ViewContextChanged;
			Loaded += EasyVariantView_Loaded;
		}

		private void EasyVariantView_Loaded(object sender, RoutedEventArgs e)
		{
			AnimationSelection_SelectionChanged(null, null);
		}

		private void ViewContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			if (args.NewValue is EasyVariantViewModel model)
			{
				Model = model;
			}
		}

		public EasyVariantViewModel Model { get; private set; }

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
					mediaUri = new Uri("ms-appx:///Assets/Animations/adiabat.mp4");
					break;
				case Logic.ProcessType.Isobaric:
					mediaUri = new Uri("ms-appx:///Assets/Animations/izobar.mp4");
					break;
				case Logic.ProcessType.Isotermic:
					mediaUri = new Uri("ms-appx:///Assets/Animations/izoterm.mp4");
					break;
				case Logic.ProcessType.Isochoric:
					mediaUri = new Uri("ms-appx:///Assets/Animations/izochor.mp4");
					break;
				case Logic.ProcessType.StirlingEngine:
					mediaUri = new Uri("ms-appx:///Assets/Animations/motor.mp4");
					break;
			}
			if (mediaUri != null)
			{
				var source = MediaSource.CreateFromStorageFile(await StorageFile.GetFileFromApplicationUriAsync(mediaUri));
				Player.Source = source;
			}
		}
	}

	public class EasyVariantViewBase : BaseView
	{
	}
}
