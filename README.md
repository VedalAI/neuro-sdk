# Neuro Game SDK

This repository contains the API documentation for allowing [Neuro-sama](https://twitch.tv/vedal987) to play games.

There are also SDKs available for Unity and Godot. If you would like to use the Neuro API in a different engine or programming language, you will have to implement the websocket protocol yourself. If you do so, consider opening a pull request to this repository to share your implementation with others.

## Contents

### API Documentation
Please familiarize yourself with the API documentation before using the SDKs. Available at [API](./API/README.md).

### Unity SDK
Available at [Unity](./Unity/README.md).

### Godot SDK
Available at [Godot](./Godot/README.md).

### Community maintained SDKs

- [Rust SDK](https://github.com/chayleaf/rust-neuro-sama-game-api)

### Randy
Randy is a simple bot that mimics the Neuro API. It makes random actions and can be used to test your game. Available at [Randy](./Randy/README.md).

## Information 

The SDKs have been created and optimized for turn-based games, namely Inscryption, Liar's Bar and Buckshot Roulette.

**Due to how the API works, we do not recommend using them with real-time games.**

Since you need to describe the entire game state in text, and receive actions in text, only games where that is feasible will work well with this API.
<details>
<summary>Examples</summary>

- Inscryption? yes
- Liar's Bar? yes
- Buckshot Roulette? yes
- Among Us? not easily
- Skyrim? no
- League of Legends? no
- Celeste? no
- KTANE? yes
- Uno? YES
- Monopoly? YES
- Euro Truck Sim? no
- CSGO? no lol
- Almost any visual novel ====> YES
- Almost any card game ====> YES
- Any RTS ====> not easily
- Most FPP ====> NO
- Shooters ====> NO
- Platformers ====> NO
- Tic tac toe? yes

You get the idea. Turn based games in general are perfect for this. Anything else and you're kinda stretching the limits of what the API can do.

Vedal said you can use this for more complex games but he told me "you wouldn't get it" so you lot probably wouldn't get it either.

</details>
