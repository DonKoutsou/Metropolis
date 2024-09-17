using Godot;
using System;
using System.Collections.Generic;

public class DialogueTrigger : Spatial
{
    [Export]
    bool OnceEnter = false;

    bool Entered = false;

    int TimesEntered = 0;

    [Export]
    bool OnceLeave = false;

    bool Left = false;

    int TimesLeft = 0;

    [Export]
    List<string> EnterDialogues = null;

    [Export]
    List<string> LeaveDialogues = null;

    public bool Enabled = true;

	//public override void _Ready()
	//{
	//	
	//}
    private void On_Player_Entered(object body)
    {
        if (!Enabled || EnterDialogues == null || EnterDialogues.Count == 0)
            return;
        Player pl = (Player)body;
        if (pl == null)
            return;

        if (OnceEnter)
        {
            if (Entered)
                return;
        }

        if (pl.GettingInVehicle)
            return;

        Entered = true;

        DialogueManager.GetInstance().ScheduleDialogue(pl, LocalisationHolder.GetString(EnterDialogues[TimesEntered]));     
        //pl.GetTalkText().Talk(EnterDialogues[TimesEntered]);
                
        TimesEntered += 1;
    }
    private void On_Player_Left(object body)
    {
        if (!Enabled || LeaveDialogues== null || LeaveDialogues.Count == 0)
            return;
        Player pl = (Player)body;
        if (OnceLeave)
        {
            if (Left)
                return;
        }

        if (pl.GettingInVehicle)
            return;

        Left = true;

        DialogueManager.GetInstance().ScheduleDialogue(pl, LeaveDialogues[TimesLeft]);
        //pl.GetTalkText().Talk(LeaveDialogues[TimesLeft]);

        TimesLeft += 1;
    }
}



