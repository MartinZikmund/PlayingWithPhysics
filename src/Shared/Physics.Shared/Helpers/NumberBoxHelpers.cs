using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.NumberFormatting;

namespace Physics.Shared.UI.Helpers
{
    public static class NumberBoxHelpers
    {
        public static void SetupFormatting(this NumberBox numberBox, double increment = 0.1, int integerDigits = 1, int fractionDigits = 1, double smallChange = 1)
        {
            IncrementNumberRounder rounder = new IncrementNumberRounder();
            rounder.Increment = increment;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = integerDigits;
            if (smallChange == Math.Floor(smallChange))
            {
                formatter.FractionDigits = 0;
            }
            else
            {
                formatter.FractionDigits = fractionDigits;
            }
            formatter.NumberRounder = rounder;

            numberBox.SmallChange = smallChange;
            numberBox.NumberFormatter = formatter;
        }
    }
}
