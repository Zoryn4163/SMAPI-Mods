﻿﻿**Better RNG** is a [Stardew Valley](http://stardewvalley.net/) mod which makes the game's
randomness more random.

## Contents
* [Install](#install)
* [Use](#use)
* [Configure](#configure)
* [Compatibility](#compatibility)
* [See also](#see-also)

## Install
1. [Install the latest version of SMAPI](https://smapi.io).
2. Install this mod from the releases page.
3. Run the game using SMAPI.

## Use
You can edit the `config.json` file to change the mod's settings.

The mod will...
* **Completely** redefine the game's random number generator (RNG) to use a Mersenne Twister
  Generator for random happenings. This can't be turned off. If you understood how the game handles
  RNG, you would understand why. Everything else in the mod can be configured as desired.
* Randomise daily luck every morning using the new RNG.
* Randomise **tomorrow's** weather every morning based on the configured chance values (including
  the probability of sun, clouds or light snow, rain, lightning storm, or blizzard).

  Note that some days of the game have hardcoded weather, so the weather on those days can't be
  changed. That means the weather channel may be wrong in rare cases.

## Configure
A `config.json` will appear in the mod's folder after you run the game once. You can optionally
open the file in a text editor to configure the mod. If you make a mistake, just delete the file
and it'll be regenerated.

For the weather fields, you specify their weight relative to the other weathers. For example, two
weathers set to `1` have an equal chance of being chosen. The actual values you choose only matter
as a proportion to the sum; the defaults sum to 100, but that's not required.

Available fields:

field                     | purpose
------------------------- | -------
`EnableDailyLuckOverride` | Whether to randomise your daily luck. Default false.
`EnableWeatherOverride`   | Whether to randomise tomorrow's weather. Default false.
`SunnyChance`             | The weight for sunny weather when randomising weather. Default 60.
`CloudySnowyChance`       | The weight for debris weather (e.g. blowing leaves, wind, etc) when randomising weather. Default 15.
`RainyChance`             | The weight for rain when randomising weather. Default 15.
`StormyChance`            | The weight for storms when randomising weather. Default 5.
`HarshSnowyChance`        | The weight for snow when randomising weather. Default 5.

## Compatibility
* Works with Stardew Valley 1.6+ on Linux/macOS/Windows.
* Works in single-player, multiplayer, and split-screen. Some things may use the main player's RNG.
* No known mod conflicts.

## See also
* [Release notes](release-notes.md)
