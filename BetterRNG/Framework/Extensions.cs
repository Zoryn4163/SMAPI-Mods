using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterRNG.Framework;

internal static class Extensions
{
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            throw new ArgumentNullException(nameof(enumerable));
        }

        var list = enumerable as IList<T> ?? enumerable.ToList();
        return list.Count == 0 ? default : list[ModEntry.Twister.Next(0, list.Count)];
    }

    public static T Choose<T>(this T[] list) where T : IWeighted
    {
        return Weighted.Choose(list);
    }
}
