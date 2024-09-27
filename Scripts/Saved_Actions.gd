class_name SavedActions
extends Resource

func _SetData(data):
	DoneActions = data["DoneActions"]
	DoneActionCount = data["DoneActionCount"]

export (Array, String) var DoneActions
export (Array, int) var DoneActionCount
