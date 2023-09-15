# WavenGSI
![Minimum Waven version](https://img.shields.io/badge/Waven%20min%20version-0.12.5-blue)

WavenGSI is a BepInEx mod designed to add Game State Integration inside Waven for [Artemis](https://artemis-rgb.com/).

⚠️ This mod alone doesn't work, it needs to be installed with the corresponding [plugin](https://github.com/urfour/WavenGSI-Plugin) on Artemis! 

## Information collected

- Player information
  - Name
  - Hero (class and weapon)
  - Health
  - Level
- Fight information
  - Action, movement and reserve points 
  - Spells
- World information
  - Level name

It uses the IP provided by Artemis, so feel free to change the port if you want.

## Manual installation

- Download and extract the latest corresponding [BepInEx](https://builds.bepinex.dev/projects/bepinex_be) build (you have to choose the _BepInEx Unity (IL2CPP)_ corresponding to your OS) on your Waven directory
- Drop the mod .dll from the [releases](https://github.com/urfour/WavenGSI-Mod/releases) on the plugins folder inside BepInEx directory
- Enjoy!