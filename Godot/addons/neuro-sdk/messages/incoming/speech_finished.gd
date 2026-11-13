class_name SpeechFinished
extends IncomingMessage

func _can_handle(command: String) -> bool:
	return command == "speech_finished"

func _validate(_command: String, message_data: IncomingData, state: Dictionary) -> ExecutionResult:
	var is_final := message_data.get_boolean("isFinal")
	var cancelled := message_data.get_boolean("cancelled")
	var reason := message_data.get_string("reason")

	state["isFinal"] = is_final
	state["cancelled"] = cancelled
	state["reason"] = reason
	return ExecutionResult.success()

func _report_result(_state: Dictionary, _result: ExecutionResult) -> void:
	pass

func _execute(state: Dictionary) -> void:
	var is_final := state.get("isFinal")
	var cancelled := state.get("cancelled")
	var reason := state.get("reason")
	Websocket.set_speech_finished(is_final, cancelled, reason)
