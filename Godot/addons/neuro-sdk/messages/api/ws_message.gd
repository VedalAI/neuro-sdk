class_name WsMessage

var command: String
var data
var game: String

func _init(command_: String, data_, game_: String):
	command = command_
	data = data_
	game = game_

func get_data() -> Dictionary:
	if data == null:
		return {
			"command": command,
			"game": game,
		}
	return {
		"command": command,
		"game": game,
		"data": data
	}
