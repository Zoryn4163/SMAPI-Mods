using System;
using StardewModdingAPI;
using StardewValley;

namespace DurableFences
{
    public class DurableFences : Mod
    {
        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.GameEvents.OneSecondTick += GameEvents_OneSecondTick;

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
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
