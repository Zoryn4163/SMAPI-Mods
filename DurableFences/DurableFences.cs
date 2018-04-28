using System;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace DurableFences
{
    /// <summary>The main entry point.</summary>
    public class DurableFences : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;
        }


        /*********
        ** Private methods
        *********/
        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (Fence fence in location.Objects.Values.OfType<Fence>())
                    fence.health.Value = fence.maxHealth.Value;
            }
        }
    }
}
