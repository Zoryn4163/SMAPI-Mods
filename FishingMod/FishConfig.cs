using StardewModdingAPI;

namespace FishingMod
{
    public class FishConfig : Config
    {
        public bool AlwaysPerfect { get; set; }
        public bool AlwaysFindTreasure { get; set; }
        public bool InstantCatchFish { get; set; }
        public bool InstantCatchTreasure { get; set; }
        public bool EasierFishing { get; set; }
        public float FishDifficultyMultiplier { get; set; }
        public float FishDifficultyAdditive { get; set; }
        public float LossAdditive { get; set; }

        public bool InfiniteTackle { get; set; }
        public bool InfiniteBait { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            AlwaysPerfect = false;
            AlwaysFindTreasure = false;
            InstantCatchFish = false;
            InstantCatchTreasure = false;
            EasierFishing = false;
            FishDifficultyMultiplier = 1;
            FishDifficultyAdditive = 0;
            LossAdditive = 2 / 1000f;

            InfiniteTackle = true;
            InfiniteBait = true;
            return this as T;
        }
    }
}