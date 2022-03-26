using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.ElectricParticle.UserControls
{
    public class InputControlContentTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
			if (item == null)
			{
				return base.SelectTemplateCore(item, container);
			}
            var viewModelName = item.GetType().Name;
            var viewModelPrefix = viewModelName.Substring(0, viewModelName.Length - "viewModel".Length);
            return (DataTemplate)Application.Current.Resources[viewModelPrefix + "DataTemplate"];
        }
    }
}
