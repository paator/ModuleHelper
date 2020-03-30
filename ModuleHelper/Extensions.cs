using System;

namespace ModuleHelper
{
    public static class Extensions
    {
        public static T Next<T>(this T src, int offset) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());

            var maxOffset = offset % 12;
            int j = Array.IndexOf<T>(Arr, src) + maxOffset;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}