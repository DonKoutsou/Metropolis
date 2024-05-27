class_name IleSaveInfo
extends Resource

func _SetData(data):
	Type = data["Type"]
	Pos = data["Pos"]
	Scene = data["Scene"]
	##SpecialName = data["SpecialName"]
	Houses = data["Houses"]
	Generators = data["Generators"]
	Vehicles = data["Vehicles"]
	Characters = data["Characters"]
	Items = data["Items"]
	Rotation = data["Rotation"]


export var Type:int = 0
export var Pos:Vector2 = Vector2.ZERO
export var Scene:PackedScene
export var SpecialName: String
export (Array, Resource) var Houses
export (Array, Resource)  var Generators
export (Array, Resource)  var Vehicles
export (Array, Resource)  var Items
export (Array, Resource) var Characters
export var Rotation:float
