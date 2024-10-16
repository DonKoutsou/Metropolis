using Godot;
using System;
using System.Collections.Generic;

public class StartingScreen : Control
{
	
	WorldRoot world;

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
		world = (WorldRoot)GetParent().GetParent();
		intro = GetNode<Control>("Intro");
		FadeInOut = GetNode<CanvasLayer>("FadeInOut").GetNode<MainMenuAnimation>("MainMenuAnimation");
		FadeInOut.FadeOut();
		GetNode<Control>("FadeInOut/ColorRect").Show();
		Init();
		ActionTracker.LoadActions();
	}
	private void FillButtonList()
	{
		if (ButtonList.Count == 0)
		{
			ButtonList.Add("Start", GetNode<Button>("PauseMenu/Panel/VBoxContainer/PanelContainer/Start_Button"));
			ButtonList.Add("StartHalf", GetNode<Button>("PauseMenu/Panel/VBoxContainer/PanelContainer/HBoxContainer/Start_Button_Half"));
			ButtonList.Add("Continue", GetNode<Button>("PauseMenu/Panel/VBoxContainer/PanelContainer/HBoxContainer/Continue_Button_Half"));
			ButtonList.Add("Exit", GetNode<Button>("PauseMenu/Panel/VBoxContainer/Exit_Button"));
			ButtonList.Add("SeedSetting", GetNode<PanelContainer>("PauseMenu/Panel/VBoxContainer/SeedSetting"));
		}
	}
	public void Init()
	{
		bool HasSave = ResourceLoader.Exists("user://SavedGame.tres");

		intro.Show();

		//GetNode<CanvasLayer>("FadeInOut").Show();
		PauseMenu cont = GetNode<PauseMenu>("PauseMenu");
		cont.Show();

		Control SeedSet = ButtonList["SeedSetting"];
		SeedSet.GetNode<TextEdit>("HBoxContainer/SeedText").Readonly = false;
		GetNode<Button>("PauseMenu/Panel/VBoxContainer/PanelContainer/Start_Button").Text = LocalisationHolder.GetString("Έναρξη");
		if (HasSave)
		{
			ButtonList["Start"].Hide();
		}
		else
		{
			ButtonList["StartHalf"].Hide();
			ButtonList["Continue"].Hide();
		}
		cont.TakeFocus();
	}
	private void On_Start_Button_Down()
	{
		if (!world.IsMapSpawned())
		{
			FadeInOut.FadeInOut();
			LoadingScreen.GetInstance().Start();
			FadeInOut.Connect("FadeOutFinished", this, "StartGame");
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
		//LoadingScreen.GetInstance().Start();
		
		FadeInOut.Connect("FadeOutFinished", this, "StartGame");
	}
	
	public void StartGame()
	{
		FadeInOut.Disconnect("FadeOutFinished", this, "StartGame");

		intro.Hide();

		GetNode<Control>("PauseMenu").Hide();

		//GetNode<Control>("Settings").GetNode<ColorRect>("ColorRect").Visible = true;

		Control Startbut = ButtonList["Start"];
		Control StartHalf = ButtonList["StartHalf"];
		Control ContinueBut = ButtonList["Continue"];
		Control SeedSet = ButtonList["SeedSetting"];

		SeedSet.GetNode<TextEdit>("HBoxContainer/SeedText").Readonly = true;
		((Button)Startbut).Text = LocalisationHolder.GetString("Συνέχεια");
		Startbut.Show();
		StartHalf.Hide();
		ContinueBut.Hide();
		GetNode<Control>("PauseMenu").Hide();
		MouseFilter = MouseFilterEnum.Ignore;
		world.CallDeferred("SpawnMap", LoadSave);
		GameIsRunning = true;
	}
	private void On_Exit_Button_Down()
	{
		if (GameIsRunning)
		{
			SaveLoadManager.GetInstance().SaveGame();
		}
		ActionTracker.SaveActions();
		GetTree().Quit();
	}
	public void Pause(bool toggle)
	{
		if (toggle)
			MouseFilter = MouseFilterEnum.Stop;
		else
			MouseFilter = MouseFilterEnum.Ignore;

		//GetNode<Control>("MainScreen").Visible = toggle;
		GetNode<Control>("PauseMenu").Visible = toggle;
		GetNode<Label>("PauseLabel").Visible = toggle;
		GetNode<PauseMenu>("PauseMenu").TakeFocus();
	}
	public void GameOver(string reason = null)
	{
		GetNode<Timer>("RestartTimer").Start();
		GetNode<Label>("GameOverLabel").Visible = true;
		CameraAnimationPlayer.GetInstance().FadeOut(5);
	}
	public void GameEnded(GameOverType Type)
	{
		GetNode<Timer>("RestartTimer").Start();
		string endingtext = string.Empty;
		if (Type == GameOverType.Ending1)
			endingtext = "Τέλος1";
		else if (Type == GameOverType.Ending2)
			endingtext = "Τέλος2";
		else if (Type == GameOverType.Ending3)
			endingtext = "Τέλος3";
		else if (Type == GameOverType.Ending4)
			endingtext = "Τέλος4";

		ActionTracker.OnActionDone(endingtext);
		GetNode<Label>("GameOverLabel2/Panel10/Label").Text = LocalisationHolder.GetString(endingtext);
			
		GetNode<Label>("GameOverLabel2").Visible = true;
		
		CameraAnimationPlayer.GetInstance().FadeOut(5);
	}
	public void ShowCredits()
	{
		FadeInOut.FadeOut();

		intro.Show();

		//GetNode<CanvasLayer>("FadeInOut").Show();

		Credits cr = GetNode<Credits>("Credits");
		cr.Connect("OnCreditsEnded", this, "CreditEnded");
		GetNode<Credits>("Credits").PlayCredits();
	}
	public void CreditEnded()
	{
		Credits cr = GetNode<Credits>("Credits");
		cr.Disconnect("OnCreditsEnded", this, "CreditEnded");
		FadeInOut.FadeInOut();
		FadeInOut.Connect("FadeOutFinished", this, "ReInit");
	}
	public void ReInit()
	{
		FadeInOut.Disconnect("FadeOutFinished", this, "ReInit");
		Init();
	}
	private void StopGame()
	{
		GetNode<Label>("GameOverLabel").Visible = false;
		GetNode<Label>("GameOverLabel2").Visible = false;
		//GetNode<Control>("Settings").GetNode<ColorRect>("ColorRect").Visible = false;
		world.StopGame();

		GameIsRunning = false;
		ShowCredits();

		//GetNode<Timer>("RestartTimer").Stop();
	}
	
}







