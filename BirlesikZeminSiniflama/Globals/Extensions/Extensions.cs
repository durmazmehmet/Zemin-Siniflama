using System;
using System.Collections.Generic;

namespace com.mehmetdurmaz.SoilClassfication.Globals.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var i in enumerable)
                action(i);

            return enumerable;
        }
    }
}
