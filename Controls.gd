extends VehicleBody3D

@export var accel_rate: float = 5.0
@export var MAX_ENGINE_FORCE = 300.0
@export var MAX_BRAKE_FORCE = 300.0

@onready var camera = $CameraPivot/Camera3D
@onready var pivot = $CameraPivot

var brakeVal: float
var accelVal: float
var clutchVal: float
var steerVal: float

var look_at

func _init():
	accelVal = 1
	brakeVal = 1
	clutchVal = 1
	look_at = global_position

func _input(event):
	#### Steering wheel compatibility adjustments ####
	# Since racing wheel pedal inputs read different axes for input in and out,
	# negative and positive values need to be added together. In this case, it 
	# adds together negative for "out" and positive for "in" actions in order to
	# get a percentage for pedal in/out
	
#		accelVal = (Input.get_action_raw_strength("accelerate_in") + -Input.get_action_raw_strength("accelerate_out") + 1) / 2
	#accelVal = Input.get_axis("accel_neg", "accel_pos")
	accelVal = Input.get_action_raw_strength("accel_pos")
#		brakeVal = (Input.get_action_raw_strength("brake_in") + -Input.get_action_raw_strength("brake_out") + 1) / 2
	#brakeVal = Input.get_axis("brake_neg", "brake_pos")
	brakeVal = Input.get_action_raw_strength("brake_pos")
#		clutchVal = (-Input.get_action_raw_strength("clutch_out") + Input.get_action_raw_strength("clutch_in") + 1) / 2
	#clutchVal = Input.get_axis("clutch_neg", "clutch_pos")
	
	steerVal = Input.get_axis("steer_right", "steer_left")
	
	if event.is_action("reload"):
		global_position = Vector3(0, 0.45, 0)
		linear_velocity = Vector3(0, 0, 0)
		global_rotation = Vector3(0, 0, 0)
	
	pass

func _physics_process(delta):
	pivot.global_position = pivot.global_position.lerp(global_position, delta*20.0)
	pivot.transform = pivot.transform.interpolate_with(transform, delta*5.0)
	
	look_at = look_at.lerp(global_position + linear_velocity, delta * 5.0)
	
	camera.look_at(look_at)

func _process(delta):
		
	#engine_force = (2 - (accelVal + 1)) * MAX_ENGINE_FORCE
	#brake = (2 - (brakeVal + 1)) * MAX_BRAKE_FORCE
	engine_force = lerp(engine_force, accelVal * MAX_ENGINE_FORCE, delta * 2.0)
	brake = lerp(brake, brakeVal * MAX_BRAKE_FORCE, delta * 5.0)
	steering = lerp(steering, steerVal, delta * 5.0)
	
	
	## Debug pedal values
	#print_debug("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", brake / MAX_BRAKE_FORCE, ' ', engine_force / MAX_ENGINE_FORCE)
#	print_debug(steerVal, ' ', car.steering)
	
