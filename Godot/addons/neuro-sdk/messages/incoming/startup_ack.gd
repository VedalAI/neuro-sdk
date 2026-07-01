class_name StartupAck
extends IncomingMessage

func _can_handle(command: String) -> bool:
	return command == "startup"

func _validate(_command: String, message_data: IncomingData, state: Dictionary) -> ExecutionResult:
	var session := message_data.get_object("session", {})
	var character_id := session.get_string("characterId")
	if character_id == "":
		return ExecutionResult.success()

	var display_name := session.get_string("displayName", character_id)
	state["character_id"] = character_id
	state["display_name"] = display_name
	return ExecutionResult.success()

func _report_result(_state: Dictionary, _result: ExecutionResult) -> void:
	pass

func _execute(state: Dictionary) -> void:
	var character_id: String = state.get("character_id", "")
	if character_id == "":
		return

	var display_name: String = state.get("display_name", character_id)
	Websocket.set_character_metadata(character_id, display_name)
