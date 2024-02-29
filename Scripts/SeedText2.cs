using Godot;
using System;

public class SeedText2 : TextEdit
{
    public override void _Ready()
    {
        var index = OS.GetDatetime();
        
        int thing = (int)index["hour"] + (int)index["minute"] + (int)index["second"];

        Random rand = new Random(thing);
        Text = rand.Next(0, 99999).ToString();
        
    }

}
