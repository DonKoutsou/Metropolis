class_name VehicleSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Location = data["Location"]
	Rotation = data["Rotation"]
	DamageInfo = data["DamageInfo"]
	Removed = data["Removed"]
	SceneData = data["SceneData"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var Location:Vector3
export var Rotation:Vector3
export var DamageInfo: Resource
export var Removed:bool
export var SceneData:String

