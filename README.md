# Neuro Game SDK

This repository contains the API documentation for allowing [Neuro-sama](https://twitch.tv/vedal987) to play games.

There are also official SDKs available for Unity and Godot, as well as community-maintained SDKs. If you would like to use the Neuro API in a different engine or programming language, you will have to implement the websocket protocol yourself. If you do so, consider opening a pull request to this repository to share your implementation with others by adding a link to the README.

## Changelog

API changes will be documented in the [CHANGELOG.md](./CHANGELOG.md) file.

Last update: 17th of December 2024, 15:50 GMT

## Contents

### API Documentation
Please familiarize yourself with the [API documentation](./API/README.md) before using the SDKs.

### SDKs
SDKs created and maintained by Alex, which are located in this repository. Pull requests are welcome.
- [Unity SDK](./Unity/README.md)
- [Godot SDK](./Godot/README.md)

### Tools
- [Randy](./Randy/README.md) is a simple bot that mimics the Neuro API. It makes random actions and can be used to test your integration.

## Community-Maintained

### SDKs
Third-party SDKs created and maintained by the community.
- [Rust SDK](https://github.com/chayleaf/rust-neuro-sama-game-api)
- [JavaScript/TypeScript SDK](https://github.com/AriesAlex/typescript-neuro-game-sdk)
- [Java SDK](https://github.com/alexcrea/jacn-sdk)
- [Lua SDK](https://github.com/Gunoshozo/lua-neuro-sama-game-api)

### Tools
- [Neuro API Human Control](https://github.com/Pasu4/neuro-api-human-control) is a graphical testing interface, similar to Randy, but it allows the user to write messages manually.

## Information 

The official SDKs have been created and optimized for turn-based games, namely Inscryption, Liar's Bar and Buckshot Roulette.

**Due to how the API works, we do not recommend using them with real-time games.**

Since you need to describe the entire game state in text, and receive actions in text, only games where that is feasible will work well with this API.
<details>
<summary>Examples</summary>

- Inscryption? yes
- Liar's Bar? yes
- Buckshot Roulette? yes
- Among Us? not easily
- Skyrim? no
- League of Legends? no (ew)
- Celeste? no
- KTANE? yes
- Uno? YES
- Monopoly? YES
- Euro Truck Sim? no
- CSGO? no
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
