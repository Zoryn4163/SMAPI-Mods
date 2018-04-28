namespace FishingMod.Framework
{
    internal class ModConfig
    {
        public bool AlwaysPerfect { get; set; }
        public bool AlwaysFindTreasure { get; set; }
        public bool InstantCatchFish { get; set; }
        public bool InstantCatchTreasure { get; set; }
        public bool EasierFishing { get; set; }
        public float FishDifficultyMultiplier { get; set; } = 1;
        public float FishDifficultyAdditive { get; set; }
        public float LossAdditive { get; set; } = 2 / 1000f;

        public bool InfiniteTackle { get; set; } = true;
        public bool InfiniteBait { get; set; } = true;
    }
}
