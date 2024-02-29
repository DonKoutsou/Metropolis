using Godot;
using System;
using System.Collections.Generic;

public class MainWorld : Spatial
{
	[Export]
	List<string> WorldScene = new List<string>();

	[Export]
	PackedScene Intro;

	MyWorld m_myworld;
	StartingScreen screen = null;

	Spatial intro;
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
	public void SpawnMap(int index)
	{
		intro.QueueFree();
		var scene = GD.Load<PackedScene>(WorldScene[index]);
		m_myworld = (MyWorld)scene.Instance();
		m_myworld.Seed = screen.GetNode<TextEdit>("SeedText").Text.ToInt();
		AddChild(m_myworld);
	}
	public void StopGame()
	{
		
		m_myworld.QueueFree();
		m_myworld = null;
		intro = (Spatial)Intro.Instance();
		AddChild(intro, true);	
		
	}
	public static void SaveGame()
	{
		//(ThothGameState)save.Load
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
