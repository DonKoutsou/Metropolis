using Godot;
using System;
using System.Collections.Generic;

public class MainWorld : Spatial
{
	[Export]
	PackedScene WorldScene = null;

	MyWorld m_myworld;
	StartingScreen screen = null;

	public StartingScreen GetStartingScreen()
	{
		return screen;
	}
	public override void _Ready()
	{
		screen = GetNode<CanvasLayer>("CanvasLayer").GetNode<StartingScreen>("StartScreen");
	}
	public bool IsMapSpawned()
	{
		return m_myworld != null;
	}
	public void SpawnMap()
	{
		
		m_myworld = (MyWorld)WorldScene.Instance();

		AddChild(m_myworld);

	}
	public void StopGame()
	{
		m_myworld.QueueFree();
		m_myworld = null;
	}
}
