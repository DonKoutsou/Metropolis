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
        return false;
    }
    public virtual bool ShouldShowExtraAction2()
    {
        return false;
    }
    public virtual string GetExtraActionText()
    {
        return "Null";
    }
    public virtual string GetExtraActionText2()
    {
        return "Null";
    }
    public virtual void Action1Done(NPC owner, Player pl)
    {

    }
    public virtual void Action2Done(NPC owner, Player pl)
    {

    }
    public virtual Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
        };
        return savedata;
    }
    public virtual void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
    }
}
