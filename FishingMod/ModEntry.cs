using System;
using FishingMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using Zoryn.Common;

namespace FishingMod;

/// <summary>The main entry point.</summary>
public class ModEntry : Mod
{
    /*********
    ** Properties
    *********/
    /// <summary>The mod configuration.</summary>
    private ModConfig Config;

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
        CommonHelper.RemoveObsoleteFiles(this, "FishingMod.pdb");

        this.Config = helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
    }


    /*********
    ** Private methods
    *********/
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
        if (Game1.activeClickableMenu is BobberBar bobber)
        {
            //Begin fishing game
            if (!this.BeganFishingGame.Value && this.UpdateIndex.Value > 15)
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

                this.BeganFishingGame.Value = true;
            }

            if (this.UpdateIndex.Value < 20)
                this.UpdateIndex.Value++;

            if (this.Config.AlwaysPerfect)
                bobber.perfect = true;

            if (!bobber.bobberInBar)
                bobber.distanceFromCatching += this.Config.LossAdditive;
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
