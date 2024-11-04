﻿**Movement Modifier** is a [Stardew Valley](http://stardewvalley.net/) mod which lets you customise
your movement.

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
The mod lets you walk diagonally, sprint quickly (optionally consumes stamina), and customise your
speed when walking, running, or riding the horse by editing the `config.json` file.

## Configure
A `config.json` will appear in the mod's folder after you run the game once. You can optionally
open the file in a text editor to configure the mod. If you make a mistake, just delete the file
and it'll be regenerated.

Available fields:

field                | purpose
-------------------- | -------
`PlayerRunningSpeed` | The player speed to add when running (or 0 for no change). Default 5.
`HorseSpeed`         | The player speed to add when riding the horse (or 0 for no change). Default 5.
`SprintKey`          | The key which causes the player to sprint. Default `LeftControl`.
`PlayerSprintingSpeedMultiplier` | The multiplier applied to the player speed when sprinting. Default 2.
`SprintingStaminaDrainPerSecond` | The stamina drain each second while sprinting. Default 15.

## Compatibility
* Works with Stardew Valley 1.6.9+ on Linux/macOS/Windows.
* Works in single-player, multiplayer, and split-screen.
* No known mod conflicts.

## See also
* [Release notes](release-notes.md)
