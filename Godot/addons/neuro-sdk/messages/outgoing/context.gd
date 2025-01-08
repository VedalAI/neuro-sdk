class_name Context
extends OutgoingMessage

var _message: String
var _silent: bool

func _init(message: String, silent: bool = false):
	_message = message
	_silent = silent

func _get_command() -> String:
	return "context"

func _get_data():
	return {
		"message": _message,
		"silent": _silent
	}

static func send(message: String, silent: bool = false):
	Websocket.send(Context.new(message, silent))
