class_name Startup
extends OutgoingMessage

func _get_command() -> String:
	return "startup"

func get_ws_message() -> WsMessage:
	var ws_message: WsMessage = super()
	ws_message.data = null
	return ws_message
