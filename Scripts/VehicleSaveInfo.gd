class_name VehicleSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Location = data["Location"]
	Rotation = data["Rotation"]
	PlayerOwned = data["PlayerOwned"]
	SceneData = data["SceneData"]

export var Name:String
export var Location:Vector3
export var Rotation:Vector3
export var PlayerOwned:bool
export var SceneData:String


