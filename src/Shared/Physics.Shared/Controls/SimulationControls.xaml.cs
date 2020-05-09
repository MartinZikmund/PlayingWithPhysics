using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.Shared.UI.Controls
{
    public sealed partial class SimulationControls : UserControl
    {
        public SimulationControls()
        {
            this.InitializeComponent();
        }
               
        public bool IsTimeSliderVisible
        {
            get { return (bool)GetValue(IsTimeSliderVisibleProperty); }
            set { SetValue(IsTimeSliderVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsTimeSliderVisibleProperty =
            DependencyProperty.Register("IsTimeSliderVisible", typeof(bool), typeof(SimulationControls), new PropertyMetadata(true));
               
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SimulationControls), new PropertyMetadata(0));
               
        public double MaxTime
        {
            get { return (double)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }

        public static readonly DependencyProperty MaxTimeProperty =
            DependencyProperty.Register("MaxTime", typeof(double), typeof(SimulationControls), new PropertyMetadata(0));       
    }
}
