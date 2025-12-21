class_name ActionsForce
extends OutgoingMessage

var _query: String
var _state
var _ephemeral_context: bool
var _action_names: Array[String]
var _priority: Priority

enum Priority {
	LOW,
	MEDIUM,
	HIGH,
	CRITICAL,
}

const _PRIORITY_MAP = {
	Priority.LOW: "low",
	Priority.MEDIUM: "medium",
	Priority.HIGH: "high",
	Priority.CRITICAL: "critical",
}

func _init(query: String, state, ephemeral_context: bool, action_names: Array[String], priority: Priority = Priority.LOW):
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
        "priority": _PRIORITY_MAP.get(_priority),
	}
