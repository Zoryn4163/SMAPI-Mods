﻿﻿**Health Bars** is a [Stardew Valley](http://stardewvalley.net/) mod which shows health bars for
monsters you encounter.

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
* Works with Stardew Valley 1.6+ on Linux/macOS/Windows.
* Works in single-player, multiplayer, and split-screen.
* No known mod conflicts.

## See also
* [Release notes](release-notes.md)
