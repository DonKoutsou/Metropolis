class_name HouseSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Locked = data["Locked"]
	Furniture = data["Furniture"]
	Decorations = data["Decorations"]

export var Name:String
export var Locked:bool
export (Array, Resource) var Furniture
export (Array, Resource) var Decorations