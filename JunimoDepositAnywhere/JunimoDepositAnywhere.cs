using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace JunimoDepositAnywhere
{
    public class JunimoDepositAnywhere : Mod
    {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            GameEvents.QuarterSecondTick += GameEvents_QuarterSecondTick;

            this.Monitor.Log("Initialized");
        }

        private void GameEvents_QuarterSecondTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.activeClickableMenu == null)
                return;

            if (Game1.activeClickableMenu is JunimoNoteMenu)
            {
                JunimoNoteMenu v = (JunimoNoteMenu) Game1.activeClickableMenu;

                List<Bundle> bndl = new List<Bundle>(v.GetType().GetBaseFieldValue<List<Bundle>>(v, "bundles"));

                foreach (Bundle b in bndl)
                {
                    if (!b.depositsAllowed)
                        b.depositsAllowed = true;
                }
            }
        }
    }
}
