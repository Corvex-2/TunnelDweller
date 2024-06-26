# TunnelDweller

Tunnel Dweller is a tool originally meant to be used by speedrunners to find or practice strategies for the 4A Games Metro Games.

Currently Supported are the following games:

**Metro 2033: Redux**

**Metro Last Light: Redux**

**NONE OF THESE FEATURES ARE CURRENTLY ALLOWED TO BE USED IN SPEEDRUN SUBMISSIONS!**

[video](https://www.youtube.com/watch?v=zFGsGG-Mvx0)

# Requirements

The Staging Environement (TunnelDweller V2) requires the .NET 7 x64 Desktop Runtime. It can be downloaded [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

# Usage

1. Downlaod the Updater down below. Run the Updater, it should create a file called "TunnelDweller.Injector.exe"
2. Run the Injector
3. Select your release stream, we currently suggest using the Staging release stream, as the other ones contain issues related to module loading.
4. Start your game (2033 or Last Light Redux)
5. Hit the "Inject" Button. A command line Window should now appear and give you some random text.
6. Switch back to the game and hit the "DEL / DELETE" Button on your Keyboard to open the menu!
7. Select which Modules you want to use, they'll automatically be downloaded and executed. You can modify the settings of most modules on they seperate tab in the menu!

# Known Issues / FAQ

**Q**: I ran the Updater, but no file was created.

**A**: If you can access the download page, but the updater isn't creating any files, then it is possible, that you don't have the required .NET-Version installed on your system. In order to use the latest Version of Tunnel Dweller you'll need to have the .NET 7 x64 Desktop Runtime installed on your system. You can download the Runtime [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). 

## Download
You can downloas tunnel dweller here: [download.technicaldifficulties.de](https://download.technicaldifficulties.de/files/metro/core/TunnelDweller.Updater.exe)

## Features

#**Overlay**
```
The Tunnel Dweller Overlay allows you to view certain statistics that you usually can't see while in game.

1. Game Time - Displays the real/actual in game time, based on an internal counter from the games memory!
2. Position - Displays the precise coordinates of your player model (XYZ coordinates)
3. Rotation - Displays the current rotation of your player model (pitch, yaw and roll)
4. Resolution - Displays the current window width and height. 
5. Aspect Ratio - Displays the current aspect ratio (could be useful for people that seem to have issues with Widescreen! THERE IS A FIX FOR IT!)
6. Level ID - Displays the current Level ID or Name, depending on your setting!
7. Target FPS - Displays the maximum fps value that the game should target.
8. Crosshair - Displays a simple corsshair in the middle of your screen. possibly useful if you play ranger hardcore.

```

![image](https://github.com/Corvex-2/TunnelDweller/assets/22897151/0ad8ad5d-d9e4-4434-a8ef-c7ffe0e5d264)

### Speed Meter

Displays a Graph on your screen of your current velocity when moving. Allows you to find more optimal pathing, additionally it proves that knifing while running is faster!

### Widescreen Fix

Fixed a bug in the Redux games where the Aspect Ratio is not calculated properly, resulting int 16:9 Ratios even when using an Ultra Wide resolution like 21:9

### Warp Manager

Allows you to create "checkpoints" so to speak. You can create a warp point and teleport back to it later. 

Your rotation is stored as well!

![image3](https://github.com/Corvex-2/TunnelDweller/assets/22897151/e1052039-cbe6-4981-b7f4-43a932be5c15)

### Var Mod (Variable Modifier)

Allows you to modify and change certain in game variables like the field of view, speed, target fps and gamma value.
