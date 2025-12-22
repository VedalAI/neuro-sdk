extends Node

const PlayOAction := preload("res://examples/play_o_action.gd")

@export var resetButton: BaseButton
@export var container: GridContainer

var _playerTurn := true

func _ready() -> void:
	NeuroSdkConfig.game = "Tic Tac Toe"
	Context.send("A Tic Tac Toe game has started. You are playing as O.", true)

	for child in container.get_children():
		if child is BaseButton:
			child.connect("pressed", func() -> void: player_play_in_cell(child))

	resetButton.connect("pressed", reset_board)

func player_play_in_cell(cell: BaseButton) -> void:
	if not _playerTurn: return
	if (cell.get_child(0) as Control).visible or (cell.get_child(1) as Control).visible: return

	_playerTurn = false

	(cell.get_child(0) as Control).visible = true
	Context.send("Your opponent played an X in the %s cell." % [cell.name], false)

	if not check_win():
		var actionWindow := ActionWindow.new(self)
		actionWindow.set_force(0, "It is your turn. Please place an O.", "", false)
		actionWindow.add_action(PlayOAction.new(actionWindow, self))
		actionWindow.register()
	else:
		enable_reset()

func bot_play_in_cell(cell: BaseButton) -> void:
	(cell.get_child(1) as Control).visible = true

	if not check_win():
		_playerTurn = true
	else:
		enable_reset()

func check_win() -> bool:
	if (check_line(0, 0, 1, 2) || check_line(0, 3, 4, 5) || check_line(0, 6, 7, 8) ||
		check_line(0, 0, 3, 6) || check_line(0, 1, 4, 7) || check_line(0, 2, 5, 8) ||
		check_line(0, 0, 4, 8) || check_line(0, 2, 4, 6)):
		Context.send("You lost. Better luck next time.", false)
		return true

	if (check_line(1, 0, 1, 2) || check_line(1, 3, 4, 5) || check_line(1, 6, 7, 8) ||
		check_line(1, 0, 3, 6) || check_line(1, 1, 4, 7) || check_line(1, 2, 5, 8) ||
		check_line(1, 0, 4, 8) || check_line(1, 2, 4, 6)):
		Context.send("You won. Congratulations.", false)
		return true

	if container.get_children().all(func(c: Node) -> bool: return (c.get_child(0) as Control).visible or (c.get_child(1) as Control).visible):
		Context.send("It's a tie. No one won.", false)
		return true

	return false

func check_line(player: int, c1: int, c2: int, c3: int) -> bool:
	return ((container.get_child(c1).get_child(player) as Control).visible &&
		(container.get_child(c2).get_child(player) as Control).visible &&
		(container.get_child(c3).get_child(player) as Control).visible)

func enable_reset() -> void:
	resetButton.visible = true

func reset_board() -> void:
	resetButton.visible = false

	for child in container.get_children():
		if child is BaseButton:
			(child.get_child(0) as Control).visible = false
			(child.get_child(1) as Control).visible = false

	Context.send("A new Tic Tac Toe game has started. You are playing as O.", true)

	_playerTurn = true
