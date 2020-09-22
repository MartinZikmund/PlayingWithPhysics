using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

namespace Physics.Shared.UI.Controls
{
    public sealed partial class AboutApp : UserControl
    {
        public AboutApp()
        {
            this.InitializeComponent();           
        }

        public string Version
        {
            get
            {
                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;

                return "Verze: " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }
    }
}
