class_name InventoryItemSaveInfo
extends Resource

func _SetData(data):
	SceneData = data["SceneData"]
	if (CustomDataKeys.size() > 0):
		CustomDataKeys = data["CustomDataKeys"]
		CustomDataValues = data["CustomDataValues"]

## Path to the level that was loaded when the game was saved
export var SceneData:String
export (Array, String) var CustomDataKeys = []
export (Array, Resource) var CustomDataValues = []

