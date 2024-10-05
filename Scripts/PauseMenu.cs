using Godot;
using System;

public class PauseMenu : Control
{
	
	
	public static PauseMenu Instance { get; private set; }

	public override void _Ready()
	{
		//UpdateViewDistance();
		GetNode<TextEdit>("Panel/VBoxContainer/SeedSetting/HBoxContainer/SeedText").Text = SettingsPanel.Instance.Seed.ToString();
		//GetNode<ColorRect>("ColorRect").Visible = false;
		Instance = this;
	}
	private void OpenSettings()
	{
		bool achvisible = GetNode<SettingsPanel>("SettingsPanel").Visible;
		GetNode<AchievementManager>("AchievementManager").Visible = false;
		GetNode<SettingsPanel>("SettingsPanel").Visible = !achvisible;
		/*if (!achvisible)
			GetNode<Button>("SettingsPanel/Panel/GridContainer/Full_Screen_Check").GrabFocus();
		else
			GetNode<Button>("SettingsPanel/Panel/GridContainer/Full_Screen_Check").ReleaseFocus();*/
	}
	private void OpenAchievements()
	{
		bool achvisible = GetNode<AchievementManager>("AchievementManager").Visible;
		GetNode<SettingsPanel>("SettingsPanel").Visible = false;
		GetNode<AchievementManager>("AchievementManager").Visible = !achvisible;
		/*if (!achvisible)
			((Control)GetNode<Control>("AchievementManager/AchievementPanel/GridContainer").GetChild(0)).GetNode<Control>("Panel").GrabFocus();
		else
			((Control)GetNode<Control>("AchievementManager/AchievementPanel/GridContainer").GetChild(0)).GetNode<Control>("Panel").ReleaseFocus();*/
	}
	private void OnSettingsClosed()
	{
		TakeFocus();
	}
	public void TakeFocus()
	{
		if (GetNode<Button>("Panel/VBoxContainer/PanelContainer/HBoxContainer/Start_Button_Half").Visible)
			GetNode<Button>("Panel/VBoxContainer/PanelContainer/HBoxContainer/Start_Button_Half").GrabFocus();
		if (GetNode<Button>("Panel/VBoxContainer/PanelContainer/Start_Button").Visible)
			GetNode<Button>("Panel/VBoxContainer/PanelContainer/Start_Button").GrabFocus();
	}
	
	
}



