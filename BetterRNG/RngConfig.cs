using System.Collections.Generic;
using StardewModdingAPI;

namespace BetterRNG
{
    public class RngConfig : Config
    {
        public bool EnableDailyLuckOverride { get; set; }
        public bool EnableWeatherOverride { get; set; }
        public List<string[]> Info { get; set; }
        public int SunnyChance { get; set; }
        public int CloudySnowyChance { get; set; }
        public int RainyChance { get; set; }
        public int StormyChance { get; set; }
        public int HarshSnowyChance { get; set; }

        public bool EnableFishingTreasureOverride { get; set; }
        public float FishingTreasureChance { get; set; }
        public bool EnableFishingStuffOverride { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            EnableDailyLuckOverride = false;
            EnableWeatherOverride = false;
            Info = new List<string[]> { "The weather chances are whole numbers as percentages.|They can add up to be any number. 60 = 60% or 0.60, but you must type 60|The fishing things are not done yet and literally do nothing. Do not bother changing their values.".Split('|') };
            SunnyChance = 60;
            CloudySnowyChance = 15;
            RainyChance = 15;
            StormyChance = 5;
            HarshSnowyChance = 5;

            EnableFishingTreasureOverride = false;
            FishingTreasureChance = 1 / 16f;
            EnableFishingStuffOverride = false;
            return this as T;
        }
    }
}