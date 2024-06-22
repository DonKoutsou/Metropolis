using Godot;
using System;

public class Street_Lamp : StaticBody
{
    [Export]
    bool Working = true;
    Light light;
    SpatialMaterial LampMat;
    public override void _Ready()
    {
        light = GetNode<Light>("SpotLight");
        MeshInstance Lamp = GetNode<MeshInstance>("MeshInstance");
        LampMat = (SpatialMaterial)Lamp.GetActiveMaterial(0);
        
       
        if (!Working)
        {
            TurnOff();
            return;
        }
        DayNight dcont = DayNight.GetInstance();
        if (dcont == null)
            return;
        if (DayNight.IsDay())
            TurnOff();
        else
            TurnOn();
        dcont.Connect("DayEventHandler", this, "TurnOff");
        dcont.Connect("NightEventHandler", this, "TurnOn");
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        DayNight dcont = DayNight.GetInstance();
        if (dcont == null)
            return;
        
        dcont.Disconnect("DayEventHandler", this, "TurnOff");
        dcont.Disconnect("NightEventHandler", this, "TurnOn");
    }
    public void SetWorkingState(bool toggle)
    {
        Working = toggle;
    }
    public void TurnOn()
    {
        light.Visible = true;
        LampMat.EmissionEnabled = true;
    }
    public void TurnOff()
    {
        light.Visible = false;
        LampMat.EmissionEnabled = false;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
