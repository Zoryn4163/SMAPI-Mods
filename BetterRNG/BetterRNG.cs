using System;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace BetterRNG
{
    /// <summary>The main entry point.</summary>
    public class BetterRng : Mod
    {
        /*********
        ** Properties
        *********/
        private float[] RandomFloats;
        private RngConfig Config;
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
            this.Config = helper.ReadConfig<RngConfig>();

            this.RandomFloats = new float[256];
            BetterRng.Twister = new MersenneTwister();

            //Destroys the game's built-in random number generator for Twister.
            Game1.random = BetterRng.Twister;

            //Just fills the buffer with junk so that we know everything is good and random.
            this.RandomFloats.FillFloats();

            //Define base randoms
            this.Weather = new[]
            {
                WeightedGeneric<int>.Create(this.Config.SunnyChance, 0),
                WeightedGeneric<int>.Create(this.Config.CloudySnowyChance, 2),
                WeightedGeneric<int>.Create(this.Config.RainyChance, 1),
                WeightedGeneric<int>.Create(this.Config.StormyChance, 3),
                WeightedGeneric<int>.Create(this.Config.HarshSnowyChance, 5)
            };

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

            //Determine base RNG to get everything up and running.
            this.DetermineRng();

            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }


        /*********
        ** Private methods
        *********/
        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            this.DetermineRng();
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                this.Config = this.Helper.ReadConfig<RngConfig>();
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
                if (targetWeather == 5 && Game1.currentSeason != "winter")
                    targetWeather = 3;
                if (targetWeather == 1 && Game1.currentSeason == "winter")
                    targetWeather = 2;
                if (targetWeather == 3 && Game1.currentSeason == "winter")
                    targetWeather = 5;
                if (targetWeather == 4)
                    targetWeather = 0;

                Game1.weatherForTomorrow = targetWeather;
            }
        }
    }
}
