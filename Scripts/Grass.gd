extends MultiMeshInstance


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var pl
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta) -> void:
	if (pl == null):
		var pls = get_tree().get_nodes_in_group("player")
		pl = pls[0]
		return
	if (pl.global_transform.origin.distance_to(global_transform.origin) > 1000):
		hide();
	else :
		show();
	material_override.set_shader_param("player_position", pl.global_transform.origin)
		
