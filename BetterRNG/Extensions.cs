using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using SFarmer = StardewValley.Farmer;

namespace BetterRNG
{
    internal static class Extensions
    {
        public static float[] DynamicDowncast(this byte[] bytes)
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

        public static T Choose<T>(this T[] list) where T : IWeighted
        {
            return Weighted.Choose(list);
        }

        public static T ChooseRange<T>(this T[] list, int min, int max) where T : IWeighted
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

        public static int[] FishingZeroes(this SFarmer player)
        {
            return Enumerable.Range(0, (player ?? Game1.player).FishingLevel - 3).ToArray();
        }
    }
}
