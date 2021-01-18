using System;
using BetterRNG.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace BetterRNG
{
    /// <summary>The main entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        private WeightedGeneric<int>[] Weather;


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
            // read config
            this.Config = helper.ReadConfig<ModConfig>();

            // init randomness
            Game1.random = ModEntry.Twister = new MersenneTwister();
            this.Weather = new[]
            {
                WeightedGeneric<int>.Create(this.Config.SunnyChance, Game1.weather_sunny),
                WeightedGeneric<int>.Create(this.Config.CloudySnowyChance, Game1.weather_debris),
                WeightedGeneric<int>.Create(this.Config.RainyChance, Game1.weather_rain),
                WeightedGeneric<int>.Create(this.Config.StormyChance, Game1.weather_lightning),
                WeightedGeneric<int>.Create(this.Config.HarshSnowyChance, Game1.weather_snow)
            };

            // hook events
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.DetermineRng();
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

        /// <summary>Randomise the daily luck and weather.</summary>
        private void DetermineRng()
        {
            if (this.Config.EnableDailyLuckOverride)
                Game1.player.team.sharedDailyLuck.Value = Math.Min(0.100000001490116, ModEntry.Twister.Next(-100, 101) / 1000.0);

            if (this.Config.EnableWeatherOverride)
            {
                int targetWeather = this.Weather.Choose().TValue;
                if (targetWeather == Game1.weather_snow && Game1.currentSeason != "winter")
                    targetWeather = Game1.weather_lightning;
                if (targetWeather == Game1.weather_rain && Game1.currentSeason == "winter")
                    targetWeather = Game1.weather_debris;
                if (targetWeather == Game1.weather_lightning && Game1.currentSeason == "winter")
                    targetWeather = Game1.weather_snow;
                if (targetWeather == Game1.weather_festival)
                    targetWeather = Game1.weather_sunny;

                Game1.weatherForTomorrow = targetWeather;
            }
        }
    }
}
