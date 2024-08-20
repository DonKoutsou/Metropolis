using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LoadingScreen : Control
{
	static int Wtime = 0;
	static LoadingScreen Instance;

	List<StreamTexture> PicsToShow = new List<StreamTexture>();

	Random r;
	int LastPickedPic = -1;
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
		if (LastPickedPic != -1)
		{
			StreamTexture lasttexttex = PicsToShow[LastPickedPic];
			PicsToShow.Remove(lasttexttex);
			VisualServer.FreeRid(lasttexttex.GetRid());
		}
		

		if (PicsToShow.Count == 0)
		{
			RephreshPics();	
		}
		
		TextureRect t = GetNode<TextureRect>("LoadingPics");

		LastPickedPic = r.Next(0, PicsToShow.Count());
		StreamTexture tex = PicsToShow[LastPickedPic];
		
		t.Texture = tex;
		AnimationPlayer pl = GetNode<AnimationPlayer>("LoadingPic");
		pl.Play("ShowPic");
	}
	private void RephreshPics()
	{
		int image = 1;
		while (ResourceLoader.Exists(string.Format("res://Assets/LoadingPics/P{0}.png", image)))
		{
			StreamTexture im = ResourceLoader.Load<StreamTexture>(string.Format("res://Assets/LoadingPics/P{0}.png", image));
			PicsToShow.Add(im);
			image ++;
		}
	}
	public void Start()
	{
		RephreshPics();

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
			foreach (StreamTexture t in PicsToShow)
			{
				VisualServer.FreeRid(t.GetRid());
			}
			PicsToShow.Clear();
		}
	}
}
