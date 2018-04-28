using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;

namespace MovementMod.Framework
{
    internal class ModConfig
    {
        /*********
        ** Accessors
        *********/
        public int PlayerRunningSpeed { get; set; } = 5;

        public int HorseSpeed { get; set; } = 5;

        public int PlayerSprintingSpeedMultiplier { get; set; } = 2;
        public string SprintKey { get; set; } = "LeftShift";
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
