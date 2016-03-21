using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Log.Info("Junimo Deposit Anywhere by Zoryn => Initialized");
        }

        private void GameEvents_QuarterSecondTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame || Game1.activeClickableMenu == null)
                return;

            if (Game1.activeClickableMenu is JunimoNoteMenu)
            {
                JunimoNoteMenu v = (JunimoNoteMenu) Game1.activeClickableMenu;

                List<Bundle> bndl = new List<Bundle>(v.GetBaseFieldValue("bundles") as List<Bundle>);

                foreach (Bundle b in bndl)
                {
                    if (!b.depositsAllowed)
                        b.depositsAllowed = true;
                }
            }
        }
    }
}
