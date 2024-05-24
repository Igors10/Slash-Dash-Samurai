# Cast Visualizer for Unity 3D

[![Minimal unity editor version](https://img.shields.io/badge/UnityEditor-2019.4%20or%20later-blue.svg)](https://unity3d.com/de/get-unity/download/archive)
[![CI](https://github.com/Dysman/bgTools-castVisualizer/workflows/CI/badge.svg)](https://github.com/Dysman/bgTools-castVisualizer/actions)&nbsp;&nbsp;
[![Release](https://img.shields.io/github/v/release/Dysman/bgTools-castVisualizer?include_prereleases&label=Release)](https://github.com/Dysman/bgTools-castVisualizer/releases)
[![GitHub package.json version (branch)](https://img.shields.io/github/package-json/v/dysman/bgTools-castVisualizer/upm?label=GitURL-UPM)](https://github.com/Dysman/bgTools-castVisualizer/tree/upm)
[![AssetStore](https://img.shields.io/badge/dynamic/xml?url=http://u3d.as/1RNW&label=UnityAssetStore&query=//*[contains(@class,%20%27product-version%27)]/div[2]&prefix=v)](http://u3d.as/1RNW)

Tool extension for the Unity Editor that visualizes all casts and overlap tests in the scene view for fast debugging. It will also display the hit results and requires no code modifications.

![Cast visualizer](https://www.bgranzow.de/downloads/CastVisualizerV1_0_0.png)

## Features

* Visualizes casts, overlap tests, and their results (hit points, colliders)
* Works with standard Unity functions, no code modification required
* Supports all cast types (check, cast, all, noAlloc)
* Shows casts for the whole project, including not self-programmed like in external plugins

### Provided visualizations

Physics
* Linecast
* Raycast
* Boxcast
* CapsuleCast
* SphereCast
* CheckBox
* CheckSphere
* CheckCapsule
* OverlapBox
* OverlapSphere
* OverlapCapsule

Physics2D
* Linecast
* Raycast
* BoxCast
* CapsuleCast
* CircleCast
* OverlapBox
* OverlapCircle
* OverlapCapsule
* OverlapArea
* OverlapCollider
* OverlapPoint

## Requirements

Unity Version: 2019.4.0 (LTS) or higher

## Installation

The plugin provides *manual* and *UPM* installation.

### Manual
Place the CastVisulizer folder somewhere in your project. It's not relevant where it's located, the plugin will find all of its files by itself.

### Unity Package Manager (UPM)
Through the Unity Plugin Manager it's possible to install the plugin direct from this git repository.
The UPM need a specific structure what will be provided into the *upm* branch.

Use following direct URL for the configuration:
```
https://github.com/Dysman/bgTools-castVisualizer.git#upm
```
See official Unity documentation for more informations: [UI](https://docs.unity3d.com/Manual/upm-ui-giturl.html) or [manifest.json](https://docs.unity3d.com/Manual/upm-git.html)

## Usage

The entry to open the _Cast Visualizer_ is located in the menubar at Tools/BG Tools/Cast Visualizer. It's a standard dockable window, so place it wherever it helps to be productive.
* GitHub (Manual)- [Manual page](Packages/CastVisualizer/Documentation~/CastVisualizer.md)
* GitHub (UPM) - Press the _Documentation_ link on the UPM description.
* Unity Asset Store Package - [MANUAL.html](MANUAL.html)