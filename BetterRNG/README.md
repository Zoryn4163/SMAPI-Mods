﻿**Better RNG** is a [Stardew Valley](http://stardewvalley.net/) mod which makes the game's
randomness more random.

Compatible with Stardew Valley 1.1+ on Linux, Mac, and Windows.

## Contents
* [Install](#install)
* [Use](#use)
* [Versions](#versions)

## Install
1. [Install the latest version of SMAPI](https://github.com/Pathoschild/SMAPI/releases).
3. Install this mod from the releases page.
4. Run the game using SMAPI.

## Use
You can edit the `config.json` file to change the mod's settings.

The mod will...
* **Completely** redefine the game's random number generator (RNG) to use a Mersenne Twister
  Generator for random happenings. This can't be turned off. If you understood how the game handles
  RNG, you would understand why. Everything else in the mod can be configured as desired.
* Randomise daily luck every morning using the new RNG.
* Randomise **tomorrow's** weather every morning based on the configured chance values (including
  the probability of sun, clouds or light snow, rain, lightning storm, or blizzard).

  The weather chance values don't need to total 100%. Chances for each are calculated relative to
  their other values.

  Note that some days of the game have hardcoded weather, so the weather on those days can't be
  changed. That means the weather channel may be wrong in rare cases.

## Versions
### 1.6
* Updated to SMAPI 1.9.

### 1.5
* Updated to SMAPI 1.3.

### 1.4
* Updated to SMAPI 1.0.
* Added support for Linux/Mac.

### 1.3
* Updated to SMAPI 0.39.6.

### 1.2
* Updated to SMAPI 0.39.3.
* Added re-randomisation when a save is loaded.
* Added hotkey to reload `config.json`.

### 1.1
* Updated to SMAPI 0.39.1.

### 1.0
* Initial release. Compatible with SMAPI 0.38.4.
