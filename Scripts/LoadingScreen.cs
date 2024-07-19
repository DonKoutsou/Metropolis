using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LoadingScreen : Control
{
	[Export]
	StreamTexture[] LoadingImages = null;
	static int Wtime = 0;
	static LoadingScreen Instance;

	List<StreamTexture> PicsToShow = new List<StreamTexture>();

	Random r;
	public override void _Ready()
	{
		base._Ready();
		Hide();
		Instance = this;
		SetProcess(false);
		r = new Random();
	}
	public static int GetWaitTime()
	{
		return Wtime;
	}
	public static LoadingScreen GetInstance()
	{
		return Instance;
	}
	public void ChangePic(string anim)
	{
		if (PicsToShow.Count == 0)
		{
			foreach (StreamTexture te in LoadingImages)
				PicsToShow.Add(te);
		}
		TextureRect t = GetNode<TextureRect>("LoadingPics");

		StreamTexture tex = PicsToShow[r.Next(0, PicsToShow.Count())];
		PicsToShow.Remove(tex);
		t.Texture = tex;
		AnimationPlayer pl = GetNode<AnimationPlayer>("LoadingPic");
		pl.Play("ShowPic");
	}
	public void Start()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
		GetNode<AnimationPlayer>("AnimationPlayer2").Play("Spinner");
		
		ChangePic("thing");

		AnimationPlayer pl = GetNode<AnimationPlayer>("LoadingPic");
		pl.Connect("animation_finished", this, "ChangePic");

		Show();
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
		GetNode<ProgressBar>("ProgressBar").Value = map.IslandSpawnIndex;
		if (map.IslandSpawnIndex == Wtime)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			Hide();
			SetProcess(false);
			AnimationPlayer pl = GetNode<AnimationPlayer>("LoadingPic");
			pl.Disconnect("animation_finished", this, "ChangePic");
		}
	}
}
