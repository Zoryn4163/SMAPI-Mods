namespace BetterRNG.Framework;

internal class WeightedGeneric<T> : IWeighted
{
    public int Weight { get; }
    public T Value { get; }

    public WeightedGeneric(int weight, T value)
    {
        this.Weight = weight;
        this.Value = value;
    }

    public static WeightedGeneric<T> Create(int weight, T value)
    {
        return new WeightedGeneric<T>(weight, value);
    }
}
