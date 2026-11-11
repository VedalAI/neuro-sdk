class_name SpeechStarted
extends OutgoingMessage

## Tells Neuro that a line of in-game dialogue has started being spoken out loud.
## Must be followed by a [SpeechEnded] with the same id when the line stops.

enum Reply {
	## Neuro is not prompted to respond to the dialogue.
	NEVER,
	## Neuro might comment on the dialogue once it has gone quiet.
	MAYBE,
	## Neuro will respond to the dialogue once it has gone quiet.
	ALWAYS,
}

const _REPLY_MAP = {
	Reply.NEVER: "never",
	Reply.MAYBE: "maybe",
	Reply.ALWAYS: "always",
}

var _id: String
var _speaker: String
var _text: String
var _expected_duration_ms: int
var _reply: Reply
var _responder: String

## Leave [param text], [param expected_duration_ms] or [param responder] at their defaults to omit them.
func _init(id: String, speaker: String, text: String = "", expected_duration_ms: int = 0, reply: Reply = Reply.MAYBE, responder: String = ""):
	assert(_REPLY_MAP.has(reply), "Invalid reply value: %s" % reply)
	_id = id
	_speaker = speaker
	_text = text
	_expected_duration_ms = expected_duration_ms
	_reply = reply
	_responder = responder

func _get_command() -> String:
	return "speech/started"

func _get_data() -> Dictionary:
	var data := {
		"id": _id,
		"speaker": _speaker,
		"reply": _REPLY_MAP.get(_reply),
	}
	if not _text.is_empty():
		data["text"] = _text
	if _expected_duration_ms > 0:
		data["expected_duration_ms"] = _expected_duration_ms
	if not _responder.is_empty():
		data["responder"] = _responder
	return data

## Sends a speech started message with a newly generated id.
## Returns the id of the line, to be passed to [method SpeechEnded.send] when the line stops.
static func send(speaker: String, text: String = "", expected_duration_ms: int = 0, reply: Reply = Reply.MAYBE, responder: String = "") -> String:
	var id := "%d-%d" % [Time.get_ticks_usec(), randi()]
	Websocket.send(SpeechStarted.new(id, speaker, text, expected_duration_ms, reply, responder))
	return id
