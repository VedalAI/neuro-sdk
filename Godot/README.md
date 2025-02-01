# Neuro Godot SDK

This is the documentation for the Godot version of the Neuro SDK. If you are looking for the general API documentation, look [here](../API/README.md).

This SDK has been built for and tested with Godot 4.1, but later versions of Godot 4 should also work.

If you encounter any issues while using this SDK, please open an issue in this repository.

## Installation

You can download this SDK from the [Godot Asset Library](https://godotengine.org/asset-library/asset/3576). The download there will always contain the latest commit on the `main` branch.

If the above download does not work, you can use the direct link to the [latest release](https://github.com/VedalAI/neuro-game-sdk/releases/tag/godot).

## Setup

1. Enable the plugin:
    - In the Godot editor, go to `Project > Project Settings > Plugins`,
    - Click on the Enable checkbox on the `Neuro SDK` entry to enable the plugin.
2. Set the `game` variable in the [`res://neuro_sdk_config.gd`](./neuro_sdk_config.gd) script to the name of your game.
3. Set the `NEURO_SDK_WS_URL` environment variable to the websocket URL you use for testing.

### Usage

Please refer to the [`USAGE.md`](./USAGE.md) file for information on how to use the SDK.
