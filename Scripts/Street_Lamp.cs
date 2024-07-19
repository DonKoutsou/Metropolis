using Godot;
using System;

public class Street_Lamp : StaticBody
{
    [Export]
    bool Working = true;
    Light light;
    SpatialMaterial LampMat;
    bool connected = false;
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
        Toggle(!DayNight.IsDay());
           //TurnOff();
        //else
            //TurnOn();

        if (connected)
            return;
        dcont.Connect("DayShift", this, "Toggle");
        //dcont.Connect("NightEventHandler", this, "TurnOn");
        connected = true;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        DayNight dcont = DayNight.GetInstance();
        if (dcont == null)
            return;
        if (!connected)
            return;
        dcont.Disconnect("DayShift", this, "Toggle");
        //dcont.Disconnect("NightEventHandler", this, "TurnOn");
        connected = false;
    }
    public void SetWorkingState(bool toggle)
    {
        Working = toggle;
    }
    public void Toggle(bool t)
    {
        light.Visible = t;
        LampMat.EmissionEnabled = t;
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
