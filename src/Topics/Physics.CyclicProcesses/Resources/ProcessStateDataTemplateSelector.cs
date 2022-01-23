using System;
using Physics.CyclicProcesses.ViewModels.Process;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.CyclicProcesses.Resources
{
	public class ProcessStateDataTemplateSelector : DataTemplateSelector
	{
		protected override DataTemplate SelectTemplateCore(object item) =>
			(DataTemplate)(item switch
			{
				IsobaricStateViewModel _ => Application.Current.Resources["IsobaricStateDataTemplate"],
				IsochoricStateViewModel _ => Application.Current.Resources["IsochoricStateDataTemplate"],
				IsotermicStateViewModel _ => Application.Current.Resources["IsotermicStateDataTemplate"],
				AdiabaticStateViewModel _ => Application.Current.Resources["AdiabaticStateDataTemplate"],
				StirlingEngineStateViewModel _ => Application.Current.Resources["StirlingEngineStateDataTemplate"],
				_ => null
			});

		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) => SelectTemplateCore(item);
	}
}
