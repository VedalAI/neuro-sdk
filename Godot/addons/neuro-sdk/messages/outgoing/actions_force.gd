class_name ActionsForce
extends OutgoingMessage

var _query: String
var _state
var _ephemeral_context: bool
var _action_names: Array[String]
var _priority: String

func _init(query: String, state, ephemeral_context: bool, action_names: Array[String], priority: String = "low"):
	_query = query
	_state = state
	_ephemeral_context = ephemeral_context
	_action_names = action_names
	_priority = priority

func _get_command() -> String:
	return "actions/force"

func _get_data() -> Dictionary:
	return {
		"state": _state,
		"query": _query,
		"ephemeral_context": _ephemeral_context,
		"action_names": _action_names,
                "priority": priority,
	}
