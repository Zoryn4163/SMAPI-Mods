﻿**Health Bars** is a [Stardew Valley](http://stardewvalley.net/) mod which shows health bars for
monsters you encounter.

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
The mod will automatically show health bars for monsters you encounter.

## Configure
A `config.json` will appear in the mod's folder after you run the game once. You can optionally
open the file in a text editor to configure the mod. If you make a mistake, just delete the file
and it'll be regenerated.

Available fields:

field                         | purpose
----------------------------- | -------
`DisplayHealthWhenNotDamaged` | Whether to show a health bar for monsters at full health. Default false.
`DisplayMaxHealthNumber`      | Whether to show the maximum health number. Default true.
`DisplayCurrentHealthNumber`  | Whether to show the current health number. Default true.
`DisplayTextBorder`           | Whether to draw a border around text so it's more visible on some backgrounds. Default true.
`TextColor`                   | The text color. Default white.
`TextBorderColor`             | The text border color. Default black.
`LowHealthColor`              | The health bar color when the monster has low health. Default dark red.
`HighHealthColor`             | The health bar color when the monster has high health. Default lime green.
`BarWidth`                    | The health bar width in pixels. Default 90.
`BarHeight`                   | The health bar height in pixels. Default 15.
`BarBorderWidth`              | The health bar's vertical border width in pixels. Default 2.
`BarBorderHeight`             | The health bar's horizontal border width in pixels. Default 2.

## Compatibility
* Works with Stardew Valley 1.3 on Linux/Mac/Windows.
* Works in single-player and multiplayer.
* No known mod conflicts.

## Versions
## 2.7.1
* Updated for the upcoming SMAPI 3.0.

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
* Fixed health bar drawing code.

### 1.3
* Updated to SMAPI 0.39.6.

### 1.2
* Updated to SMAPI 0.39.3.
* Added hotkey to reload `config.json`.
* Fixed zoom handling.
* Fixed monsters that spawn after the location is loaded not having health bars.

### 1.1
* Updated to SMAPI 0.39.1.

### 1.0
* Initial release. Compatible with SMAPI 0.38.4.
