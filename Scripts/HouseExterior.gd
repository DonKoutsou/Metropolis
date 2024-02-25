extends StaticBody

onready var mesh_instance = $MeshInstance


func Toggle(tog : bool) -> void:
	var mat := mesh_instance.material_override as SpatialMaterial
	if (tog):
		mat.params_cull_mode = mat.CULL_BACK
	else :
		mat.params_cull_mode = mat.CULL_DISABLED
