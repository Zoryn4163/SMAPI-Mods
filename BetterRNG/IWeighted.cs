namespace BetterRNG
{
    public interface IWeighted
    {
        int Weight { get; set; }
        object Value { get; set; }
    }
}