[![Win/Mac/Linux](https://img.shields.io/badge/platform-windows%20%7C%20macos%20%7C%20linux-informational)]()
## TCA-Injector
A Bepinex Plugin to load asset bundles into the game Tiny Combat Arena

Copyright (c) 2023 JMayh with [MIT License](https://github.com/DuckMallard/TCA-Injector/blob/master/LICENSE.txt)

Credit to [Why485](https://twitter.com/Why485) and [Microprose](https://www.microprose.com/games/tiny-combat-arena/) for Tiny Combat Arena
___
### How does this work?
- The TCA-Exporter addon allows anyone to convert models in blender into assetbundles.
- This plugin (with the help of the plugin loader bepinex) allows TCA to access these assetbundles in game and display them.
- The plugin also allows JSON and assetbundles to be hot loaded, meaning that developing new assets and tweaking JSON values is much easier. Just exit to the main menu before hopping back into a gamemode and this plugin handles the rest.
___
### Mandatory Prerequisites
- [Tiny Combat Arena](https://store.steampowered.com/app/1347550/Tiny_Combat_Arena/) (0.11.1.2T) - You will need to have purchased Tiny Combat Arena to use this addon. Additionally this Addon contains no artwork, code or other data from the game.
- [Bepinex](https://github.com/BepInEx/BepInEx/releases) (5.4.21) **LTS** - Bepinex loads the the previously mentioned plugin. It is easy to install and can be used to run plugins for anyone. One day TCA might come with Bepinex already installed but for now its just a few clicks to setup..
### Optional Software:
- [TCA-Extractor Addon](https://github.com/DuckMallard/TCA-Extractor) (1.0.0) - With just the plugin you can play with mods created by others. But by using the extractor addon with Blender you can create your own models and mod them into TCA.
___
### Getting Started: Mod loading
- If you havent already install Bepinex from [here](https://github.com/BepInEx/BepInEx/releases). You want the x64 version, as TCA is a 64 bit executable.
- Unzip the contents of the download into the games root directory. In this case it will be somthing like `C:\Program Files (x86)\Steam\steamapps\common\TinyCombatArena`, you can check by making sure it unzips into the same folder that contains `Arena.exe`.
- Run `Arena.exe` once to setup Bepinex.
- Download the latest release of this plugin from [here](https://github.com/DuckMallard/TCA-Injector/releases/Latest) and place the `.dll` into the folder `something\TinyCombatArena\BepInEx\Plugins\`.
- Finally create the folder `something\TinyCombatArena\AssetBundles` (paying attention to the spelling and capitilisation). 
- From now on you can put any assetbundles you want TCA to have access to into the `AssetBundles\` and this plugin will load them every time you run TCA
___
### Notes:
- This is a very early release and has many bugs. Either open an issue, or hop on the [TCA Modding Server](https://discord.gg/D5ScNgcTJh) to receieve support.
- This Plugin is also in its infancy and cannot support some more complex modding. At the moment custom textures arent supported. This is the next area of development and should hopefully be functioning soon.