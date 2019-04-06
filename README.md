# ror2-modloader
#### A way to dynamically load mods in Risk of Rain 2

## What mods are there for this?

- [MaxPlayers mod](https://github.com/meepen/ror2-maxplayers-mod) by Meepen
- [Huntress 270 Degrees Run mod](https://github.com/meepen/ror2-huntress-sprint-mod) edited to work by Meepen
- Want to add your own mod to this list? Make a pull request [here](https://github.com/meepen/ror2-modloader/pulls)!


## How do I install this?

Download the latest release and follow the instructions in `HOW TO INSTALL.txt`

To install mods after installing, just place them in your `Mods` folder in `Risk of Rain 2`!


## Why do this include a mscorlib.dll?

This is required to provide Reflection.Emit, as the .NET Standard shipped with the game does not support it. These are taken from Unity's Editor for 4.5. They can be found in `net4.5` and must be put in the `Managed` folder if you build this yourself.


## How do I build this?

### Prerequisites
- [premake5](https://github.com/premake/premake-core/releases)
- [this modloader](https://github.com/meepen/ror2-modloader)
- A compiler for c++ and c#

### Building
- Run `premake5 <compiler>` (for example with Visual Studio 2017 `premake5 vs2017`)
- Build the project (vs2017 project is in project/ror2-modloader.sln)

### Release
- Run `readyrelease.exe` in the main folder
- This should make a `release.zip` for you to distribute.