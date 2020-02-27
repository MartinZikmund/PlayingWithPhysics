using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.NumberFormatting;

namespace Physics.Shared.Helpers
{
    public static class NumberBoxHelpers
    {
        public static DecimalFormatter SetupFromatting(float increment = 0.1f, int integerDigits = 1, int fractionDigits = 1)
        {
            IncrementNumberRounder rounder = new IncrementNumberRounder();
            rounder.Increment = 0.1;
            rounder.RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp;

            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 1;
            formatter.FractionDigits = 1;
            formatter.NumberRounder = rounder;
            return formatter;
        }
    }
}
