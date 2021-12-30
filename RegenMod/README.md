﻿**Regen Mod** is a [Stardew Valley](http://stardewvalley.net/) mod which lets you regenerate
health and stamina passively over time (configurable).

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
The mod will automatically regenerate your health and stamina passively over time. You can edit the
`config.json` file to customise the regeneration, including...

* Whether stamina regenerates (including how fast and when);
* Whether stamina _drains_ over time instead;
* Whether health regenerates (including how fast and when);
* Whether health _drains_ over time instead.

## Configure
A `config.json` will appear in the mod's folder after you run the game once. You can optionally
open the file in a text editor to configure the mod. If you make a mistake, just delete the file
and it'll be regenerated.

Available fields:

field                            | purpose
-------------------------------- | -------
`RegenStaminaConstant`           | Whether to constantly regenerate stamina. Default false.
`RegenStaminaConstantAmountPerSecond` | The amount of stamina to constantly regenerate per second. Default 0.
`RegenStaminaStill`              | Whether to regenerate stamina while standing still. Default false.
`RegenStaminaStillAmountPerSecond` | The amount of stamina to regenerate per second while standing still. Default false.
`RegenStaminaStillTimeRequiredMS`  | The amount of time the player must stand still to regenerate stamina, in milliseconds. Default 1000.
`RegenHealthConstant`            | Whether to constantly regenerate health. Default false.
`RegenHealthConstantAmountPerSecond` | The amount of stamina to constantly regenerate per second. Default 0.
`RegenHealthStill`               | Whether to regenerate health while standing still. Default false.
`RegenHealthStillAmountPerSecond` | The amount of health to regenerate per second while standing still. Default 0.
`RegenHealthStillTimeRequiredMS`  | The amount of time the player must stand still to regenerate health. Default 1000.

## Compatibility
* Works with Stardew Valley 1.5.5+ on Linux/macOS/Windows.
* Works in single-player, multiplayer, and split-screen.
* No known mod conflicts.

## See also
* [Release notes](release-notes.md)
