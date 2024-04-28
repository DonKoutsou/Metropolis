using Godot;
using System;

public class Street_Lamp : StaticBody
{
    SpotLight light;
    public override void _Ready()
    {
        light = GetNode<SpotLight>("SpotLight");
        DayNight dcont = DayNight.GetInstance();
        if (DayNight.IsDay())
            TurnOff();
        else
            TurnOn();
        dcont.Connect("DayEventHandler", this, "TurnOff");
        dcont.Connect("NightEventHandler", this, "TurnOn");
    }
    public void TurnOn()
    {
        light.Visible = true;
    }
    public void TurnOff()
    {
        light.Visible = false;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
