class_name ActionsUnregister
extends OutgoingMessage

var _names: Array[String]

func _init(actions: Array[WsAction]):
	_names.assign(actions.map(func(action: WsAction) -> String: return action.name))

func _get_command() -> String:
	return "actions/unregister"

func _get_data():
	return {
		"action_names": _names
	}

func merge(other: OutgoingMessage) -> bool:
	if not other is ActionsUnregister:
		return false

	_names = _names.filter(func(my_name: String) -> bool: return !other._names.has(my_name))
	_names.append_array(other._names)
	return true
