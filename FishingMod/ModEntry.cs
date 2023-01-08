using System;
using FishingMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
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

        /// <summary>The current fishing bobber bar.</summary>
        private readonly PerScreen<SBobberBar> Bobber = new();

        /// <summary>Whether the player is in the fishing minigame.</summary>
        private readonly PerScreen<bool> BeganFishingGame = new();

        /// <summary>The number of ticks since the player opened the fishing minigame.</summary>
        private readonly PerScreen<int> UpdateIndex = new();


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
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            this.Bobber.Value = e.NewMenu is BobberBar menu
                ? new SBobberBar(menu, this.Helper.Reflection)
                : null;
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // apply infinite bait/tackle
            if (e.IsOneSecond && (this.Config.InfiniteBait || this.Config.InfiniteTackle))
            {
                if (Game1.player.CurrentTool is FishingRod rod)
                {
                    if (this.Config.InfiniteBait && rod.attachments?.Length > 0 && rod.attachments[0] != null)
                        rod.attachments[0].Stack = rod.attachments[0].maximumStackSize();

                    if (this.Config.InfiniteTackle && rod.attachments?.Length > 1 && rod.attachments[1] != null)
                        rod.attachments[1].uses.Value = 0;
                }
            }

            // apply fishing minigame changes
            if (Game1.activeClickableMenu is BobberBar && this.Bobber.Value != null)
            {
                SBobberBar bobber = this.Bobber.Value;

                //Begin fishing game
                if (!this.BeganFishingGame.Value && this.UpdateIndex.Value > 15)
                {
                    //Do these things once per fishing minigame, 1/4 second after it updates
                    bobber.Difficulty *= this.Config.FishDifficultyMultiplier;
                    bobber.Difficulty += this.Config.FishDifficultyAdditive;

                    if (this.Config.AlwaysFindTreasure)
                        bobber.Treasure = true;

                    if (this.Config.InstantCatchFish)
                    {
                        if (bobber.Treasure)
                            bobber.TreasureCaught = true;
                        bobber.DistanceFromCatching += 100;
                    }

                    if (this.Config.InstantCatchTreasure)
                        if (bobber.Treasure || this.Config.AlwaysFindTreasure)
                            bobber.TreasureCaught = true;

                    if (this.Config.EasierFishing)
                    {
                        bobber.Difficulty = Math.Max(15, Math.Max(bobber.Difficulty, 60));
                        bobber.MotionType = 2;
                    }

                    this.BeganFishingGame.Value = true;
                }

                if (this.UpdateIndex.Value < 20)
                    this.UpdateIndex.Value++;

                if (this.Config.AlwaysPerfect)
                    bobber.Perfect = true;

                if (!bobber.BobberInBar)
                    bobber.DistanceFromCatching += this.Config.LossAdditive;
            }
            else
            {
                //End fishing game
                this.BeganFishingGame.Value = false;
                this.UpdateIndex.Value = 0;
            }
        }

        /// <inheritdoc cref="IInputEvents.ButtonsChanged"/>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (this.Config.ReloadKey.JustPressed())
            {
                this.Config = this.Helper.ReadConfig<ModConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}
