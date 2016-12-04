using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace HealthBars
{
    public class HealthBarConfig : Config
    {
        public bool DisplayHealthWhenNotDamaged { get; set; }

        public bool DisplayMaxHealthNumber { get; set; }
        public bool DisplayCurrentHealthNumber { get; set; }

        public bool DisplayTextBorder { get; set; }

        public Color TextColor { get; set; }
        public Color TextBorderColor { get; set; }

        public Color LowHealthColor { get; set; }
        public Color HighHealthColor { get; set; }

        public int BarWidth { get; set; }
        public int BarHeight { get; set; }

        public int BarBorderWidth { get; set; }
        public int BarBorderHeight { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            DisplayHealthWhenNotDamaged = false;

            DisplayMaxHealthNumber = true;
            DisplayCurrentHealthNumber = true;

            DisplayTextBorder = true;

            TextColor = Color.White;
            TextBorderColor = Color.Black;

            LowHealthColor = Color.DarkRed;
            HighHealthColor = Color.LimeGreen;

            BarWidth = 90;
            BarHeight = 15;

            BarBorderWidth = 2;
            BarBorderHeight = 2;
            return this as T;
        }
    }
}