using System;
using FishingMod.Framework;
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
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        private SBobberBar Bobber;
        private bool BeganFishingGame;
        private int UpdateIndex;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is BobberBar menu)
                this.Bobber = SBobberBar.ConstructFromBaseClass(menu);
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // apply infinite bait/tackle
            if (Context.IsWorldReady && e.IsOneSecond && (this.Config.InfiniteBait || this.Config.InfiniteTackle))
            {
                if (Game1.player.CurrentTool is FishingRod rod && rod.attachments?.Length > 0 && rod.attachments[0] != null)
                {
                    if (this.Config.InfiniteBait)
                        rod.attachments[0].Stack = rod.attachments[0].maximumStackSize();

                    if (this.Config.InfiniteTackle && rod.attachments?.Length > 1 && rod.attachments[1] != null)
                        rod.attachments[1].uses.Value = 0;
                }
            }

            // apply fishing minigame changes
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

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F5)
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
