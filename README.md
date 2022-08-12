<p align="center">
    <img src="https://i.imgur.com/CgpwyIQ.png" width="190" height="200" >
</p>

An open source implementation of the Ultima Online Classic Client.

Individuals/hobbyists: support continued maintenance and development via the monthly Patreon:
<br>&nbsp;&nbsp;[![Patreon](https://raw.githubusercontent.com/wiki/ocornut/imgui/web/patreon_02.png)](http://www.patreon.com/classicuo)

Individuals/hobbyists: support continued maintenance and development via PayPal:
<br>&nbsp;&nbsp;[![PayPal](https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=9ZWJBY6MS99D8)

<a href="https://discord.gg/VdyCpjQ">
<img src="https://img.shields.io/discord/458277173208547350.svg?logo=discord"
alt="chat on Discord"></a>

[![GitHub Actions Status](https://github.com/ClassicUO/ClassicUO/workflows/Build-Test/badge.svg)](https://github.com/ClassicUO/ClassicUO/actions)
[![GitHub Actions Status](https://github.com/ClassicUO/ClassicUO/workflows/Deploy/badge.svg)](https://github.com/ClassicUO/ClassicUO/actions)

# Project dust765
This project is to address a problem constructed within the toxicity of this community. This is to show the community, open source projects are not meant for cliques and high school drama but rather the expansion of something greater, innovation. -A penny for your thoughts, the adder that prays beneath the rose.

# contact and team info

Discord: dust765#2787

Dust765: 7 Link, 6 G..., 5 S...

# feature showcase

[Video Part 1 on YouTube](https://youtu.be/074Osj1Fcrg)

[Video Part 2 on YouTube](https://youtu.be/P7YBrI3s6ZI)

[Video Part 3 on YouTube](https://youtu.be/aqHiiOhx8Q8)

# art / hue changes

Stealth footsteps color

Energy Bolt - art and color

Gold - art and color

Tree to stumps / tiles and color

Blockers to stumps / tiles and color

# visual helpers

Highlight tiles at range

Highlight tiles at range if spell is up

Preview field spells, wall of stone and area of effect spells

Glowing weapons

Color own aura by HP

Highlight lasttarget (more colors and options)

# healthbar

highlight lasttarget healthbar

color border by state

flashing outline (many options)

# cursor

Show spell on cursor (and runout countdown)

# Added files

/src/Dust765

# changed constants

WAIT_FOR_TARGET_DELAY 5000 -> 4000

MAX_CIRCLE_OF_TRANSPARENCY_RADIUS 200 -> 1000

DEATH_SCREEN_TIMER 1500 -> 750

MAX_JOURNAL_HISTORY_COUNT 100 -> 250

# Changed files and line number

no comment possible:

FILE        START   END     COMMIT
README.md   20      *

comments:

FILE                                            START   END     COMMIT
/src/Configuration/Profile.cs	                282     *       BASICSETUP

/src/Game/UI/Gumps/OptionsGump.cs	            177     179     BASICSETUP

/src/Game/UI/Gumps/OptionsGump.cs	            372     376     BASICSETUP

/src/Game/UI/Gumps/OptionsGump.cs	            453     457     BASICSETUP

/src/Game/UI/Gumps/OptionsGump.cs	            3380    3426    BASICSETUP

/src/Game/UI/Gumps/OptionsGump.cs	            4246    4248    BASICSETUP

/src/Game/Managers/MacroManager.cs              2252    2276    BASICSETUP

/src/Game/Managers/MacroManager.cs              2281    2284    BASICSETUP

/src/Game/Constants.cs                          87              CONSTANTS

/src/Game/Constants.cs                          108             CONSTANTS

/src/Game/Constants.cs                          122             CONSTANTS

/src/Game/Constants.cs                          127             CONSTANTS

/src/Game/GameObjects/Views/ItemView.cs         37      39      ART / HUE CHANGES

/src/Game/GameObjects/Views/ItemView.cs         91      97      ART / HUE CHANGES

/src/Game/GameObjects/Item.cs                   36      39      ART / HUE CHANGES

/src/Game/GameObjects/Item.cs                   126     161     ART / HUE CHANGES

/src/Game/GameObjects/Views/GameEffectView.cs   8       10      ART / HUE CHANGES

/src/Game/GameObjects/Views/GameEffectView.cs   110     124     ART / HUE CHANGES

/src/Game/GameObjects/Views/StaticView.cs       35      37      ART / HUE CHANGES

/src/Game/GameObjects/Views/StaticView.cs       95      108     ART / HUE CHANGES

/src/Game/GameObjects/Views/StaticView.cs       135     148     ART / HUE CHANGES

/src/Game/Scenes/GameSceneDrawingSorting.cs     38      40      ART / HUE CHANGES

/src/Game/Scenes/GameSceneDrawingSorting.cs     675     680     ART / HUE CHANGES

/src/Game/Scenes/GameSceneDrawingSorting.cs     698     702     ART / HUE CHANGES

/src/Game/Scenes/GameSceneDrawingSorting.cs     759     763     ART / HUE CHANGES

/src/Game/Scenes/GameSceneDrawingSorting.cs     854     858     ART / HUE CHANGES

/src/Game/GameObjects/Static.cs                 57      59      ART / HUE CHANGES

/src/Game/GameObjects/Static.cs                 71      75      ART / HUE CHANGES

/src/Game/GameObjects/Static.cs                 126     131     ART / HUE CHANGES

/src/Game/Managers/MacroManager.cs              1746    1756    ART / HUE CHANGES

/src/Game/GameObjects/Views/LandView.cs         34      36      VISUAL HELPERS

/src/Game/GameObjects/Views/LandView.cs         83      110     VISUAL HELPERS

/src/Game/GameObjects/Views/MultiView.cs        34      36      VISUAL HELPERS

/src/Game/GameObjects/Views/MultiView.cs        118     145     VISUAL HELPERS

/src/Game/GameObjects/Views/StaticView.cs       36      38      VISUAL HELPERS

/src/Game/GameObjects/Views/StaticView.cs       97      124     VISUAL HELPERS

/src/Game/GameObjects/Views/MobileView.cs       37      39      VISUAL HELPERS

/src/Game/GameObjects/Views/MobileView.cs       89      96      VISUAL HELPERS

/src/Game/GameObjects/Views/MobileView.cs       115     130     VISUAL HELPERS

/src/Game/GameObjects/Views/MobileView.cs       199     213     VISUAL HELPERS

/src/Game/GameObjects/Views/MobileView.cs       440     446     VISUAL HELPERS

/src/Game/GameObjects/Views/MobileView.cs       796     802     VISUAL HELPERS

/src/Game/GameCursor.cs                         37      39      VISUAL HELPERS

/src/Game/GameCursor.cs                         80      84      VISUAL HELPERS

/src/Game/GameCursor.cs                         348     353     VISUAL HELPERS

/src/Game/GameActions.cs                        54      56      VISUAL HELPERS

/src/Game/GameActions.cs                        656     659     VISUAL HELPERS

/src/Game/GameActions.cs                        670     673     VISUAL HELPERS

/src/Game/Managers/TargetManager.cs             35      37      VISUAL HELPERS

/src/Game/Managers/TargetManager.cs             163     166     VISUAL HELPERS

/src/Game/Managers/TargetManager.cs             191     193     VISUAL HELPERS

/src/Game/Managers/TargetManager.cs             235     238     VISUAL HELPERS

/src/Game/Managers/MacroManager.cs              1771    1776    VISUAL HELPERS

/src/Game/Managers/MacroManager.cs              2278    2280    VISUAL HELPERS

/src/Network/PacketHandlers.cs	                41      43      VISUAL HELPERS

/src/Network/PacketHandlers.cs	                828     831     VISUAL HELPERS

/src/Game/GameObjects/Mobile.cs                 146     149     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             359     362     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             490     493     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             580     583     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             594     597     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             601     690     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             752     781     HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             1035    1040    HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             1205    1210    HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             1312    1317    HEALTHBAR

/src/Game/UI/Gumps/HealthBarGump.cs             1359    1362    HEALTHBAR

/src/Game/GameCursor.cs                         38      40      CURSOR

/src/Game/GameCursor.cs                         87      91      CURSOR

/src/Game/GameCursor.cs                         360     373     CURSOR

# Introduction
ClassicUO is an open source implementation of the Ultima Online Classic Client. This client is intended to emulate all standard client versions and is primarily tested against Ultima Online free shards.

The client is currently under heavy development but is functional. The code is based on the [FNA-XNA](https://fna-xna.github.io/) framework. C# is chosen because there is a large community of developers working on Ultima Online server emulators in C#, because FNA-XNA exists and seems reasonably suitable for creating this type of game.

![screenshot_2020-07-06_12-29-02](https://user-images.githubusercontent.com/20810422/208747312-04f6782f-3dc8-4951-b0a0-73d2305bbfca.png)


ClassicUO is natively cross platform and supports:
* Browser [Chrome]
* Windows [DirectX 11, OpenGL, Vulkan]
* Linux   [OpenGL, Vulkan]
* macOS   [Metal, OpenGL, MoltenVK]

# Download & Play!
| Platform | Link |
| --- | --- |
| Browser | [Play!](https://play.classicuo.org) |
| Windows x64 | [Download](https://www.classicuo.eu/launcher/win-x64/ClassicUOLauncher-win-x64-release.zip) |
| Linux x64 | [Download](https://www.classicuo.eu/launcher/linux-x64/ClassicUOLauncher-linux-x64-release.zip) |
| macOS | [Download](https://www.classicuo.eu/launcher/osx/ClassicUOLauncher-osx-x64-release.zip) |

Or visit the [ClassicUO Website](https://www.classicuo.eu/)

# How to build the project

Clone repository with:
```
git config --global url."https://".insteadOf git://
git clone --recursive https://github.com/ClassicUO/ClassicUO.git
```

Build the project:
```
dotnet build -c Release
```

# Contribute
Everyone is welcome to contribute! The GitHub issues and project tracker are kept up to date with tasks that need work.

# Legal
The code itself has been written using the following projects as a reference:

* [OrionUO](https://github.com/hotride/orionuo)
* [Razor](https://github.com/msturgill/razor)
* [UltimaXNA](https://github.com/ZaneDubya/UltimaXNA)
* [ServUO](https://github.com/servuo/servuo)

Backend:
* [FNA](https://github.com/FNA-XNA/FNA)

This work is released under the BSD 4 license. This project does not distribute any copyrighted game assets. In order to run this client you'll need to legally obtain a copy of the Ultima Online Classic Client.
Using a custom client to connect to official UO servers is strictly forbidden. We do not assume any responsibility of the usage of this client.

Ultima Online(R) © 2022 Electronic Arts Inc. All Rights Reserved.
