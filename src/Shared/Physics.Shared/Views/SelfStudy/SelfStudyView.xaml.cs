using Physics.Shared.ViewModels;
using Windows.UI.Xaml;

namespace Physics.Shared.Views.SelfStudy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelfStudyView : BaseView
    {
        public SelfStudyView()
        {
            this.InitializeComponent();
            this.DataContextChanged += SelfStudyView_DataContextChanged;
        }

        public SelfStudyViewModel Model { get; private set; }

        private void SelfStudyView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Model = (SelfStudyViewModel)args.NewValue;
        }
    }
}
