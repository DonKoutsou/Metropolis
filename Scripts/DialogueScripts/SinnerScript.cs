using Godot;
using System;
using System.Collections.Generic;

public class SinnerScript : BaseDialogueScript
{

    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {   
        
    }
    public override bool ShouldShowExtraAction()
    {
        return true;
    }
    public override bool ShouldShowExtraAction2()
    {
        return true;
    }
    public override string GetExtraActionText()
    {
        return "Null";
    }
    public override string GetExtraActionText2()
    {
        return "Null";
    }
    public override string Action1Done()
    {
        return "null";
    }
    public override string Action2Done()
    {
        return "null";
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
        };
        return savedata;
    }
}
