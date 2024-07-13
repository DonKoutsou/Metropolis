class_name PortSaveInfo
extends Resource

func _SetData(data):
	Location = data["Location"]
	Visited = data["Visited"]

export var Location:Vector2
export var Visited:bool