using System;
using System.Linq;

namespace BetterRNG.Framework;

internal static class Weighted
{
    public static T Choose<T>(T[] list)
        where T : IWeighted
    {
        if (!list.Any())
            throw new InvalidOperationException("Can't choose a value because no options were provided.");

        int totalWeight = list.Sum(c => c.Weight);
        int choice = ModEntry.Twister.Next(totalWeight);
        int sum = 0;

        foreach (T obj in list)
        {
            for (float i = sum; i < obj.Weight + sum; i++)
            {
                if (i >= choice)
                    return obj;
            }
            sum += obj.Weight;
        }

        return list.First();
    }
}
