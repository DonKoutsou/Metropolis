extends MultiMeshInstance

func Update(plpos) -> void:
	material_override.set_shader_param("player_position", plpos)
