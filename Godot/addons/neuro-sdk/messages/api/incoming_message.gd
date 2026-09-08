@abstract
class_name IncomingMessage
extends Node

func can_handle(command: String) -> bool:
	return _can_handle(command)

func validate(command: String, message_data: IncomingData, state: Dictionary) -> ExecutionResult:
	var result := _validate(command, message_data, state)
	if result == null:
		push_error("IncomingMessage._validate() returned null. An error probably occurred.")
		return ExecutionResult.mod_failure(Strings.action_failed_error)

	return result

func report_result(state: Dictionary, result: ExecutionResult) -> void:
	_report_result(state, result)

func execute(state: Dictionary) -> void:
	_execute(state)

@abstract
func _can_handle(_command: String) -> bool

@abstract
func _validate(_command: String, _data: IncomingData, _state: Dictionary) -> ExecutionResult

@abstract
func _report_result(_state: Dictionary, _result: ExecutionResult) -> void

@abstract
func _execute(_state: Dictionary) -> void
