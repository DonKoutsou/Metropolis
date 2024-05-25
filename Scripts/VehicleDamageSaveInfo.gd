class_name VehicleDamageSaveInfo
extends Resource

func _SetData(data):
	DestroyedWings = data["DestroyedWings"]
	LightCondition = data["LightCondition"]

## Path to the level that was loaded when the game was saved
export  (Array, Resource) var DestroyedWings
export var LightCondition:bool

