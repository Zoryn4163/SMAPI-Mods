﻿**Movement Modifier** is a [Stardew Valley](http://stardewvalley.net/) mod which lets you customise
your movement.

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
* Works with Stardew Valley 1.3 on Linux/Mac/Windows.
* Works in single-player and multiplayer.
* No known mod conflicts.

## Versions
## Upcoming release
* Updated for the upcoming SMAPI 3.0.

### 2.6
* Updated to latest Stardew Valley 1.3.

### 1.9
* Updated to Stardew Valley 1.3 (including multiplayer).
* Enabled speed boost and sprinting by default.
* Simplified mod configuration.
* Removed walking speed boost (can break cutscenes).

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

### 1.3
* Updated to SMAPI 0.39.6.
* Added sprint feature.

### 1.2
* Updated to SMAPI 0.39.3.
* Added hotkey to reload `config.json`.
* Added option to override horse speed.

### 1.1
* Updated to SMAPI 0.39.1.

### 1.0
* Initial release. Compatible with SMAPI 0.38.4.
