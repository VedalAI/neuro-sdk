class_name ActionsRegister
extends OutgoingMessage

var _actions: Array[WsAction]

func _init(actions: Array[WsAction]):
	_actions = actions

func _get_command() -> String:
	return "actions/register"

func _get_data():
	return {
		"actions": _actions.map(func(action: WsAction) -> Dictionary: return action.to_dict())
	}

func merge(other: OutgoingMessage) -> bool:
	if not other is ActionsRegister:
		return false

	_actions = _actions.filter(func(my_action: WsAction) -> bool: return !other._actions.any(func(their_action: WsAction) -> bool: return my_action.name == their_action.name))
	_actions.append_array(other._actions)
	return true
