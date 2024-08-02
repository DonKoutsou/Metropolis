class_name BreakableSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Destroyed = data["Destroyed"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var Destroyed:bool

