using Godot;
using System;
using System.Collections.Generic;

public class GameOverTrigger : Spatial
{
    [Export]
    GameOverType Type = GameOverType.Ending1; 
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
		start.GameEnded(Type);
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
		start.GameEnded(Type);
		SaveLoadManager.GetInstance().ClearSaves();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        Enabled = false;
    }
}
public enum GameOverType
{
    Ending1,
    Ending2,
    Ending3,
    Ending4
}


