using Godot;
using System;
using System.Collections.Generic;

public class MainWorld : Spatial
{
	[Export]
	PackedScene WorldScene = null;

	[Export]
	PackedScene Intro = null;

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

		intro = (Spatial)Intro.Instance();
		AddChild(intro, true);
	}
	public bool IsMapSpawned()
	{
		return m_myworld != null;
	}
	public void SpawnMap(bool LoadSave)
	{
		intro.Free();

		m_myworld = (MyWorld)WorldScene.Instance();
		m_myworld.LoadSave = LoadSave;
		AddChild(m_myworld);

	}
	public void StopGame()
	{
		m_myworld.QueueFree();
		m_myworld = null;
		intro = (Spatial)Intro.Instance();
		AddChild(intro, true);
	}
}
