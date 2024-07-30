class_name IleSaveInfo
extends Resource

func _SetData(data):
	Type = data["Type"]
	Pos = data["Pos"]
	Scene = data["Scene"]
	SpecialName = data["SpecialName"]
	ImageIndex = data["ImageIndex"]
	Houses = data["Houses"]
	Furnitures = data["Furnitures"]
	Generators = data["Generators"]
	Vehicles = data["Vehicles"]
	Characters = data["Characters"]
	Items = data["Items"]
	Rotation = data["Rotation"]
	KeepInstance = data["KeepInstance"]
	Visited = data["Visited"]
	HasPort = data["HasPort"]
	if (HasPort) :
		Ports = data["Ports"]


export var Type:int = 0
export var Pos:Vector2 = Vector2.ZERO
export var Scene:PackedScene
export var SpecialName: String
export var ImageIndex:int = 0
export (Array, Resource) var Houses
export (Array, Resource) var Furnitures
export (Array, Resource)  var Generators
export (Array, Resource)  var Vehicles
export (Array, Resource)  var Items
export (Array, Resource) var Characters
export var HasPort :bool
export (Array, Resource) var Ports
export var Rotation:float
export var KeepInstance:bool
export var Visited:bool
