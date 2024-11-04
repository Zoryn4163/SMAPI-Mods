using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace HealthBars.Framework;

/// <summary>The mod configuration.</summary>
internal class ModConfig
{
    /// <summary>Whether to show a health bar for monsters at full health.</summary>
    public bool DisplayHealthWhenNotDamaged { get; set; }

    /// <summary>Whether to show the maximum health number.</summary>
    public bool DisplayMaxHealthNumber { get; set; } = true;

    /// <summary>Whether to show the current health number.</summary>
    public bool DisplayCurrentHealthNumber { get; set; } = true;

    /// <summary>Whether to draw a border around text so it's more visible on some backgrounds.</summary>
    public bool DisplayTextBorder { get; set; } = true;

    /// <summary>The text color.</summary>
    public Color TextColor { get; set; } = Color.White;

    /// <summary>The text border color.</summary>
    public Color TextBorderColor { get; set; } = Color.Black;

    /// <summary>The health bar color when the monster has low health.</summary>
    public Color LowHealthColor { get; set; } = Color.DarkRed;

    /// <summary>The health bar color when the monster has high health.</summary>
    public Color HighHealthColor { get; set; } = Color.LimeGreen;

    /// <summary>The health bar width in pixels.</summary>
    public int BarWidth { get; set; } = 90;

    /// <summary>The health bar height in pixels.</summary>
    public int BarHeight { get; set; } = 15;

    /// <summary>The health bar's vertical border width in pixels.</summary>
    public int BarBorderWidth { get; set; } = 2;

    /// <summary>The health bar's horizontal border width in pixels.</summary>
    public int BarBorderHeight { get; set; } = 2;

    /// <summary>The keys which reload the mod config.</summary>
    public KeybindList ReloadKey { get; set; } = new(SButton.F5);
}
