using Godot;
using System;
using System.Collections.Generic;

public class StartingScreen : Control
{
	
	MainWorld world;

	Dictionary<string, Control> ButtonList = new Dictionary<string, Control>();

	

	public override void _Ready()
	{
		world = (MainWorld)GetParent().GetParent();
		GetNode<Control>("Settings").Show();
		GetNode<Control>("MainScreen").Show();
		Control cont = GetNode<Control>("MainScreen");
		ButtonList.Add("Start", cont.GetNode<Button>("Start_Button"));
		ButtonList.Add("Exit", cont.GetNode<Button>("Exit_Button"));
		ButtonList.Add("SeedSetting", GetNode<Control>("Settings").GetNode<Panel>("SeedSetting"));
	}
	private void On_Start_Button_Down()
	{
		if (!world.IsMapSpawned())
		{
			GetNode<Control>("MainScreen").Hide();

			GetNode<LoadingScreen>("LoadingScreen").EnableTime();

			Control Startbut;
			ButtonList.TryGetValue("Start", out Startbut);
			Control SeedSet;
			ButtonList.TryGetValue("SeedSetting", out SeedSet);
			SeedSet.GetNode<TextEdit>("SeedText").Readonly = true;
			((Button)Startbut).Text = "Συνέχεια";
			GetNode<Control>("Settings").Hide();
			world.SpawnMap(0);

			MouseFilter = MouseFilterEnum.Ignore;
		}
		else
		{
			MyWorld.GetInstance().Pause();
		}
		
	}
	private void On_Setting_Button_Down()
	{
		GetNode<Control>("Settings").Show();
		GetNode<Control>("Settings").MouseFilter = MouseFilterEnum.Stop;
		GetNode<Control>("MainScreen").Hide();
		GetNode<Control>("MainScreen").MouseFilter = MouseFilterEnum.Ignore;
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

		GetNode<Control>("MainScreen").Visible = toggle;
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

		GetNode<Control>("MainScreen").Show();

		GetNode<Timer>("RestartTimer").Stop();
	}
}







