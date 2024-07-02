class_name HouseSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Furniture = data["Furniture"]
	Decorations = data["Decorations"]

export var Name:String
export (Array, Resource) var Furniture
export (Array, Resource) var Decorations