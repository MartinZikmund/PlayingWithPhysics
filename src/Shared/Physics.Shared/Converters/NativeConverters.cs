using MvvmCross.Converters;
using MvvmCross.Platforms.Uap.Converters;
using MvvmCross.Plugin.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Shared.UI.Converters
{
    public class NativeBoolToVisibilityConverter : MvxNativeValueConverter<MvxVisibilityValueConverter>
    {
    }

    public class NativeInvertedBoolToVisibilityConverter : MvxNativeValueConverter<MvxInvertedVisibilityValueConverter>
    {
    }
}
