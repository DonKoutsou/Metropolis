class_name SavedGame
extends Resource

func _SetData(data):
	currentile = data["currentile"]
	MahalasEntryID = data["ΜαχαλάςEntryID"]
	EventWallID = data["EventWallID"]
	ExitID = data["ExitID"]
	OrderedCells = data["OrderedCells"]
	CurrentTile = data["CurrentTile"]
	finishedspawning = data["finishedspawning"]
	RandomisedEntryID = data["RandomisedEntryID"]
	RandomTimes = data["RandomTimes"]
	Seed = data["Seed"]
	ilemapvectors = data["ilemapVectors"]
	ilemap = data["ilemap"]
	playerlocation = data["PlayerLocation"]
	##playerenergy = data["PlayerEnergy"]
	InventoryContents = data["InventoryContents"]
	playerHasVehicle = data["playerHasVehicle"]
	##DeliverJobs = data["DeliverJobs"]
	##ActiveDeliveryJob = data["ActiveDeliveryJob"]
	Date = data["Date"];
	if (playerHasVehicle):
		VehicleName = data["VehicleName"]
		##VehicleState = data["VehicleState"];
		##WingState = data["WingState"];
		##LightState = data["LightState"];

## Path to the level that was loaded when the game was saved
export var currentile:int
export var MahalasEntryID:int
export var EventWallID:int
export var ExitID:int
export (Array, Vector2) var OrderedCells = []

export var CurrentTile:Vector2
export var finishedspawning:bool

## Saved data for all dynamic parts of the level
export (Array, int) var RandomisedEntryID = []
export var RandomTimes:int
export var Seed:int

export (Array, Vector2) var ilemapvectors = []
export  (Array, Resource) var ilemap = []
export  (Array, Resource) var InventoryContents = []

export var playerlocation:Vector3
##export var playerenergy:float
export var playerHasVehicle:bool


export var VehicleName:String
##export var VehicleState:bool
##export var WingState:bool
##export var LightState:bool

##export (Array, Vector2) var DeliverJobs = []
##export var ActiveDeliveryJob:int
##export (Array, Vector2) var EscortJobs = []

export (Array, int) var Date = []
