class_name DecoSaveInfo
extends Resource

func _SetData(data):
	SceneData = data["SceneData"]
	Placement = data["Placement"]
	Name = data["Name"]

## Path to the level that was loaded when the game was saved
export var SceneData:String
export var Placement:Transform
export var Name:String
