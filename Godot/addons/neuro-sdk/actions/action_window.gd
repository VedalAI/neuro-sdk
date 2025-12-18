class_name ActionWindow
extends Node

enum State {
	STATE_BUILDING,
	STATE_REGISTERED,
	STATE_FORCED,
	STATE_ENDED
}
var _state: State = State.STATE_BUILDING
var _force_enabled: bool = false
var _force_timeout: float
var _action_force_query: String
var _action_force_state: String
var _action_force_ephemeral_context: bool
var _action_force_priority: String
var _end_enabled: bool = false
var _end_timeout: float
var _actions: Array[NeuroAction] = []
var _context_enabled: bool = false
var _context_message: String
var _context_silent: bool
var _timer: float = 0

func _init(parent: Node):
	parent.add_child(self)

func set_force(timeout: float, query: String, state: String, ephemeral_context: bool = false, priority: String = "low") -> void:
	if !_validate_frozen():
		return

	_force_enabled = true
	_force_timeout = timeout
	_action_force_query = query
	_action_force_state = state
	_action_force_ephemeral_context = ephemeral_context
	_action_force_priority = priority

func set_end(end_timeout: float) -> void:
	if !_validate_frozen():
		return

	_end_enabled = true
	_end_timeout = end_timeout

func set_context(message: String, silent: bool = false) -> void:
	if !_validate_frozen():
		return

	_context_enabled = true
	_context_message = message
	_context_silent = silent

func add_action(action: NeuroAction) -> void:
	if !_validate_frozen():
		return

	if action.can_be_used():
		_actions.append(action)

func register() -> void:
	if _state != State.STATE_BUILDING:
		push_error("Cannot register an ActionWindow more than once.")
		return

	if _actions.size() == 0:
		push_error("Cannot register an ActionWindow with no actions.")
		return

	if _context_enabled:
		Context.send(_context_message, _context_silent)
	NeuroActionHandler.register_actions(_actions)

	_state = State.STATE_REGISTERED

func result(execution_result: ExecutionResult) -> ExecutionResult:
	if _state == State.STATE_BUILDING:
		push_error("Cannot handle a result before registering.")
	elif _state == State.STATE_ENDED:
		push_error("Cannot handle a result after the ActionWindow has ended.")
	elif execution_result.successful:
		_end()
	elif _state == State.STATE_FORCED:
		pass #_send_force(); # Neuro is now responsible for retrying failed action forces

	return execution_result

func _validate_frozen() -> bool:
	if _state != State.STATE_BUILDING:
		push_error("Tried to mutate action after it was registered")
		return false
	return true

func _process(delta: float) -> void:
	if _state != State.STATE_REGISTERED:
		return

	_timer += delta

	if _force_enabled and _timer >= _force_timeout:
		_state = State.STATE_FORCED
		_force_enabled = false
		_send_force()

	if _end_enabled and _timer >= _end_timeout:
		_end()

func _send_force() -> void:
	var array: Array[String] = []
	array.assign(_actions.map(func(action: NeuroAction) -> String: return action.get_name()))
	Websocket.send(ActionsForce.new(_action_force_query, _action_force_state, _action_force_ephemeral_context, array, _action_force_priority))

func _end() -> void:
	NeuroActionHandler.unregister_actions(_actions)
	_end_enabled = false
	_state = State.STATE_ENDED
	queue_free()
