using Physics.Shared.UI.Rendering.Skia;
using Physics.Shared.UI.Views;
using Physics.CyclicProcesses.Rendering;
using Physics.CyclicProcesses.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace Physics.CyclicProcesses.Views
{
	public sealed partial class EasyVariantView : EasyVariantViewBase
	{
		public EasyVariantView()
		{
			InitializeComponent();
			AnimationSelection_SelectionChanged(null, null);
		}

		private void AnimationSelection_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
		{
			if (AnimationView == null)
			{
				return;
			}

			switch (AnimationSelection.SelectedIndex)
			{
				case 0:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/adiabat.html"));
					break;
				case 1:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/izobar.html"));
					break;
				case 2:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/izoterm.html"));
					break;
				case 3:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/izochor.html"));
					break;
				case 4:
					AnimationView.Navigate(new Uri("ms-appx-web:///Assets/Animations/motor.html"));
					break;
			}
		}
	}

	public class EasyVariantViewBase : Page
	{
	}
}
