using Microsoft.Xna.Framework;

namespace HealthBars.Framework
{
    internal class ModConfig
    {
        public bool DisplayHealthWhenNotDamaged { get; set; }

        public bool DisplayMaxHealthNumber { get; set; } = true;
        public bool DisplayCurrentHealthNumber { get; set; } = true;

        public bool DisplayTextBorder { get; set; } = true;

        public Color TextColor { get; set; } = Color.White;
        public Color TextBorderColor { get; set; } = Color.Black;

        public Color LowHealthColor { get; set; } = Color.DarkRed;
        public Color HighHealthColor { get; set; } = Color.LimeGreen;

        public int BarWidth { get; set; } = 90;
        public int BarHeight { get; set; } = 15;

        public int BarBorderWidth { get; set; } = 2;
        public int BarBorderHeight { get; set; } = 2;
    }
}
