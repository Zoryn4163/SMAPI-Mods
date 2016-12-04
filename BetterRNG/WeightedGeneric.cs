namespace BetterRNG
{
    public class WeightedGeneric<T> : IWeighted
    {
        public object Value { get; set; }
        public int Weight { get; set; }

        public T TValue => (T)Value;

        public WeightedGeneric()
        {
            //Nothing
        }

        public WeightedGeneric(int weight, T value)
        {
            Weight = weight;
            Value = value;
        }

        public static WeightedGeneric<Ty> Create<Ty>(int weight, Ty value)
        {
            return new WeightedGeneric<Ty>(weight, value);
        }
    }
}