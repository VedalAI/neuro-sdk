#nullable enable

using System;
using NeuroSdk.Messages.API;
using NeuroSdk.Websocket;
using Newtonsoft.Json;

namespace NeuroSdk.Messages.Outgoing
{
    /// <summary>
    /// Tells Neuro that a line of in-game dialogue has started being spoken out loud.
    /// Must be followed by a <see cref="SpeechEnded"/> with the same id when the line stops.
    /// </summary>
    public sealed class SpeechStarted : OutgoingMessageBuilder
    {
        public enum Reply
        {
            /// <summary>
            /// Neuro is not prompted to respond to the dialogue.
            /// </summary>
            Never,
            /// <summary>
            /// Neuro might comment on the dialogue once it has gone quiet.
            /// </summary>
            Maybe,
            /// <summary>
            /// Neuro will respond to the dialogue once it has gone quiet.
            /// </summary>
            Always
        }

        public SpeechStarted(string id, string speaker, string? text = null, int? expectedDurationMs = null, Reply reply = Reply.Maybe, string? responder = null)
        {
            if (!Enum.IsDefined(typeof(Reply), reply))
            {
                throw new ArgumentOutOfRangeException(nameof(reply), "Invalid reply value.");
            }

            Id = id;
            _speaker = speaker;
            _text = text;
            _expectedDurationMs = expectedDurationMs is > 0 ? expectedDurationMs : null;
            _reply = reply.ToString().ToLowerInvariant();
            _responder = responder;
        }

        protected override string Command => "speech/started";

        [JsonProperty("id", Order = 0)]
        public readonly string Id;

        [JsonProperty("speaker", Order = 10)]
        private readonly string _speaker;

        [JsonProperty("text", Order = 20)]
        private readonly string? _text;

        [JsonProperty("expected_duration_ms", Order = 30)]
        private readonly int? _expectedDurationMs;

        [JsonProperty("reply", Order = 40)]
        private readonly string _reply;

        [JsonProperty("responder", Order = 50)]
        private readonly string? _responder;

        /// <summary>
        /// Sends a speech started message with a newly generated id.
        /// </summary>
        /// <returns>The id of the line, to be passed to <see cref="SpeechEnded.Send"/> when the line stops.</returns>
        public static string Send(string speaker, string? text = null, int? expectedDurationMs = null, Reply reply = Reply.Maybe, string? responder = null)
        {
            string id = Guid.NewGuid().ToString();
            WebsocketConnection.Instance!.Send(new SpeechStarted(id, speaker, text, expectedDurationMs, reply, responder));
            return id;
        }
    }
}
