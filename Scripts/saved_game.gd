class_name SavedGame
extends Resource

func _SetData(data):
	currentile = data["currentile"]
	MahalasEntryID = data["ΜαχαλάςEntryID"]
	ExitID = data["ExitID"]
	OrderedCells = data["OrderedCells"]
	CurrentTile = data["CurrentTile"]
	finishedspawning = data["finishedspawning"]
	RandomisedEntryID = data["RandomisedEntryID"]
	ilemapvectors = data["ilemapVectors"]
	ilemap = data["ilemap"]
	playerlocation = data["PlayerLocation"]
	playerenergy = data["PlayerEnergy"]
	MapGridVectors = data["MapGridVectors"]
	MapGridTypes = data["MapGridTypes"]

## Path to the level that was loaded when the game was saved
export var currentile:int
export var MahalasEntryID:int
export var ExitID:int
export (Array, Vector2) var OrderedCells = []

export var CurrentTile:Vector2
export var finishedspawning:bool

## Saved data for all dynamic parts of the level
export (Array, int) var RandomisedEntryID = []

export (Array, Vector2) var ilemapvectors = []
export  (Array, Resource) var ilemap = []
export var playerlocation:Vector3
export var playerenergy:float
export (Array, Vector2) var MapGridVectors = []
export (Array, int) var MapGridTypes = []
