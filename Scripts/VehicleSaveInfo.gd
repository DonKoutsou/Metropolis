class_name VehicleSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Location = data["Location"]
	Rotation = data["Rotation"]
	DamageInfo = data["DamageInfo"]
	PlayerOwned = data["PlayerOwned"]
	SceneData = data["SceneData"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var Location:Vector3
export var Rotation:Vector3
export var DamageInfo: Resource
export var PlayerOwned:bool
export var SceneData:String

