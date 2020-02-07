using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Physics.HomogenousMovement.PhysicsServices;

namespace Physics.HomogenousMovement.PhysicsServices
{
    class MovementTypeToBoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            MovementType type = Enum.Parse<MovementType>(parameter.ToString(), true);
            return type == (MovementType)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool isChecked = (bool) value;
            if (isChecked)
            {
                return Enum.Parse<MovementType>(parameter.ToString(), true);
            }

            return 0;
        }
    }
}
