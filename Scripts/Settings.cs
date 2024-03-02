using Godot;
using System;

public class Settings : Control
{
    public int ViewDistance = 3;

    public int TimeProgression = 1;
    public int Seed;

    static Settings set;

    public override void _Ready()
    {
        UpdateViewDistance();
        UpdateTimeProgression();
        var index = OS.GetDatetime();
        
        int thing = (int)index["hour"] + (int)index["minute"] + (int)index["second"];

        Random rand = new Random(thing);
        Seed = rand.Next(0, 99999);
        
        GetNode<Panel>("SeedSetting").GetNode<TextEdit>("SeedText").Text = Seed.ToString();
        set = this;
    }
    private void UpdateViewDistance()
    {
        GetNode<Panel>("ViewDistanceSetting").GetNode<RichTextLabel>("ViewDistanceNumber").BbcodeText = "[center]" + ViewDistance.ToString();
    }
    private void UpdateTimeProgression()
    {
        DayNight.UpdateTimeProgression(TimeProgression);
        GetNode<Panel>("TimeMultiplierSetting").GetNode<RichTextLabel>("TimeProgressionNumber").BbcodeText = "[center]" + TimeProgression.ToString();
    }
    private void IncreaseTimeProgression()
    {
        TimeProgression += 1;
        UpdateTimeProgression();
    }
    private void DecreaseTimeProgression()
    {
        if (TimeProgression == 1)
            return;
        TimeProgression -= 1;
        UpdateTimeProgression();
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
        TextEdit text = GetNode<Panel>("SeedSetting").GetNode<TextEdit>("SeedText");
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
