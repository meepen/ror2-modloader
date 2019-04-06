# ror2-modloader
#### A way to dynamically load mods in Risk of Rain 2

## How do I install this?

#### The first thing you **MUST DO** is rename `Risk Of Rain 2_Data/Plugins/AkHarmonizer.dll` to `AkHarmonizer_Original.dll`

After the first step, build or download the [Latest Release](https://github.com/meepen/ror2-modloader/releases/latest) and drag the files into your `Risk of Rain 2`.

To install mods afterwards, just place them in your `Mods` folder in `Risk of Rain 2`!


## Why do I include a mscorlib.dll?

These are required to provide Reflection.Emit, as the .NET Standard shipped with the game does not support it. These are taken from Unity's Editor for 4.5. They can be found in `net4.5` and must be put in the `Managed` folder if you build this yourself.

## What mods are there for this?

- [MaxPlayers mod](https://github.com/meepen/ror2-maxplayers-mod) by Meepen
- [Huntress 270 Degrees Run mod](https://github.com/meepen/ror2-huntress-sprint-mod) edited to work by Meepen
- Want to add your own mod? Make a pull request [here](https://github.com/meepen/ror2-modloader/pulls)!