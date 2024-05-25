class_name GeneratorSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	CurrentEnergy = data["CurrentEnergy"]
	DespawnDay = data["DespawnDay"]
	DespawnHour = data["DespawnHour"]
	DespawnMins = data["DespawnMins"]

## Path to the level that was loaded when the game was saved
export var Name:String
export var CurrentEnergy:float
export var DespawnDay:int = 0
export var DespawnHour:int = 0
export var DespawnMins:int = 0

