using Microsoft.Toolkit.Uwp.UI;
using Physics.SelfStudy.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Physics.SelfStudy.Views
{
    public sealed partial class SelfStudyView : Page
    {
        public SelfStudyView()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        public SelfStudyViewModel ViewModel { get; } = new SelfStudyViewModel();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadAsync((string)e.Parameter);
        }

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ChapterContentsList.Focus(FocusState.Programmatic);
		}

		private void ChapterContentsList_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
		{
			var scroller = ChapterContentsList.FindDescendant<ScrollViewer>();
			if (scroller is null)
			{
				return;
			}
			var verticalOffset = scroller.VerticalOffset;
			var horizontalOffset = scroller.HorizontalOffset;
			var handled = true;
			switch (e.Key)
			{
				case Windows.System.VirtualKey.Up: verticalOffset = verticalOffset - 10 < 0 ? 0 : verticalOffset - 10; break;
				case Windows.System.VirtualKey.Down: verticalOffset = verticalOffset + 10 > scroller.ScrollableHeight ? scroller.ScrollableHeight : verticalOffset + 10; break;
				default:
					{
						handled = false;
						break;
					}
			}

			scroller.ChangeView(horizontalOffset, verticalOffset, 1);
			e.Handled = handled;
		}
	}
}
