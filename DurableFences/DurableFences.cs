using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;

namespace DurableFences
{
    public class DurableFences : Mod
    {
        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.GameEvents.OneSecondTick += GameEvents_OneSecondTick;

            Log.Verbose("DurableFences by Zoryn => Initialized");
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            foreach (GameLocation gl in Game1.locations)
            {
                foreach (var v in gl.Objects)
                {
                    if (v.Value is Fence && v.Value.Cast<Fence>() != null)
                    {
                        v.Value.Cast<Fence>().health = v.Value.Cast<Fence>().maxHealth;
                    }
                }
            }
        }
    }
}
