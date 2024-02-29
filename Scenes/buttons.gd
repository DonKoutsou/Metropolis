extends Control

signal load_pressed
signal save_pressed

func _on_load_pressed():
	emit_signal("load_pressed")

func _on_save_pressed():
	emit_signal("save_pressed")

func _on_back_pressed():
	get_tree().change_scene("res://scenes/main.tscn")
