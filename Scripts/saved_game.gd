class_name SavedGame
extends Resource

func _SetData(data):
	currentile = data["currentile"]
	##MahalasEntryID = data["ΜαχαλάςEntryID"]
	EventWallID = data["EventWallID"]
	ExitID = data["ExitID"]
	Exitpalcement = data["Exitpalcement"]
	##OrderedCells = data["OrderedCells"]
	CurrentTile = data["CurrentTile"]
	finishedspawning = data["finishedspawning"]
	RandomisedEntryID = data["RandomisedEntryID"]
	RandomTimes = data["RandomTimes"]
	Seed = data["Seed"]
	ilemapvectors = data["ilemapVectors"]
	UnlockedLightHouses = data["UnlockedLightHouses"]
	ilemap = data["ilemap"]
	playerlocation = data["PlayerLocation"]
	PlayerEnergy = data["PlayerEnergy"]
	HasBaby = data["HasBaby"]
	BabyAlive = data["BabyAlive"]
	InventoryContents = data["InventoryContents"]
	PlayerHasVehicle = data["PlayerHasVehicle"]
	##DeliverJobs = data["DeliverJobs"]
	##ActiveDeliveryJob = data["ActiveDeliveryJob"]
	Date = data["Date"];
	if (PlayerHasVehicle):
		VehicleName = data["VehicleName"]
		##VehicleState = data["VehicleState"];
		##WingState = data["WingState"];
		##LightState = data["LightState"];

## Path to the level that was loaded when the game was saved
export var currentile:int
##export var MahalasEntryID:int
export var EventWallID:int
export var ExitID:int
export var Exitpalcement:Vector2
##export (Array, Vector2) var OrderedCells = []

export var CurrentTile:Vector2
export var finishedspawning:bool

## Saved data for all dynamic parts of the level
export (Array, int) var RandomisedEntryID = []
export var RandomTimes:int
export var Seed:int

export (Array, Vector2) var ilemapvectors = []
export (Array, Vector2) var UnlockedLightHouses = []
export  (Array, Resource) var ilemap = []
export  (Array, Resource) var InventoryContents = []

export var playerlocation:Vector3
export var PlayerEnergy:float
export var HasBaby:bool
export var BabyAlive:bool
export var PlayerHasVehicle:bool


export var VehicleName:String
##export var VehicleState:bool
##export var WingState:bool
##export var LightState:bool

##export (Array, Vector2) var DeliverJobs = []
##export var ActiveDeliveryJob:int
##export (Array, Vector2) var EscortJobs = []

export (Array, int) var Date = []
