@abstract
class_name OutgoingMessage

@abstract
func _get_command() -> String


func _get_data() -> Dictionary:
	return {}


func merge(_other: OutgoingMessage) -> bool:
	return false


func get_ws_message() -> WsMessage:
	return WsMessage.new(_get_command(), _get_data(), NeuroSdkConfig.game)
