using Godot;
using System;

public class Settings : Control
{
    public int ViewDistance = 3;
    public int Seed;

    static Settings set;
    public override void _Ready()
    {
        UpdateViewDistance();
        var index = OS.GetDatetime();
        
        int thing = (int)index["hour"] + (int)index["minute"] + (int)index["second"];

        Random rand = new Random(thing);
        Seed = rand.Next(0, 99999);
        GetNode<TextEdit>("SeedText").Text = Seed.ToString();
        set = this;
    }
    private void UpdateViewDistance()
    {
        GetNode<Panel>("ViewDistanceSetting").GetNode<RichTextLabel>("ViewDistanceNumber").BbcodeText = "[center]" + ViewDistance.ToString();
    }
    private void IncreaseViewDistance()
    {
        ViewDistance += 1;
       UpdateViewDistance();
    }
    private void On_SeedText_changed()
    {
       Seed = GetNode<TextEdit>("SeedText").Text.ToInt();
    }
    private void DecreaseViewDistance()
    {
        ViewDistance -= 1;
       UpdateViewDistance();
    }
    static public Settings GetGameSettings()
    {
        return set;
    }
}
