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

        private float[] RandomFloats;

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
            this.RandomFloats = new float[256];
            this.RandomFloats.FillFloats(); // fill buffer with junk so everything is good and random
            this.Weather = new[]
            {
                WeightedGeneric<int>.Create(this.Config.SunnyChance, Game1.weather_sunny),
                WeightedGeneric<int>.Create(this.Config.CloudySnowyChance, Game1.weather_debris),
                WeightedGeneric<int>.Create(this.Config.RainyChance, Game1.weather_rain),
                WeightedGeneric<int>.Create(this.Config.StormyChance, Game1.weather_lightning),
                WeightedGeneric<int>.Create(this.Config.HarshSnowyChance, Game1.weather_snow)
            };
            this.DetermineRng();

            /*
            //Debugging for my randoms
            while (true)
            {
                List<int> got = new List<int>();
                int v = 0;
                int gen = 1024;
                float genf = gen;
                for (int i = 0; i < gen; i++)
                {
                    v = Weighted.ChooseRange(OneToTen, 1, 5).TValue;
                    got.Add(v);

                    if (gen <= 1024)
                        Console.Write(v + ", ");
                }
                Console.Write("\n");
                Console.WriteLine("Generated {0} Randoms", got.Count);
                Console.WriteLine("1: {0} | 2: {1} | 3: {2} | 4: {3} | 5: {4} | 6: {5} | 7: {6} | 8: {7} | 9: {8} | 10: {9} | ?: {10}", got.Count(x => x == 1), got.Count(x => x == 2), got.Count(x => x == 3), got.Count(x => x == 4), got.Count(x => x == 5), got.Count(x => x == 6), got.Count(x => x == 7), got.Count(x => x == 8), got.Count(x => x == 9), got.Count(x => x == 10), got.Count(x => x > 10));
                Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9}", got.Count(x => x == 1) / genf * 100 + "%", got.Count(x => x == 2) / genf * 100 + "%", got.Count(x => x == 3) / genf * 100 + "%", got.Count(x => x == 4) / genf * 100 + "%", got.Count(x => x == 5) / genf * 100 + "%", got.Count(x => x == 6) / genf * 100 + "%", got.Count(x => x == 7) / genf * 100 + "%", got.Count(x => x == 8) / genf * 100 + "%", got.Count(x => x == 9) / genf * 100 + "%", got.Count(x => x == 10) / genf * 100 + "%");

                //Console.WriteLine(OneToTen.ChooseRange(0, 10).TValue);

                Console.ReadKey();
            }
            */

            // hook events
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player loads a save slot and the world is initialised.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
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

        private void DetermineRng()
        {
            //0 = SUNNY, 1 = RAIN, 2 = CLOUDY/SNOWY, 3 = THUNDER STORM, 4 = FESTIVAL/EVENT/SUNNY, 5 = SNOW
            //Generate a good set of new random numbers to choose from for daily luck every morning.
            this.RandomFloats.FillFloats();

            if (this.Config.EnableDailyLuckOverride)
                Game1.dailyLuck = RandomFloats.Random() / 10;

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
