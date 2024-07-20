class_name VehicleDamageSaveInfo
extends Resource

func _SetData(data):
	##WingStates = data["WingStates"]
	EngineStates = data["EngineStates"]
	LightCondition = data["LightCondition"]
	HullState = data["HullState"]

## Path to the level that was loaded when the game was saved
##export  (Array, Resource) var WingStates
export  (Array, Resource) var EngineStates
export var LightCondition:bool
export var HullState:int

