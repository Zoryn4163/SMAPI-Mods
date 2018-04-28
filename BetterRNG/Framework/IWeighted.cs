namespace BetterRNG.Framework
{
    internal interface IWeighted
    {
        int Weight { get; set; }
        object Value { get; set; }
    }
}
