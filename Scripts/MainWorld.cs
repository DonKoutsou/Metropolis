using Godot;
using System;
using System.Collections.Generic;

public class MainWorld : YSort
{
	[Export]
	List<string> WorldScene = new List<string>();

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
	public void SpawnMap(int index)
	{
		var scene = GD.Load<PackedScene>(WorldScene[index]);
		m_myworld = (MyWorld)scene.Instance();
		AddChild(m_myworld);
	}
	public void StopGame()
	{
		
		m_myworld.QueueFree();
		m_myworld = null;
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
