using Physics.FluidFlow.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Physics.FluidFlow.Controls
{
	public sealed partial class SceneConfigurationDialog : ContentDialog
	{
		public SceneConfigurationDialog()
		{
			this.InitializeComponent();
		}

		public SceneConfigurationDialogViewModel Model { get; } = new SceneConfigurationDialogViewModel();
	}
}
