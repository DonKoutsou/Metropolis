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
		GetNode<LoadingScreen>("LoadingScreen").EnableTime();
		//foreach (Button but in WorldButtons)
		//{
		//	but.Hide();
		//}
		world.SpawnMap(0);
		GetNode<VideoPlayer>("VideoPlayer").Paused = true;
		GetNode<VideoPlayer>("VideoPlayer").Hide();
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







