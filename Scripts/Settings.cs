using Godot;
using System;

public class Settings : Control
{
	[Export]
	public int ViewDistance = 2;


	[Export]
	public int TimeProgression = 1;

	[Export]
	public int FOVOverride = 30;
	
	public int Seed;

	static Settings set;

	public override void _Ready()
	{
		//UpdateViewDistance();
		UpdateTimeProgression();
		var index = OS.GetDatetime();
		
		int thing = (int)index["hour"] + (int)index["minute"] + (int)index["second"];

		Random rand = new Random(thing);
		Seed = rand.Next(0, 99999);
		
		GetNode<Panel>("Panel").GetNode<Panel>("SeedSetting").GetNode<TextEdit>("SeedText").Text = Seed.ToString();
		//GetNode<ColorRect>("ColorRect").Visible = false;
		set = this;
	}
	private void OpenSettings()
	{
		GetNode<SettingsPanel>("SettingsPanel").Visible = true;
	}
	private void UpdateViewDistance()
	{
		GetNode<Panel>("ViewDistanceSetting").GetNode<RichTextLabel>("ViewDistanceNumber").BbcodeText = "[center]" + ViewDistance.ToString();
	}
	private void UpdateTimeProgression()
	{
		DayNight.UpdateTimeProgression(TimeProgression);
		//GetNode<Panel>("TimeMultiplierSetting").GetNode<RichTextLabel>("TimeProgressionNumber").BbcodeText = "[center]" + TimeProgression.ToString();
	}
	private void UpdateFOV()
	{
		if (!StartingScreen.IsGameRunning())
			return;
		Camera	cam = GetTree().Root.GetCamera();
		if (cam != null)
			cam.Fov = FOVOverride;
	}
	public void IncreaseTimeProgression()
	{
		TimeProgression += 1;
		UpdateTimeProgression();
	}
	public void DecreaseTimeProgression()
	{
		if (TimeProgression == 1)
			return;
		TimeProgression -= 1;
		UpdateTimeProgression();
	}
	private void IncreaseFOV()
	{
		if (FOVOverride == 90)
			return;
		FOVOverride += 1;
		UpdateFOV();
	}
	private void DecreaseFOV()
	{
		if (FOVOverride == 25)
			return;
		FOVOverride -= 1;
		UpdateFOV();
	}
	private void IncreaseViewDistance()
	{
		ViewDistance += 1;
		UpdateViewDistance();
	}
	private void DecreaseViewDistance()
	{
		if (ViewDistance == 2)
			return;
		ViewDistance -= 1;
		UpdateViewDistance();
	}
	private void On_SeedText_changed()
	{
		TextEdit text = GetNode<Panel>("Panel").GetNode<Panel>("SeedSetting").GetNode<TextEdit>("SeedText");
		string newseedtext = text.Text;

		if (newseedtext == string.Empty)
		{
			text.Text = 0.ToString();
			text.CursorSetColumn(1);
			return;
		}

		if (!newseedtext.IsValidInteger())
		{
			text.Text = Seed.ToString();
			return;
		}
		
		if (newseedtext.Contains("\n"))
		{
			text.Text = newseedtext.Replace("\n", String.Empty);
			text.CursorSetColumn(newseedtext.Length - 1);
		}
			
		if (newseedtext.Contains("0"))
		{
			newseedtext = newseedtext.TrimStart('0');
			if (newseedtext == string.Empty)
			{
				text.Text = 0.ToString();
				text.CursorSetColumn(1);
				return;
			}
			text.Text = newseedtext;
			text.CursorSetColumn(newseedtext.Length);
		}

		int newseed = text.Text.ToInt();
		
		if (newseed > 100000)
		{
			text.Text = Seed.ToString();
			text.CursorSetColumn(Seed.ToString().Length);
			return;
		}
		if (newseed < 0)
		{
			text.Text = 0.ToString();
			text.CursorSetColumn(1);
			return;
		}
		Seed = text.Text.ToInt();
	}
	
	static public Settings GetGameSettings()
	{
		return set;
	}
	
}



