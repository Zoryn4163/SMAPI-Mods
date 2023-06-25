using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Zoryn.Common;

namespace DurableFences
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            CommonHelper.RemoveObsoleteFiles(this, "DurableFences.pdb");

            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised once per second after the game state is updated.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
            foreach (GameLocation location in Game1.locations)
            {
                foreach (Object obj in location.Objects.Values)
                {
                    if (obj is Fence fence)
                        fence.health.Value = fence.maxHealth.Value;
                }
            }
        }
    }
}
