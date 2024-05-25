class_name FurnitureSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Searched = data["Searched"]
	HasItem = data["HasItem"]
	ItemType = data["ItemType"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var Searched:bool
export var HasItem:bool
export var ItemType:int

