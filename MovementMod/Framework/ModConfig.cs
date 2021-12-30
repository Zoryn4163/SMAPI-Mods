using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace MovementMod.Framework
{
    /// <summary>The mod configuration.</summary>
    internal class ModConfig
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The player speed to add when running (or 0 for no change).</summary>
        public int PlayerRunningSpeed { get; set; } = 5;

        /// <summary>The player speed to add when riding the horse (or 0 for no change).</summary>
        public int HorseSpeed { get; set; } = 5;

        /// <summary>The key which causes the player to sprint.</summary>
        public KeybindList SprintKey { get; set; } = new(SButton.LeftShift);

        /// <summary>The keys which reload the mod config.</summary>
        public KeybindList ReloadKey { get; set; } = new(SButton.F5);

        /// <summary>The multiplier applied to the player speed when sprinting.</summary>
        public int PlayerSprintingSpeedMultiplier { get; set; } = 2;

        /// <summary>The stamina drain each second while sprinting.</summary>
        public float SprintingStaminaDrainPerSecond { get; set; } = 15;
    }
}
