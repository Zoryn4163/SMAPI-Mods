namespace BetterRNG.Framework
{
    internal class ModConfig
    {
        public bool EnableDailyLuckOverride { get; set; }
        public bool EnableWeatherOverride { get; set; }
        public string[][] Info { get; set; }
        public int SunnyChance { get; set; }
        public int CloudySnowyChance { get; set; }
        public int RainyChance { get; set; }
        public int StormyChance { get; set; }
        public int HarshSnowyChance { get; set; }

        /// <summary>Construct an instance.</summary>
        public ModConfig()
        {
            this.Info = new[]
            {
                new [] {
                    "The weather chances are whole numbers as percentages.",
                    "They can add up to be any number. 60 = 60% or 0.60, but you must type 60"
                }
            };
            this.SunnyChance = 60;
            this.CloudySnowyChance = 15;
            this.RainyChance = 15;
            this.StormyChance = 5;
            this.HarshSnowyChance = 5;
        }
    }
}
