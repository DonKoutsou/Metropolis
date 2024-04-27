using Godot;
using System;
using System.Collections.Generic;

public class StartingScreen : Control
{
	
	MainWorld world;

	Dictionary<string, Button> ButtonList = new Dictionary<string, Button>();

	public override void _Ready()
	{
		world = (MainWorld)GetParent().GetParent();
		GetNode<Control>("Settings").Hide();
		GetNode<Control>("MainScreen").Show();
		VBoxContainer cont = GetNode<Control>("MainScreen").GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer");
		ButtonList.Add("Start", cont.GetNode<Button>("Start_Button"));
		ButtonList.Add("Settings", cont.GetNode<Button>("Setting_Button"));
		ButtonList.Add("Exit", cont.GetNode<Button>("Exit_Button"));
	}
	private void On_Start_Button_Down()
	{

		GetNode<Control>("MainScreen").Hide();
		GetNode<Control>("Settings").GetNode<Panel>("SeedSetting").Hide();
		

		GetNode<LoadingScreen>("LoadingScreen").EnableTime();

		world.SpawnMap(0);

		MouseFilter = MouseFilterEnum.Ignore;
	}
	private void On_Setting_Button_Down()
	{
		GetNode<Control>("Settings").Show();
		GetNode<Control>("Settings").MouseFilter = MouseFilterEnum.Stop;
		GetNode<Control>("MainScreen").Hide();
		GetNode<Control>("MainScreen").MouseFilter = MouseFilterEnum.Ignore;
	}
	private void On_Settings_Back_Button_Down()
	{
		CloseSettings();
	}
	public void CloseSettings()
	{
		
		GetNode<Control>("Settings").Hide();
		GetNode<Control>("Settings").MouseFilter = MouseFilterEnum.Ignore;

		GetNode<Control>("MainScreen").Show();
		GetNode<Control>("MainScreen").MouseFilter = MouseFilterEnum.Stop;
	}
	private void On_Controlls_Button_Down()
	{
		GetNode<TextureRect>("Controlls").Show();
		GetNode<Button>("Controlls_Back").Show();
	}
	private void On_Controlls_Back_Button_Down()
	{
		GetNode<TextureRect>("Controlls").Hide();
		GetNode<Button>("Controlls_Back").Hide();
	}
	private void On_FullScreen_Button_Down()
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
		Button Startbut;
		ButtonList.TryGetValue("Start", out Startbut);
		Startbut.Hide();
		GetNode<Control>("MainScreen").GetNode<PanelContainer>("PanelContainer").Visible = toggle;
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







