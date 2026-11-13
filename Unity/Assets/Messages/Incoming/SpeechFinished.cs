#nullable enable

using NeuroSdk.Messages.API;
using NeuroSdk.Websocket;
using Newtonsoft.Json.Linq;

namespace NeuroSdk.Messages.Incoming
{
	// ReSharper disable once UnusedType.Global
	public sealed class SpeechFinished : IncomingMessageHandler<SpeechFinished.ParsedData>
	{
		public sealed class ParsedData
		{
			public ParsedData(bool isFinal, bool? cancelled, string? reason)
			{
				IsFinal = isFinal;
				Cancelled = cancelled;
				Reason = reason;
			}

			public bool IsFinal { get; }
			public bool? Cancelled { get; }
			public string? Reason { get; }
		}

		public override bool CanHandle(string command) => command == "speech_finished";

		protected override ExecutionResult Validate(string command, MessageJData messageData,
			out ParsedData? parsedData)
		{
			parsedData = null;
			
			if (messageData.Data is not JObject root) return ExecutionResult.Success();

			bool isFinal = root.Value<bool>("isFinal");
			bool? cancelled = root.Value<bool?>("cancelled");
			string? reason = root.Value<string?>("reason");

			parsedData = new ParsedData(isFinal, cancelled, reason);
			return ExecutionResult.Success();
		}

		protected override void ReportResult(ParsedData? parsedData, ExecutionResult result)
		{
		}

		protected override void Execute(ParsedData? parsedData)
		{
			if (parsedData == null) return;
			WebsocketConnection.Instance?.SetSpeechFinished(
				new SpeechFinishedResult(parsedData.IsFinal, parsedData.Cancelled, parsedData.Reason)
			);
		}
	}
}