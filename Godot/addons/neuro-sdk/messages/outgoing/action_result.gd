class_name ActionResult
extends OutgoingMessage

var _id: String
var _success: bool
var _message

func _init(id: String, result: ExecutionResult):
	_id = id
	_success = result.successful
	_message = result.message

func _get_command() -> String:
	return "action/result"

func _get_data():
	return {
		"id": _id,
		"success": _success,
		"message": _message
	}
