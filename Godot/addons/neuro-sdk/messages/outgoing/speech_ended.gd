class_name SpeechEnded
extends OutgoingMessage

## Tells Neuro that a line previously reported with [SpeechStarted] has stopped.

var _id: String

func _init(id: String):
	_id = id

func _get_command() -> String:
	return "speech/ended"

func _get_data() -> Dictionary:
	return {
		"id": _id
	}

static func send(id: String):
	Websocket.send(SpeechEnded.new(id))
