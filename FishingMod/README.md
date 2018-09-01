﻿**Fishing Mod** is a [Stardew Valley](http://stardewvalley.net/) mod which lets you customise the
fishing experience by editing the mod settings.

## Contents
* [Install](#install)
* [Use](#use)
* [Configure](#configure)
* [Compatibility](#compatibility)
* [Versions](#versions)

## Install
1. [Install the latest version of SMAPI](https://smapi.io).
2. Install this mod from the releases page.
3. Run the game using SMAPI.

## Use
The mod lets you optionally...

* always perfect catch;
* always find treasure;
* instant fish catch;
* instant treasure catch;
* change fish difficulty (multiplier or additive);
* change bar drain;
* set infinite tackle use;
* set infinite bait.

## Configure
A `config.json` will appear in the mod's folder after you run the game once. You can optionally
open the file in a text editor to configure the mod. If you make a mistake, just delete the file
and it'll be regenerated.

Available fields:

field                      | purpose
-------------------------- | -------
`AlwaysPerfect`            | Whether the game should consider every catch to be perfectly executed, even if it wasn't. Default false.
`AlwaysFindTreasure`       | Whether to always find treasure. Default false.
`InstantCatchFish`         | Whether to catch fish instantly. Default false.
`InstantCatchTreasure`     | Whether to catch treasure instantly. Defalt false.
`EasierFishing`            | Whether to significantly lower the max fish difficulty.
`FishDifficultyMultiplier` | A multiplier applied to the fish difficulty. This can a number between 0 and 1 to lower difficulty, or more than 1 to increase it.
`FishDifficultyAdditive`   | A value added to the fish difficulty. This can be less than 0 to decrease difficulty, or more than 0 to increase it.
`LossAdditive`             | A value added to the gradual completion drain when the fish isn't inside the green bar. This can be a negative value to increase drain speed, or a positive value to decrease it. A value of 0.003 will prevent drain entirely.
`InfiniteTackle`           | Whether fishing tackles last forever.
`InfiniteBait`             | Whether fishing bait lasts forever.

## Compatibility
* Works with Stardew Valley 1.3 on Linux/Mac/Windows.
* Works in single-player and multiplayer.
* No known mod conflicts.

## Versions
### 2.6.1
* Fixed unlimited tackles no longer being unlimited.

### 2.6
* Updated to latest Stardew Valley 1.3.

### 1.9
* Updated to Stardew Valley 1.3 (including multiplayer).

### 1.8
* Updated to SMAPI 1.15 & 2.0.

### 1.7
* Updated to Stardew Valley 1.2.

### 1.6
* Updated to SMAPI 1.9.

### 1.5
* Updated to SMAPI 1.3.

### 1.4
* Updated to SMAPI 1.0.
* Added support for Linux/Mac.

### 1.3.1
* Fixed the 'always find treasure' option not working.

### 1.3
* Updated to SMAPI 0.39.6.

### 1.2
* Updated to SMAPI 0.39.3.
* Added hotkey to reload `config.json`.

### 1.1
* Updated to SMAPI 0.39.1.

### 1.0
* Initial release. Compatible with SMAPI 0.38.4.
