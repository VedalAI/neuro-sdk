#nullable enable

using NeuroSdk.Messages.API;
using NeuroSdk.Websocket;
using Newtonsoft.Json.Linq;

namespace NeuroSdk.Messages.Incoming
{
    // ReSharper disable once UnusedType.Global
    public sealed class Startup : IncomingMessageHandler<Startup.ParsedData>
    {
        public sealed class ParsedData
        {
            public ParsedData(string characterId, string displayName)
            {
                CharacterId = characterId;
                DisplayName = displayName;
            }

            public string CharacterId { get; }
            public string DisplayName { get; }
        }

        public override bool CanHandle(string command) => command == "startup";

        protected override ExecutionResult Validate(string command, MessageJData messageData, out ParsedData? parsedData)
        {
            parsedData = null;

            if (messageData.Data is not JObject root) return ExecutionResult.Success();
            if (root["session"] is not JObject session) return ExecutionResult.Success();

            string characterId = session["characterId"]?.Value<string>() ?? "";
            if (characterId.Length == 0) return ExecutionResult.Success();

            string displayName = session["displayName"]?.Value<string>() ?? characterId;
            parsedData = new ParsedData(characterId, displayName.Length == 0 ? characterId : displayName);
            return ExecutionResult.Success();
        }

        protected override void ReportResult(ParsedData? parsedData, ExecutionResult result)
        {
        }

        protected override void Execute(ParsedData? parsedData)
        {
            if (parsedData == null) return;
            WebsocketConnection.Instance?.SetCharacterMetadata(
                new CharacterMetadata(parsedData.CharacterId, parsedData.DisplayName)
            );
        }
    }
}
