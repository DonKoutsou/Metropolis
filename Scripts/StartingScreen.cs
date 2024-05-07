using Godot;
using System;
using System.Collections.Generic;

public class StartingScreen : Control
{
	
	MainWorld world;

	Dictionary<string, Control> ButtonList = new Dictionary<string, Control>();

	Control intro;

	MainMenuAnimation FadeInOut;

	static bool GameIsRunning = false;
	public static bool IsGameRunning()
	{
		return GameIsRunning;
	}

	public override void _Ready()
	{
		world = (MainWorld)GetParent().GetParent();
		intro = GetNode<Control>("Intro");
		FadeInOut = GetNode<CanvasLayer>("FadeInOut").GetNode<MainMenuAnimation>("MainMenuAnimation");
		GetNode<CanvasLayer>("FadeInOut").Show();
		Control cont = GetNode<Control>("Settings");
		cont.Show();
		ButtonList.Add("Start", cont.GetNode<Panel>("Panel").GetNode<Button>("Start_Button"));
		ButtonList.Add("Exit", cont.GetNode<Panel>("Panel").GetNode<Button>("Exit_Button"));
		ButtonList.Add("SeedSetting", cont.GetNode<Panel>("Panel").GetNode<Panel>("SeedSetting"));
	}
	private void On_Start_Button_Down()
	{
		if (!world.IsMapSpawned())
		{
			FadeInOut.FadeInOut();
		}
		else
		{
			MyWorld.GetInstance().Pause();
		}
	}
	public void StartGame()
	{
		intro.Hide();

		GetNode<Control>("Settings").Hide();

		GetNode<Control>("Settings").GetNode<ColorRect>("ColorRect").Visible = true;

		Control Startbut;
		ButtonList.TryGetValue("Start", out Startbut);
		Control SeedSet;
		ButtonList.TryGetValue("SeedSetting", out SeedSet);
		SeedSet.GetNode<TextEdit>("SeedText").Readonly = true;
		((Button)Startbut).Text = "Συνέχεια";
		GetNode<Control>("Settings").Hide();
		MouseFilter = MouseFilterEnum.Ignore;
		world.CallDeferred("SpawnMap");
		GameIsRunning = true;
	}
	private void On_Full_Screen_Toggled(bool button_pressed)
	{
		OS.WindowFullscreen = !OS.WindowFullscreen;
	}
	
	private void On_Exit_Button_Down()
	{
		GetTree().Quit();
	}
	public void Pause(bool toggle)
	{
		if (toggle)
			MouseFilter = MouseFilterEnum.Stop;
		else
			MouseFilter = MouseFilterEnum.Ignore;

		//GetNode<Control>("MainScreen").Visible = toggle;
		GetNode<Control>("Settings").Visible = toggle;
	}
	public void GameOver()
	{
		GetNode<Timer>("RestartTimer").Start();
		GetNode<Label>("GameOverLabel").Visible = true;
	}
	public void GameWon(Character pl)
	{
		GetNode<Timer>("RestartTimer").Start();
		GetNode<Label>("GameWonLabel").Visible = true;
	}
	private void StopGame()
	{
		GetNode<Label>("GameOverLabel").Visible = false;
		GetNode<Label>("GameWonLabel").Visible = false;
		world.StopGame();

		GameIsRunning = false;
		GetNode<Control>("Settings").Show();

		GetNode<Timer>("RestartTimer").Stop();
	}
}







