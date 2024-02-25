extends MeshInstance

# Internal Variables.
var distance := 0.0
var is_running := false

# Play with these variables.
export(float) var max_distance := 20.0
export(float) var speed := 2.0

# Get the reference to the material to pass data to shader parameters.
onready var SHADER:ShaderMaterial = self.get_active_material(0)

# Don't forget to assign the start point node to this variable.
var player


func _ready():
	# Set the start point of the effect in the shader to the world position of
	# of the origin_point.
	player = get_parent().get_parent().get_parent().get_parent().get_parent()
	


func _process(delta):
	if is_running:
		SHADER.set_shader_param("start_point", player.get_global_transform().origin)
		distance += delta * speed
		if distance > max_distance:
			is_running = false
			# Set distance to 0 to stop shader from rendering the effect.
			distance = 0.0
		SHADER.set_shader_param("radius", distance)
	
	if Input.is_action_just_pressed("ui_accept"):
		is_running = true
		distance = 0.0
