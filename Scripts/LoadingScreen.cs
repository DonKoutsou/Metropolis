using Godot;
using System;

public class LoadingScreen : Control
{
	int WaitTime;

	static int Wtime = 0;
	static LoadingScreen Instance;
	public override void _Ready()
	{
		base._Ready();
		Hide();
		Instance = this;
		SetProcess(false);
		
	}
	public static int GetWaitTime()
	{
		return Wtime;
	}
	public static LoadingScreen GetInstance()
	{
		return Instance;
	}
	public void Start()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
		GetNode<AnimationPlayer>("AnimationPlayer2").Play("Spinner");
		Show();
		GetNode<ProgressBar>("ProgressBar").MaxValue = WaitTime;
		Wtime = WaitTime;
		SetProcess(true);
	}
	public override void _Process(float delta)
	{
		base._Process(delta);
		WorldMap map = WorldMap.GetInstance();
		if (map == null)
			return;

		if (Wtime == 0)
		{
			GetNode<ProgressBar>("ProgressBar").MaxValue = map.GetIslandCount();
			Wtime = map.GetIslandCount();
		}
		GetNode<ProgressBar>("ProgressBar").Value = map.currentile;
		if (map.currentile == Wtime)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			SetProcess(false);
		}
	}
}
