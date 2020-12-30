using MvvmCross.ViewModels;
using Windows.UI;

namespace Physics.Shared.UI.Models.Input
{
	public class SimulationColors : MvxNotifyPropertyChanged
    {
		private Color _selected = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1");

		public Color[] Palette { get; } = new Color[]
		{
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#0063B1"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#2D7D9A"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#E81123"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#881798"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#498205"),
			Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#515C6B"),
		};

		public Color Selected
		{
			get => _selected;
			set
			{
				if (value != _selected)
				{
					_selected = value;
					RaisePropertyChanged();
				}
			}
		}
	}
}
