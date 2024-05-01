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

	//public override void _Ready()
	//{
	//	
	//}
    private void On_Player_Entered(object body)
    {
        Player pl = (Player)body;
        if (pl == null)
            return;
        if (OnceEnter)
        {
            if (Entered)
                return;
        }
        Entered = true;
                
        TalkText.GetInst().Talk(EnterDialogues[TimesEntered], pl);
                
        TimesEntered += 1;
    }
    private void On_Player_Left(object body)
    {
        Player pl = (Player)body;
        if (OnceLeave)
        {
            if (Left)
                return;
        }
        Left = true;

        TalkText.GetInst().Talk(LeaveDialogues[TimesLeft], pl);

        TimesLeft += 1;
    }
}



