using Godot;
using System;
using System.Collections.Generic;

public class WorldRoot : Spatial
{
	[Export]
	string WorldScene = "res://Scenes/World/MyWorld.tscn";
	[Export]
	string Intro = "res://Scenes/Islands/Test/IslandTestEscene.tscn";

	Spatial intro;

	MyWorld m_myworld;
	StartingScreen screen = null;

	static WorldRoot instance;

	public StartingScreen GetStartingScreen()
	{
		return screen;
	}
	public static WorldRoot GetInstance()
	{
		return instance;
	}
	public override void _Ready()
	{
		instance = this;
		screen = GetNode<CanvasLayer>("CanvasLayer").GetNode<StartingScreen>("StartScreen");

		intro = (Spatial)ResourceLoader.Load<PackedScene>(Intro).Instance();

		GetNode("Control/ViewportContainer/Viewport").AddChild(intro, true);

		GetNode<Control>("CanvasLayer/PlayerUI").Hide();
	}
	public bool IsMapSpawned()
	{
		return m_myworld != null;
	}
	public void SpawnMap(bool LoadSave)
	{
		intro.Free();
		m_myworld = (MyWorld)ResourceLoader.Load<PackedScene>(WorldScene).Instance();
		m_myworld.LoadSave = LoadSave;
		m_myworld.Connect("PlayerSpawnedEventHandler", PlayerUI.GetInstance(), "OnPlayerSpawned");
		GetNode("Control/ViewportContainer/Viewport").AddChild(m_myworld);

		
	}
	public void StopGame()
	{
		m_myworld.Free();
		m_myworld = null;
		intro = (Spatial)ResourceLoader.Load<PackedScene>(Intro).Instance();
		GetNode("Control/ViewportContainer/Viewport").AddChild(intro, true);
		OS.VsyncEnabled = false;
		MapGrid.GetInstance().ResetMap();
		GetNode<Control>("CanvasLayer/PlayerUI").Hide();
	}
}
