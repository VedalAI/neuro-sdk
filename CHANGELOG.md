# Changelog

## 10th of November 2026
- Added the `speech/started` and `speech/ended` messages to the [API specification](./API/SPECIFICATION.md#speech-started). Games can use them to report in-game dialogue that is being spoken out loud, so that Neuro hears the line and does not talk over it. Guidance on using them has been added to [`BEST_PRACTICES.md`](./API/BEST_PRACTICES.md#in-game-dialogue).
- Added `SpeechStarted` and `SpeechEnded` messages to the Unity and Godot SDKs.
- Documented the `silent` parameter of the `actions/force` message. When `true`, Neuro picks an action without saying anything out loud about it. The default is `false`.

## 21st of August 2026
- Updated the action result guidance in [`BEST_PRACTICES.md`](./API/BEST_PRACTICES.md): successful results should include a `message` describing any state that changed as a result of her action that would be helpful for her to know, rather than omitting messages by default.
- Documented the server-side timeout on `action/result`: after about 20 seconds the action is treated as failed and late results are discarded.

## 20th of August 2026
- Added [`BEST_PRACTICES.md`](./API/BEST_PRACTICES.md), answering long-standing integration questions about context formatting, action design, result handling, forces and reconnection.
- Documented the `speech_finished` message. Clients should almost always wait for `isFinal: true`, since she has not really finished speaking until then.
- Documented that unrecognized commands and malformed messages are silently ignored, and that custom commands used by tooling should be vendor-prefixed.
- Corrected the `actions/register` documentation: registering an already-registered action name replaces the previous definition (it was previously documented as being ignored).

## 19th of August 2026
- Added the optional [voice chat side-channel](./API/VOICE_CHAT.md) for games with built-in voice chat: a second websocket at `/game/<name>/voice` carrying per-speaker PCM audio so Neuro can hear the other players (with attribution) and speak into the game's voice chat.
- Added `NeuroSdk.Voice.NeuroVoiceChat` to the Unity SDK and `NeuroVoiceChat` to the Godot SDK, wrapping the voice chat protocol (URL derivation, handshake, speaker management, resampling, graceful degradation when the server doesn't support voice).

## 1st of July 2026, 12:45 GMT
- Added a `startup` acknowledgement from Neuro to game clients with connected session metadata.
- Updated the Unity and Godot SDKs to expose connected character metadata (`characterId` and `displayName`) from the startup acknowledgement.

## 29th of December 2025, 19:20 GMT
- Updated Unity SDK to 2.0.0:
  - Added support for using the Neuro SDK with modded IL2CPP games.
  - Dependency on UniTask has been removed, and all inheritable async methods have been replaced with synchronous versions. If you still need to perform asynchronous operations, you can start your own UniTask, or just use coroutines.
  - Previously obsolete members have been removed, [check the pull request](https://github.com/VedalAI/neuro-sdk/pull/56) for a brief update guide.
  - Implemented `priority` parameter for `ActionWindow.SetForce`

## 22nd of December 2025, 22:00 GMT
- Updated Godot SDK:
  - Implemented `priority` parameter for `ActionWindow.set_force`

## 17th of December 2025, 15:00 GMT
- Added `priority` to the `actions/force` message. This allows you to specify how urgently Neuro should respond to the action force when she is speaking. The default is `"low"`, which will cause Neuro to wait until she finishes speaking before responding. `"medium"` causes her to finish her current utterance sooner. `"high"` prompts her to process the action force immediately, shortening her utterance and then responding. `"critical"` will interrupt her speech and make her respond at once. Use `"critical"` with caution, as it may lead to abrupt and potentially jarring interruptions. The default behavior before this was added is identical to `"low"`.

## 29th of July 2025, 02:15 GMT
- Added `multipleOf` to the list of unsupported keywords in Action schemas and noted that `uniqueItems` may or may not work, and you should perform your own checks accordingly.
- Clarified that Action schemas must have `"type": "object"`. If your schema has a different type, wrap it in an object with a property instead.

## 7th of January 2025, 12:15 GMT
- Added python script for running web-based games which can be found [here](./Web%20Game%20Runner/).
- Updated Unity SDK to 1.2.0:
  - `ActionWindow` now supports a fluent API. Additionally, `NeuroAction` no longer takes a constructor parameter of type `ActionWindow`, it is now set automatically by the SDK, so you can remove it.
  - `FormatString` is now internal, please replace it with your own implementation.

## 26th of December 2024, 19:00 GMT
- Fixed an issue in the Unity SDK which caused infinite task multiplication if the websocket client could not connect to the server ([#29](https://github.com/VedalAI/neuro-game-sdk/issues/29)). Please update as soon as possible to version `1.1.6`.

## 22nd of December 2024, 22:00 GMT
- The Godot SDK has been published to the [Godot Asset Library](https://godotengine.org/asset-library/asset/3576).

## 22nd of December 2024, 05:00 GMT
- The Unity SDK can now be used for modding Unity games built in Mono. Documentation can be found [here](https://github.com/VedalAI/neuro-game-sdk/tree/bb05509/Unity#for-modding).

## 17th of December 2024, 15:50 GMT
- The Unity SDK has been updated to work with WebGL builds. The websocket-sharp dependency has been removed as well. [Documentation](https://github.com/VedalAI/neuro-game-sdk/blob/9bd606e/Unity/README.md#webgl-additional-setup) for WebGL builds has been added.

## 17th of December 2024, 01:00 GMT
- The [API documentation for actions](https://github.com/VedalAI/neuro-game-sdk/blob/4549109/API/SPECIFICATION.md#action) has been updated to specify which JSON schema keywords should not be used.
