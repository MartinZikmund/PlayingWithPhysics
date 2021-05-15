using Physics.Shared.UI.Models;
using Physics.Shared.UI.ViewModels;
using Physics.Shared.Views;

namespace Physics.Shared.UI.Views
{
	public sealed partial class AppListView : BaseView
	{
		public AppListView()
		{
			InitializeComponent();
			DataContextChanged += AppListView_DataContextChanged;
		}

		public AppListViewModel Model { get; private set; }

		private void AppListView_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
		{
			Model = (AppListViewModel)args.NewValue;
		}

		private void GridView_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
		{
			var item = e.ClickedItem as AppListItemViewModel;
			if (item != null)
			{
				item.OpenCommand.Execute(null);
			}
		}
	}
}
