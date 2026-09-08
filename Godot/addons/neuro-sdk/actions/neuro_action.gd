@abstract
class_name NeuroAction

var _action_window: ActionWindow

func _init(action_window: ActionWindow):
	_action_window = action_window

func get_name() -> String:
	return _get_name()

func can_be_used() -> bool:
	return _can_be_used()

func validate(data: IncomingData, state: Dictionary) -> ExecutionResult:
	if _action_window:
		return _action_window.result(_validate_action(data, state))
	return _validate_action(data, state)

func execute(state: Dictionary) -> void:
	_execute_action(state)

func get_ws_action() -> WsAction:
	return WsAction.new(_get_name(), _get_description(), _get_schema())

func _can_be_used() -> bool:
	return true

@abstract
func _get_name() -> String

@abstract
func _get_description() -> String

@abstract
func _get_schema() -> Dictionary

@abstract
func _validate_action(_data: IncomingData, _state: Dictionary) -> ExecutionResult

@abstract
func _execute_action(_state: Dictionary) -> void
