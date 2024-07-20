using Godot;
using System;
using System.Collections.Generic;

public class VehicleHud : Control
{
    Vehicle CurrentVeh;
    static VehicleHud Instance;

    static public VehicleHud GetInstance()
    {
        return Instance;
    }
    Dictionary<string, Button> ButtonList = new Dictionary<string, Button>();
    public override void _Ready()
    {
        Instance = this;
        //MyWorld.GetInstance().Connect("PlayerSpawnedEventHandler", this, "ConnectToPlayer");
        Hide();
        HBoxContainer container = GetNode<Panel>("Panel").GetNode<MarginContainer>("MarginContainer").GetNode<HBoxContainer>("HBoxContainer");
        ButtonList.Add("Engine", container.GetNode<Label>("Label").GetNode<Button>("EngineToggle"));
        //ButtonList.Add("Sail", container.GetNode<Label>("Label2").GetNode<Button>("SailToggle"));
        ButtonList.Add("Lights", container.GetNode<Label>("Label3").GetNode<Button>("LightsToggle"));
    }
    public void ConnectToPlayer(Player pl)
    {
        pl.Connect("VehicleBoardEventHandler", this, "OnBoarding");
    }
    public void OnBoarding(bool toggle, Vehicle Veh)
    {
        if (toggle)
        {
            CurrentVeh = Veh;
            Show();
            SetProcess(true);
            //Button SailToggle;
            //ButtonList.TryGetValue("Sail", out SailToggle);
            //SailToggle.SetPressedNoSignal(CurrentVeh.HasDeployedWings());
            //if (CurrentVeh.GetWingCount() == 0)
            //{
            //    ((Control)SailToggle.GetParent()).Hide();
            //}
            //else
            //{
            //    ((Control)SailToggle.GetParent()).Show();
            //}
            Button LightsToggle;
            ButtonList.TryGetValue("Lights", out LightsToggle);
            LightsToggle.SetPressedNoSignal(CurrentVeh.LightCondition());
            Button EngineToggle;
            ButtonList.TryGetValue("Engine", out EngineToggle);
            EngineToggle.SetPressedNoSignal(false);


            //GetNode<RepairUI>("RepairUI").StartUI(Veh);
        }
        else
        {
            CurrentVeh = null;
            Hide();
            SetProcess(false);
            Button EngineToggle;
            ButtonList.TryGetValue("Engine", out EngineToggle);
            EngineToggle.SetPressedNoSignal(false);
            //Button SailToggle;
            //ButtonList.TryGetValue("Sail", out SailToggle);
            //SailToggle.SetPressedNoSignal(false);
            Button LightsToggle;
            ButtonList.TryGetValue("Lights", out LightsToggle);
            LightsToggle.SetPressedNoSignal(false);

            //GetNode<RepairUI>("RepairUI").StopUI();
        }
            
    }

    ///Signals////
    private void On_Engine_Toggled(bool button_pressed)
    {
        if (!CurrentVeh.ToggleMachine(button_pressed))
        {
            Button EngineToggle;
            ButtonList.TryGetValue("Engine", out EngineToggle);
            EngineToggle.SetPressedNoSignal(!button_pressed);
        }
    }
    /*private void On_Sail_Toggle(bool button_pressed)
    {
        if (!CurrentVeh.ToggleWings(button_pressed))
        {
            Button SailToggle;
            ButtonList.TryGetValue("Sail", out SailToggle);
            SailToggle.SetPressedNoSignal(!button_pressed);
        }
    }*/
    private void On_Lights_Toggle(bool button_pressed)
    {
        //CurrentVeh.ToggleLights(button_pressed);
        if (!CurrentVeh.ToggleLights(button_pressed))   
        {
            Button LightsToggle;
            ButtonList.TryGetValue("Lights", out LightsToggle);
            LightsToggle.SetPressedNoSignal(!button_pressed);
        }
    }
    public Button GetButton(string buttonname)
    {
        Button but;
        ButtonList.TryGetValue(buttonname, out but);
        return but;
    }
}
