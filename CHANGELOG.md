# Changelog

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
