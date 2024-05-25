class_name CharacterSaveInfo
extends Resource

func _SetData(data, HasData):
	Name = data["Name"]
	Position = data["Position"]
	SceneData = data["SceneData"]
	if (HasData):
		CustomDataKeys = data["CustomDataKeys"]
		CustomDataValues = data["CustomDataValues"]

export var Name:String
export var Position:Vector3
export var SceneData:String
export (Array, String) var CustomDataKeys = []
export (Array, Resource) var CustomDataValues = []
