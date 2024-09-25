class_name CharacterSaveInfo
extends Resource

func _SetData(data):
	Name = data["Name"]
	Position = data["Position"]
	SceneData = data["SceneData"]
	Energy = data["Energy"]
	Alive = data["Alive"]
	Talked = data["Talked"]
	##LimbColors = data["LimbColors"]
	if (data["OwnedVeh"] != null):
		OwnedVeh = data["OwnedVeh"]
	if (CustomDataKeys.size() > 0):
		CustomDataKeys = data["CustomDataKeys"]
		CustomDataValues = data["CustomDataValues"]

export var Name:String
export var Position:Vector3
export var SceneData:String
export var Energy:float
export var Alive:bool
export var Talked:bool
##export (Array, Color) var LimbColors = []
export (Array, String) var CustomDataKeys = []
export (Array, Resource) var CustomDataValues = []
export var OwnedVeh:NodePath
