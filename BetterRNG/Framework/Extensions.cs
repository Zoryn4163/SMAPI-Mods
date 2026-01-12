namespace BetterRNG.Framework;

internal static class Extensions
{
    public static T Choose<T>(this T[] list) where T : IWeighted
    {
        return Weighted.Choose(list);
    }
}
