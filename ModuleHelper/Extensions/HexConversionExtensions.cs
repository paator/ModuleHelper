using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ModuleHelper.Extensions
{
    public static class HexConversionExtensions
    {
        public static void SwitchBetweenHexAndDec(this ICollection<string> collection, bool isUsingHex)
        {
            if (isUsingHex)
            {
                var enumeration = collection.Select(x => int.Parse(x).ToString("X"));
                collection = new ObservableCollection<string>(enumeration);
            }
            else
            {
                var enumeration = collection.Select(x => Convert.ToInt64(x, 16).ToString());
                collection = new ObservableCollection<string>(enumeration);
            }
        }

        public static ObservableCollection<string> ToStringHex(this List<int> numbers, bool convertToHex)
        {
            IList<string> converted;

            if (convertToHex)
            {
                converted  = numbers.ConvertAll(x => x.ToString("X"));
            }
            else
            {
                converted = numbers.ConvertAll(x => x.ToString());
            }

            return new ObservableCollection<string>(converted);
        }
    }
}