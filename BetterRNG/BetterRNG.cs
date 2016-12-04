using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Inheritance;
using StardewValley;

namespace BetterRNG
{
    public class BetterRng : Mod
    {
        public static MersenneTwister Twister { get; private set; }
        public static float[] RandomFloats { get; private set; }

        public static RngConfig ModConfig { get; private set; }

        public static Farmer Player => Game1.player;

        public static List<WeightedGeneric<int>> Weather { get; private set; }

        public static List<WeightedGeneric<int>> OneToTen { get; private set; }

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            ModConfig = helper.ReadConfig<RngConfig>();

            RandomFloats = new float[256];
            Twister = new MersenneTwister();

            //Destroys the game's built-in random number generator for Twister.
            Game1.random = Twister;

            //Just fills the buffer with junk so that we know everything is good and random.
            RandomFloats.FillFloats();

            //Define base randoms
            OneToTen = new List<WeightedGeneric<int>>() { WeightedGeneric<int>.Create(75, 1), WeightedGeneric<int>.Create(50, 2), WeightedGeneric<int>.Create(25, 3), WeightedGeneric<int>.Create(15, 4), WeightedGeneric<int>.Create(11, 5), WeightedGeneric<int>.Create(9, 6), WeightedGeneric<int>.Create(7, 7), WeightedGeneric<int>.Create(5, 8), WeightedGeneric<int>.Create(3, 9), WeightedGeneric<int>.Create(1, 10) };
            Weather = new List<WeightedGeneric<int>>() { WeightedGeneric<int>.Create(ModConfig.SunnyChance, 0), WeightedGeneric<int>.Create(ModConfig.CloudySnowyChance, 2), WeightedGeneric<int>.Create(ModConfig.RainyChance, 1), WeightedGeneric<int>.Create(ModConfig.StormyChance, 3), WeightedGeneric<int>.Create(ModConfig.HarshSnowyChance, 5) };

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
            DetermineRng();

            PlayerEvents.FarmerChanged += PlayerEvents_FarmerChanged;
            GameEvents.UpdateTick += GameEvents_UpdateTick;
            PlayerEvents.LoadedGame += PlayerEvents_LoadedGame;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;

            this.Monitor.Log("Initialized (press F5 to reload config)");
        }

        private void PlayerEvents_LoadedGame(object sender, EventArgsLoadedGameChanged e)
        {
            Task.Run(() =>
            {
                while (Game1.gameMode != 3)
                    Thread.Sleep(100);
                DetermineRng();
            });
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (SGame.Debug)
                SGame.QueueDebugMessage($"[Twister] Daily Luck: {Game1.dailyLuck} | Tomorrow's Weather: {Game1.weatherForTomorrow}");
        }

        private void PlayerEvents_FarmerChanged(object sender, EventArgsFarmerChanged e)
        {
            DetermineRng();
        }

        public static void DetermineRng()
        {
            //0 = SUNNY, 1 = RAIN, 2 = CLOUDY/SNOWY, 3 = THUNDER STORM, 4 = FESTIVAL/EVENT/SUNNY, 5 = SNOW
            //Generate a good set of new random numbers to choose from for daily luck every morning.
            RandomFloats.FillFloats();

            if (ModConfig.EnableDailyLuckOverride)
                Game1.dailyLuck = RandomFloats.Random() / 10;

            if (ModConfig.EnableWeatherOverride)
            {
                int targWeather = BetterRng.Weather.Choose().TValue;
                if (targWeather == 5 && Game1.currentSeason != "winter")
                    targWeather = 3;
                if (targWeather == 1 && Game1.currentSeason == "winter")
                    targWeather = 2;
                if (targWeather == 3 && Game1.currentSeason == "winter")
                    targWeather = 5;
                if (targWeather == 4)
                    targWeather = 0;

                Game1.weatherForTomorrow = targWeather;
            }
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Keys.F5)
            {
                ModConfig = this.Helper.ReadConfig<RngConfig>();
                this.Monitor.Log("Config reloaded", LogLevel.Info);
            }
        }
    }
}