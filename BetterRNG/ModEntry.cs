using System;
using BetterRNG.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Zoryn.Common;

namespace BetterRNG;

/// <summary>The main entry point.</summary>
public class ModEntry : Mod
{
    /*********
    ** Properties
    *********/
    /// <summary>The mod configuration.</summary>
    private ModConfig Config;

    private WeightedGeneric<string>[] Weather;


    /*********
    ** Accessors
    *********/
    internal static MersenneTwister Twister { get; private set; }


    /*********
    ** Public methods
    *********/
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        CommonHelper.RemoveObsoleteFiles(this, "BetterRNG.pdb");

        // read config
        this.Config = helper.ReadConfig<ModConfig>();

        // init randomness
        Game1.random = ModEntry.Twister = new MersenneTwister();
        this.Weather = new[]
        {
            WeightedGeneric<string>.Create(this.Config.SunnyChance, Game1.weather_sunny),
            WeightedGeneric<string>.Create(this.Config.CloudySnowyChance, Game1.weather_debris),
            WeightedGeneric<string>.Create(this.Config.RainyChance, Game1.weather_rain),
            WeightedGeneric<string>.Create(this.Config.StormyChance, Game1.weather_lightning),
            WeightedGeneric<string>.Create(this.Config.HarshSnowyChance, Game1.weather_snow)
        };

        // hook events
        helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
    }


    /*********
    ** Private methods
    *********/
    /// <summary>Raised after the player starts a new day.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnDayStarted(object sender, DayStartedEventArgs e)
    {
        this.DetermineRng();
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

    /// <summary>Randomise the daily luck and weather.</summary>
    private void DetermineRng()
    {
        if (this.Config.EnableDailyLuckOverride)
            Game1.player.team.sharedDailyLuck.Value = Math.Min(0.100000001490116, ModEntry.Twister.Next(-100, 101) / 1000.0);

        if (Context.IsMainPlayer && this.Config.EnableWeatherOverride)
        {
            string targetWeather = this.Weather.Choose().TValue;
            if (targetWeather == Game1.weather_snow && Game1.season != Season.Winter)
                targetWeather = Game1.weather_lightning;
            if (targetWeather == Game1.weather_rain && Game1.season == Season.Winter)
                targetWeather = Game1.weather_debris;
            if (targetWeather == Game1.weather_lightning && Game1.season == Season.Winter)
                targetWeather = Game1.weather_snow;
            if (targetWeather == Game1.weather_festival)
                targetWeather = Game1.weather_sunny;

            Game1.weatherForTomorrow = targetWeather;
        }
    }
}
