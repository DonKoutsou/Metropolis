using Godot;
using System;
using System.Collections.Generic;

public class BaseDialogueScript : Node
{
    protected int DialogueProg = 0;
    public virtual void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {   
        
    }
    public virtual bool ShouldShowExtraAction()
    {
        return true;
    }
    public virtual bool ShouldShowExtraAction2()
    {
        return true;
    }
    public virtual string GetExtraActionText()
    {
        return "Null";
    }
    public virtual string GetExtraActionText2()
    {
        return "Null";
    }
    public virtual string Action1Done()
    {
        return "null";
    }
    public virtual string Action2Done()
    {
        return "null";
    }
    public virtual Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
        };
        return savedata;
    }
}
