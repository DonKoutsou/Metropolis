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
		FillButtonList();
		world = (MainWorld)GetParent().GetParent();
		intro = GetNode<Control>("Intro");
		FadeInOut = GetNode<CanvasLayer>("FadeInOut").GetNode<MainMenuAnimation>("MainMenuAnimation");
		Init();
	}
	private void FillButtonList()
	{
		if (ButtonList.Count == 0)
		{
			Control cont = GetNode<Control>("Settings");
			ButtonList.Add("Start", cont.GetNode<Panel>("Panel").GetNode<Button>("Start_Button"));
			ButtonList.Add("StartHalf", cont.GetNode<Panel>("Panel").GetNode<Button>("Start_Button_Half"));
			ButtonList.Add("Continue", cont.GetNode<Panel>("Panel").GetNode<Button>("Continue_Button_Half"));
			ButtonList.Add("Exit", cont.GetNode<Panel>("Panel").GetNode<Button>("Exit_Button"));
			ButtonList.Add("SeedSetting", cont.GetNode<Panel>("Panel").GetNode<Panel>("SeedSetting"));
		}
	}
	public void Init()
	{
		FadeInOut.FadeOut();
		bool HasSave = ResourceLoader.Exists("user://SavedGame.tres");

		intro.Show();

		GetNode<CanvasLayer>("FadeInOut").Show();
		Control cont = GetNode<Control>("Settings");
		cont.Show();

		Control SeedSet = ButtonList["SeedSetting"];
		SeedSet.GetNode<TextEdit>("SeedText").Readonly = false;
		cont.GetNode<Panel>("Panel").GetNode<Button>("Start_Button").Text = "Έναρξη";
		if (HasSave)
			cont.GetNode<Panel>("Panel").GetNode<Button>("Start_Button").Hide();
		else
		{
			cont.GetNode<Panel>("Panel").GetNode<Button>("Start_Button_Half").Hide();
			cont.GetNode<Panel>("Panel").GetNode<Button>("Continue_Button_Half").Hide();
		}
	}
	private void On_Start_Button_Down()
	{
		if (!world.IsMapSpawned())
		{
			FadeInOut.FadeInOut();
			LoadingScreen.GetInstance().Start();
		}
		else
		{
			MyWorld.GetInstance().Pause();
		}
	}
	bool LoadSave = false;
	private void On_Continue_Button_Down()
	{
		LoadSave = true;
		FadeInOut.FadeInOut();
		LoadingScreen.GetInstance().Start();
	}

	public void StartGame()
	{
		intro.Hide();

		GetNode<Control>("Settings").Hide();

		GetNode<Control>("Settings").GetNode<ColorRect>("ColorRect").Visible = true;

		Control Startbut = ButtonList["Start"];
		Control StartHalf = ButtonList["StartHalf"];
		Control ContinueBut = ButtonList["Continue"];
		Control SeedSet = ButtonList["SeedSetting"];

		SeedSet.GetNode<TextEdit>("SeedText").Readonly = true;
		((Button)Startbut).Text = "Συνέχεια";
		Startbut.Show();
		StartHalf.Hide();
		ContinueBut.Hide();
		GetNode<Control>("Settings").Hide();
		MouseFilter = MouseFilterEnum.Ignore;
		world.CallDeferred("SpawnMap", LoadSave);
		GameIsRunning = true;
	}
	private void On_Full_Screen_Toggled(bool button_pressed)
	{
		OS.WindowFullscreen = !OS.WindowFullscreen;
	}
	
	private void On_Exit_Button_Down()
	{
		SaveLoadManager.GetInstance().SaveGame();
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
		GetNode<Label>("PauseLabel").Visible = toggle;	
	}
	public void GameOver(string reason = null)
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
		Init();

		//GetNode<Timer>("RestartTimer").Stop();
	}
}







