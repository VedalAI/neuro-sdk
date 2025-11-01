# <img src="Assets/icon.png" width="29" style="vertical-align:middle;">  Neuro SDK

This repository contains the API documentation for allowing [Neuro-sama](https://twitch.tv/vedal987) to play games.

There are also official SDKs available for Unity and Godot, as well as community-maintained SDKs. If you would like to use the Neuro API in a different engine or programming language, you will have to implement the websocket protocol yourself. If you do so, consider opening a pull request to this repository to share your implementation with others by adding a link to the README.

## Changelog

Significant API and SDK changes will be documented in the [CHANGELOG.md](./CHANGELOG.md) file.

Last update: 29th of July 2025, 02:15 GMT

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

> [!Caution]
> These links are provided **solely for convenience** so that community members can find other implementations more easily.  
> We **do not review, verify, or endorse** any of the projects listed below, and they may change or stop working at any time.  
> Use any community-maintained SDKs, tools, or code **at your own risk**. We **do not take any responsibility** for their functionality, security, or reliability.

<details>
<summary>I understand. Click to expand and view list.</summary>

### SDKs
Third-party SDKs created and maintained by the community.
- [Rust SDK](https://github.com/chayleaf/rust-neuro-sama-game-api)
- [JavaScript/TypeScript SDK](https://github.com/AriesAlex/typescript-neuro-game-sdk)
- [Java SDK](https://github.com/alexcrea/jacn-sdk)
- [Lua SDK](https://github.com/Gunoshozo/lua-neuro-sama-game-api)
- [Gamemaker SDK](https://github.com/noellepunk/Neuro-Gamemaker-SDK)
- [C SDK](https://github.com/xslendix/libneurosdk)
- [Python SDK](https://github.com/CoolCat467/Neuro-API)
- [Kotlin SDK](https://github.com/RedEpicness/neuro-sdk-kotlin)
- [C++ SDK](https://github.com/chris-pie/neuro-sdk-websocketpp)
- [Generic C# SDK](https://github.com/pandapanda135/CSharp-Neuro-SDK)
- [Ren'Py SDK](https://github.com/caheuer/neuro-renpy-implementation#for-developers)

### Tools
- [Tony](https://github.com/Pasu4/neuro-api-tony) is a graphical testing interface, similar to Randy, but it allows the user to write messages manually.
- [Jippity](https://github.com/EnterpriseScratchDev/neuro-api-jippity) is a testing tool that aims to be a more "realistic" version of Randy, by using OpenAI to mimic Neuro.
- [Gary](https://github.com/Govorunb/gary) is another backend implementation for advanced use. Originally created for testing with local LLMs like Llama, now also has support for a Randy-like "random generator" mode and a web UI for Tony-like manual sending.

</details>

## Information 

The official SDKs have been created and optimized for turn-based games, namely Inscryption, Liar's Bar and Buckshot Roulette.

**Due to how the API works, we do not recommend using them with real-time games.**

Since you need to describe the entire game state in text, and receive actions in text, only games where that is feasible will work well with this API.
<details>
<summary>Examples</summary>

Games that could work
- Inscryption
- Liar's Bar
- Buckshot Roulette
- Keep Talking and Nobody Explodes
- Uno
- Monopoly
- Most visual novels
- Most card games

Games that would not work

- Among Us
- Skyrim
- League of Legends
- Celeste
- Euro Truck Sim
- CSGO
- Any real-time strategy game
- Shooters
- Platformers

You get the idea. Turn based games in general are perfect for this. Anything else and you're kinda stretching the limits of what the API can do.

Vedal said you can use this for more complex games but he told me "you wouldn't get it" so I'm not even going to try to imagine how this would even work, and I am just going to assume that he is wrong.

</details>
