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
        public override void Entry(params object[] objects)
        {
            GameEvents.QuarterSecondTick += GameEvents_QuarterSecondTick;

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
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
