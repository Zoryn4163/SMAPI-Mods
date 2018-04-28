using System;
using FishingMod.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace FishingMod
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private SBobberBar Bobber;
        private bool BeganFishingGame;
        private int UpdateIndex;
        private ModConfig Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            GameEvents.UpdateTick += this.GameEvents_OnUpdateTick;
            GameEvents.OneSecondTick += this.GameEvents_OneSecondTick;
            MenuEvents.MenuChanged += this.MenuEvents_MenuChanged;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        private void MenuEvents_MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {
            if (e.NewMenu is BobberBar menu)
                this.Bobber = SBobberBar.ConstructFromBaseClass(menu);
        }

        private void GameEvents_OneSecondTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
                return;

            if (this.Config.InfiniteBait || this.Config.InfiniteTackle)
            {
                if (Game1.player.CurrentTool is FishingRod rod && rod.attachments?.Length > 0 && rod.attachments[0] != null)
                {
                    if (this.Config.InfiniteBait)
                        rod.attachments[0].Stack = rod.attachments[0].maximumStackSize();

                    if (rod.attachments?.Length > 1 && rod.attachments[1] != null)
                    {
                        if (this.Config.InfiniteTackle)
                        {
                            rod.attachments[1].Stack = rod.attachments[1].maximumStackSize();
                            rod.attachments[1].Scale = new Vector2(rod.attachments[1].Scale.X, 1.1f);
                        }
                    }
                }
            }
        }

        private void GameEvents_OnUpdateTick(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is BobberBar && this.Bobber != null)
            {
                SBobberBar bobber = this.Bobber;

                //Begin fishing game
                if (!this.BeganFishingGame && this.UpdateIndex > 15)
                {
                    //Do these things once per fishing minigame, 1/4 second after it updates
                    bobber.difficulty *= this.Config.FishDifficultyMultiplier;
                    bobber.difficulty += this.Config.FishDifficultyAdditive;

                    if (this.Config.AlwaysFindTreasure)
                        bobber.treasure = true;

                    if (this.Config.InstantCatchFish)
                    {
                        if (bobber.treasure)
                            bobber.treasureCaught = true;
                        bobber.distanceFromCatching += 100;
                    }

                    if (this.Config.InstantCatchTreasure)
                        if (bobber.treasure || this.Config.AlwaysFindTreasure)
                            bobber.treasureCaught = true;

                    if (this.Config.EasierFishing)
                    {
                        bobber.difficulty = Math.Max(15, Math.Max(bobber.difficulty, 60));
                        bobber.motionType = 2;
                    }

                    this.BeganFishingGame = true;
                }

                if (this.UpdateIndex < 20)
                    this.UpdateIndex++;

                if (this.Config.AlwaysPerfect)
                    bobber.perfect = true;

                if (!bobber.bobberInBar)
                    bobber.distanceFromCatching += this.Config.LossAdditive;
            }
            else
            {
                //End fishing game
                this.BeganFishingGame = false;
                this.UpdateIndex = 0;
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
