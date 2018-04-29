using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;

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
        public string SprintKey { get; set; } = "LeftShift";
        
        /// <summary>The multiplier applied to the player speed when sprinting.</summary>
        public int PlayerSprintingSpeedMultiplier { get; set; } = 2;

        /// <summary>The stamina drain each second while sprinting.</summary>
        public float SprintingStaminaDrainPerSecond { get; set; } = 15;


        /*********
        ** Public methods
        *********/
        public Keys GetSprintKey(IMonitor monitor)
        {
            if (Enum.TryParse(this.SprintKey, out Keys key))
            {
                monitor.Log($"Bound key '{key}' for sprinting.");
                return key;
            }

            monitor.Log($"Failed to find specified key '{this.SprintKey}', using default 'LeftShift' for sprinting.", LogLevel.Warn);
            return Keys.LeftShift;
        }
    }
}
