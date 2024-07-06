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

	public StartingScreen GetStartingScreen()
	{
		return screen;
	}
	public override void _Ready()
	{
		screen = GetNode<CanvasLayer>("CanvasLayer").GetNode<StartingScreen>("StartScreen");

		intro = (Spatial)ResourceLoader.Load<PackedScene>(Intro).Instance();
		AddChild(intro, true);
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
		AddChild(m_myworld);

	}
	public void StopGame()
	{
		m_myworld.Free();
		m_myworld = null;
		intro = (Spatial)ResourceLoader.Load<PackedScene>(Intro).Instance();
		AddChild(intro, true);
	}
}
