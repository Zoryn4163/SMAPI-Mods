using System;
using System.Linq;
using StardewModdingAPI;
using StardewValley;

namespace DurableFences
{
    public class DurableFences : Mod
    {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            StardewModdingAPI.Events.GameEvents.OneSecondTick += GameEvents_OneSecondTick;

            this.Monitor.Log("Initialized");
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            foreach (GameLocation gl in Game1.locations)
            {
                foreach (Fence fence in gl.Objects.Values.OfType<Fence>())
                    fence.health = fence.maxHealth;
            }
        }
    }
}
