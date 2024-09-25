class_name ItemSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Position = data["Position"]
	SceneData = data["SceneData"]
	if (CustomDataKeys.size() > 0) :
		CustomDataKeys = data["CustomDataKeys"]
		CustomDataValues = data["CustomDataValues"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var Position:Vector3
export var SceneData:String
export (Array, String) var CustomDataKeys = []
export (Array, Resource) var CustomDataValues = []

