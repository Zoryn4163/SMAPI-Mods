namespace BetterRNG.Framework;

internal class WeightedGeneric<T> : IWeighted
{
    public object Value { get; set; }
    public int Weight { get; set; }

    public T TValue => (T)this.Value;

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
