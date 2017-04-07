namespace BetterRNG
{
    internal interface IWeighted
    {
        int Weight { get; set; }
        object Value { get; set; }
    }
}
