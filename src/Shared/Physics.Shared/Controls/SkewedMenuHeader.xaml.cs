using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.Shared.UI.Controls
{
    public sealed partial class SkewedMenuHeader : UserControl
    {
        public SkewedMenuHeader()
        {
            this.InitializeComponent();
        }

        public bool ShowBackButton
        {
            get { return (bool)GetValue(ShowBackButtonProperty); }
            set { SetValue(ShowBackButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowBackButtonProperty =
            DependencyProperty.Register(nameof(ShowBackButton), typeof(bool), typeof(SkewedMenuHeader), new PropertyMetadata(true));

        public ICommand GoBackCommand   
        {
            get { return (ICommand)GetValue(GoBackCommandProperty); }
            set { SetValue(GoBackCommandProperty, value); }
        }

        public static readonly DependencyProperty GoBackCommandProperty =
            DependencyProperty.Register(nameof(GoBackCommand), typeof(ICommand), typeof(SkewedMenuHeader), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SkewedMenuHeader), new PropertyMetadata(null));
    }
}
