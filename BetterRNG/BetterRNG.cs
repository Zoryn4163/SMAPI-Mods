using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<WeightedGeneric<Int32>> Weather { get; private set; }

        public static List<WeightedGeneric<Int32>> OneToTen { get; private set; }

        public override void Entry(params object[] objects)
        {
            ModConfig = new RngConfig().InitializeConfig(BaseConfigPath);

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

            Log.Info(GetType().Name + " by Zoryn => Initialized (Press F5 To Reload Config)");
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

        private void PlayerEvents_FarmerChanged(object sender, StardewModdingAPI.Events.EventArgsFarmerChanged e)
        {
            DetermineRng();
        }

        public static void DetermineRng()
        {
            //0 = SUNNY, 1 = RAIN, 2 = CLOUDY/SNOWY, 3 = THUNDER STORM, 4 = FESTIVAL/EVENT/SUNNY, 5 = SNOW
            //Generate a good set of new random numbers to choose from for daily luck every morning.
            RandomFloats.FillFloats();

            if(ModConfig.EnableDailyLuckOverride)
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
                ModConfig = ModConfig.ReloadConfig();
                Log.Success("Config Reloaded for " + GetType().Name);
            }
        }
    }

    public class RngConfig : Config
    {
        public bool EnableDailyLuckOverride { get; set; }
        public bool EnableWeatherOverride { get; set; }
        public List<string[]> Info { get; set; }
        public int SunnyChance { get; set; }
        public int CloudySnowyChance { get; set; }
        public int RainyChance { get; set; }
        public int StormyChance { get; set; }
        public int HarshSnowyChance { get; set; }

        public bool EnableFishingTreasureOverride { get; set; }
        public float FishingTreasureChance { get; set; }
        public bool EnableFishingStuffOverride { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            EnableDailyLuckOverride = false;
            EnableWeatherOverride = false;
            Info = new List<string[]> { "The weather chances are whole numbers as percentages.|They can add up to be any number. 60 = 60% or 0.60, but you must type 60|The fishing things are not done yet and literally do nothing. Do not bother changing their values.".Split('|') };
            SunnyChance = 60;
            CloudySnowyChance = 15;
            RainyChance = 15;
            StormyChance = 5;
            HarshSnowyChance = 5;

            EnableFishingTreasureOverride = false;
            FishingTreasureChance = 1 / 16f;
            EnableFishingStuffOverride = false;
            return this as T;
        }
    }







    public static class Extensions
    {
        public static float[] DynamicDowncast(this Byte[] bytes)
        {
            float[] f = new float[bytes.Length / 4];
            for (int i = 0; i < f.Length; i++)
            {
                f[i] = BitConverter.ToSingle(bytes, i == 0 ? 0 : (i * 4) - 1);
            }
            return f;
        }

        public static void FillFloats(this float[] floats)
        {
            for (int i = 0; i < floats.Length; i++)
                floats[i] = BetterRng.Twister.Next(-100, 100) / 100f;
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.Count == 0 ? default(T) : list[BetterRng.Twister.Next(0, list.Count)];
        }

        public static float Abs(this float f)
        {
            return Math.Abs(f);
        }

        public static T Choose<T>(this List<T> list) where T : IWeighted
        {
            return Weighted.Choose(list);
        }

        public static T ChooseRange<T>(this List<T> list, Int32 min, Int32 max) where T : IWeighted
        {
            return Weighted.ChooseRange(list, min, max);
        }

        public static List<T> TempZero<T>(this List<T> list, int[] whichToZero) where T : class, IWeighted
        {
            T[] ls = new T[list.Count];
            list.CopyTo(ls);

            for (int index = 0; index < ls.Length; index++)
            {
                if (!whichToZero.Contains(index))
                    continue;

                T t = ls.ElementAtOrDefault(index);
                if (t == null)
                    continue;

                t.Weight = 0;
            }

            return ls.ToList();
        }

        public static List<T> TempMult<T>(this List<T> list, float mult, int beginAt = 0) where T : class, IWeighted
        {
            T[] ls = new T[list.Count];
            list.CopyTo(ls);

            for (int index = beginAt; index < ls.Length; index++)
            {
                T t = ls.ElementAtOrDefault(index);
                if (t == null)
                    continue;

                if (t.Weight == 0)
                    continue;

                t.Weight = mult > 0.5f ? (int)Math.Round(t.Weight * mult) : t.Weight;
                mult *= 0.8f;
            }

            return ls.ToList();
        }

        public static int[] FishingZeroes(this Farmer player)
        {
            if (player == null)
                player = BetterRng.Player;

            return Enumerable.Range(0, player.FishingLevel - 3).ToArray();
        }
    }

    public static class Weighted
    {
        public static T Choose<T>(List<T> list) where T : IWeighted
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            int totalweight = list.Sum(c => c.Weight);
            int choice = BetterRng.Twister.Next(totalweight);
            int sum = 0;

            foreach (var obj in list)
            {
                for (float i = sum; i < obj.Weight + sum; i++)
                {
                    if (i >= choice)
                    {
                        return obj;
                    }
                }
                sum += obj.Weight;
            }

            return list.First();
        }

        public static T ChooseRange<T>(List<T> list, Int32 min, Int32 max) where T : IWeighted
        {
            //Grab a random value right quick, make sure the list is working and all
            T choice = Choose(list);

            //4096 changes to pick a number from the range.
            //If it takes longer than that something isn't right.
            for (int i = 0; i < 4096; i++)
            {
                choice = Choose(list);
                if (choice.Value is Int32)
                {
                    Int32 x = (Int32)choice.Value;
                    if (x >= min && x <= max)
                        return choice;
                }
            }

            if (choice is WeightedGeneric<int>)
            {
                int w = choice.Weight;
                choice = Activator.CreateInstance<T>();
                choice.Value = min < max ? BetterRng.Twister.Next(min, max) : BetterRng.Twister.Next(Math.Max(min, max));
                choice.Weight = w;
                return choice;
            }

            throw new ArgumentOutOfRangeException(nameof(min) + nameof(max), "The requested value was not in the Min/Max range supplied.");
        }
    }

    public interface IWeighted
    {
        int Weight { get; set; }
        object Value { get; set; }
    }

    public class WeightedGeneric<T> : IWeighted
    {
        public object Value { get; set; }
        public int Weight { get; set; }

        public T TValue => (T)Value;

        public WeightedGeneric()
        {
            //Nothing
        }

        public WeightedGeneric(int weight, T value)
        {
            Weight = weight;
            Value = value;
        }

        public static WeightedGeneric<Ty> Create<Ty>(int weight, Ty value)
        {
            return new WeightedGeneric<Ty>(weight, value);
        }
    }
}