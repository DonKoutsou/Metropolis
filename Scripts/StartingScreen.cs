using Godot;
using System;
using System.Collections.Generic;

public class StartingScreen : Control
{
	
	MainWorld world;

	//int selectedmap = 0;
	
	//List <Button> WorldButtons = new List<Button>();

	public override void _Ready()
	{
		world = (MainWorld)GetParent().GetParent();
		//WorldButtons.Insert(0, GetNode<Button>("World1Button"));
		//WorldButtons.Insert(1, GetNode<Button>("World2Button"));
		//WorldButtons[0].Pressed = true;
	}
	private void On_Start_Button_Down()
	{
		//if (selectedmap == -1)
			//return;
		//GetNode<Label>("TitleLabel").Hide();
		GetNode<Button>("Start_Button").Hide();
		GetNode<Button>("Exit_Button").Hide();
		GetNode<Button>("Controlls_Button").Hide();
		GetNode<RichTextLabel>("SeedText2").Hide();
		GetNode<TextEdit>("SeedText").Hide();
		GetNode<LoadingScreen>("LoadingScreen").EnableTime();
		//foreach (Button but in WorldButtons)
		//{
		//	but.Hide();
		//}
		world.SpawnMap(0);
		GetNode<VideoPlayer>("VideoPlayer").Paused = true;
		GetNode<VideoPlayer>("VideoPlayer").Hide();
		MouseFilter = MouseFilterEnum.Ignore;
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
		
		GetNode<Label>("PauseLabel").Visible = toggle;
		GetNode<Button>("Exit_Button").Visible = toggle;
		GetNode<Button>("Controlls_Button").Visible = toggle;

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
		//GetNode<Label>("TitleLabel").Show();
		GetNode<Button>("Start_Button").Show();
		GetNode<Button>("World1Button").Show();
		GetNode<Button>("World2Button").Show();
		GetNode<Timer>("RestartTimer").Stop();
	}
	private void SetWorld1(bool button_pressed)
	{
		if (!button_pressed)
		{
			//selectedmap = -1;
			return;
		}
		//WorldButtons[1].Pressed = false;
		//selectedmap = 0;
	}
	private void SetWorld2(bool button_pressed)
	{
		if (!button_pressed)
		{
			//selectedmap = -1;
			return;
		}
		//WorldButtons[0].Pressed = false;
		//selectedmap = 1;
		// Replace with function body.
	}
}







