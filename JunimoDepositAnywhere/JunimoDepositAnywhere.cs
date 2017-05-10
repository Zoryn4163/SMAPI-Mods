using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace JunimoDepositAnywhere
{
    /// <summary>The main entry point.</summary>
    public class JunimoDepositAnywhere : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.QuarterSecondTick += this.GameEvents_QuarterSecondTick;
        }


        /*********
        ** Protected methods methods
        *********/
        /// <summary>A method invoked roughly four times per second.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void GameEvents_QuarterSecondTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.activeClickableMenu == null)
                return;

            if (Game1.activeClickableMenu is JunimoNoteMenu menu)
            {
                foreach (Bundle bundle in menu.bundles)
                    bundle.depositsAllowed = true;
            }
        }
    }
}
