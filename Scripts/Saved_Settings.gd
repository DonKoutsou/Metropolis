class_name SavedSettings
extends Resource

func _SetData(data):
	FullScreen = data["FullScreen"]
	Vsync = data["Vsync"]
	AAFXAA = data["AAFXAA"]
	SSAO = data["SSAO"]
	MSAA = data["MSAA"]
	MaxFPS = data["MaxFPS"]
	Resolution = data["Resolution"]
	Brightness = data["Brightness"]
	Contrast = data["Contrast"]
	Saturation = data["Saturation"]
	Sound = data["Sound"]

export var FullScreen:bool
export var Vsync:bool
export var AAFXAA:bool
export var SSAO:bool
export var MSAA:int
export var MaxFPS:int
export var Resolution:int
export var Brightness:float
export var Contrast:float
export var Saturation:float
export var Sound:float


