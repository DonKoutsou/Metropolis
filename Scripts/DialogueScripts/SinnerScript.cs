using Godot;
using System;
using System.Collections.Generic;

public class SinnerScript : BaseDialogueScript
{

    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {   
        string text = string.Empty;
        
        switch(DialogueProg)
        {
            case 0:
            {
                text = "Για κοίτα τη .";
                DialogueProg ++;
                break;
            }
            case 1:
            {
                text = "Αυτό που κουβαλάς είναι αυτό που νομίζω; Πηγένεις στην Μητρόπολη από'τι συμπερένω. Θα ήθελα να σε βοηθήσω στο ταξίδι σου αλλά δυστηχώς τα καΐκια που βλέπεις δεν λειτουργούν. Μου λείπουν τα εργαλεία και δεν μπορώ να τα επιδιορθώσω.";
                DialogueProg ++;
                break;
            }
            case 2:
            {
                text = string.Format("Μόλις επισκευάσω τα καΐκια είσαι ελεύθερος να πάρεις όποιο θες. Τα πανία τους θα σε βοηθήσουν να φτάσεις στην Μητρόπολη πολύ πιο εύκολά, εφόσον ο άνεμος είναι με το μέρος σου.");
                DialogueProg ++;
                break;
            }
            case 3:
            {
                text = "Ότι εργαλεία βρείς στα ταξίδια σου φέρτα εδώ και θα επισκευάσω τα καΐκια.";
                break;
            }
        }
        Talker.GetTalkText().Talk(text);
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
    public override string Action1Done(NPC owner, Player pl)
    {
        return "null";
    }
    public override string Action2Done(NPC owner, Player pl)
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
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
    }
}
