#nullable enable

using NeuroSdk.Messages.API;
using NeuroSdk.Websocket;
using Newtonsoft.Json;

namespace NeuroSdk.Messages.Outgoing
{
    /// <summary>
    /// Tells Neuro that a line previously reported with <see cref="SpeechStarted"/> has stopped.
    /// </summary>
    public sealed class SpeechEnded : OutgoingMessageBuilder
    {
        public SpeechEnded(string id)
        {
            Id = id;
        }

        protected override string Command => "speech/ended";

        [JsonProperty("id")]
        public readonly string Id;

        public static void Send(string id) => WebsocketConnection.Instance!.Send(new SpeechEnded(id));
    }
}
