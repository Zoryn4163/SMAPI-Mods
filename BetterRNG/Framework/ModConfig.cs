using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace BetterRNG.Framework;

/// <summary>The mod configuration.</summary>
internal class ModConfig
{
    /// <summary>Whether to randomise your daily luck.</summary>
    public bool EnableDailyLuckOverride { get; set; }

    /// <summary>Whether to randomise tomorrow's weather.</summary>
    public bool EnableWeatherOverride { get; set; }

    /// <summary>The weight for sunny weather when randomising weather.</summary>
    public int SunnyChance { get; set; } = 60;

    /// <summary>The weight for debris weather (e.g. blowing leaves, wind, etc) when randomising weather.</summary>
    public int CloudySnowyChance { get; set; } = 15;

    /// <summary>The weight for rain when randomising weather.</summary>
    public int RainyChance { get; set; } = 15;

    /// <summary>The weight for storms when randomising weather.</summary>
    public int StormyChance { get; set; } = 5;

    /// <summary>The weight for snow when randomising weather.</summary>
    public int HarshSnowyChance { get; set; } = 5;

    /// <summary>The keys which reload the mod config.</summary>
    public KeybindList ReloadKey { get; set; } = new(SButton.F5);
}
