using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.ElectricParticle.Resources
{
    public class VariantStateDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Zero { get; set; }

        public DataTemplate Parallel { get; set; }

        public DataTemplate Perpendicular { get; set; }

        public DataTemplate Radiation { get; set; }

        protected override DataTemplate SelectTemplateCore(object item) =>
            GetTemplate(item);

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) =>
            GetTemplate(item);

        private DataTemplate GetTemplate(object item) =>
            item switch
            {
                //ZeroVariantStateViewModel _ => Zero,
                //ParallelVariantStateViewModel _ => Parallel,
                //PerpendicularVariantStateViewModel _ => Perpendicular,
                //RadiationVariantStateViewModel _ => Radiation,
                _ => throw new NotImplementedException(),
            };
    }
}
