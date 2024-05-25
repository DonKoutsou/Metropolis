class_name HouseSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Furniture = data["Furniture"]

export var Name:String
export (Array, Resource) var Furniture
