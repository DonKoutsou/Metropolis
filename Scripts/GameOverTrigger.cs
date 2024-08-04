using Godot;
using System;
using System.Collections.Generic;

public class GameOverTrigger : Spatial
{
    static bool GlobalEnabled = true;

    public static void ToggleTriggers(bool t)
    {
        GlobalEnabled = t;
    }

    [Export]
    bool OnEnter = false;

    public bool Enabled = true;
    private void On_Player_Entered(object body)
    {
        if (!GlobalEnabled || !Enabled || !OnEnter)
            return;
        
        Player pl = (Player)body;

        if (pl.GettingInVehicle)
            return;
        StartingScreen start = WorldRoot.GetInstance().GetStartingScreen();
		start.GameEnded();
		SaveLoadManager.GetInstance().ClearSaves();
    }
    private void On_Player_Left(object body)
    {
        if (!GlobalEnabled || !Enabled || OnEnter)
            return;
        Player pl = (Player)body;
        if (pl.GettingInVehicle)
            return;
        StartingScreen start = WorldRoot.GetInstance().GetStartingScreen();
		start.GameEnded();
		SaveLoadManager.GetInstance().ClearSaves();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        Enabled = false;
    }
}



