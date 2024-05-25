class_name ItemSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Position = data["Position"]
	CustomDataKey = data["CustomDataKey"]
	CustomDataValue = data["CustomDataValue"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var Position:Vector3
export (Array, String) var CustomDataKey = []
export (Array, Resource) var CustomDataValue = []

