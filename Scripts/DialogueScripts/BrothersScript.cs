using Godot;
using System;
using System.Collections.Generic;

public class BrothersScript : BaseDialogueScript
{
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {   
        string text = "Διάλογος 1";
        
        /*
        switch(DialogueProg)
        {
            case 0:
            {
                text = "Γεία σου καΐκτση, τι κάνεις εδώ; Δεν έχει και πολλά να δείς. Τελευταία φορά που ζούσε κάποιος εδώ ήταν.... \nΔυσκολεύομαι να θημηθώ...\nΕίσαι ευπρόσδεκτος πάντος, εγώ γυρνάω την όαση βρήσκοντας μέροι να ζωγραφίσω.";
                DialogueProg ++;
                break;
            }
            case 1:
            {
                text = "Eξερευνείς ή έχεις κάποιο προορισμό;";
                DialogueProg ++;
                break;
            }
            case 2:
            {
                text = string.Format("Ένα........ Απίστευτο. Έχω πόλλες ερωτήσεις αλλά πρέπει να βιαστείς, κατευθήνσου {0}. Πρόσεχε την ομήχλη, δεν είναι απλή σκόνη, δεν θα αντέξει το μωρό μέσα της για πολύ ώρα.", WorldMap.GetInstance().GetExitDirection());
                DialogueProg ++;
                break;
            }
            case 3:
            {
                text = "Αν στα ταξίδια σου βρείς λίγο χρώμα θα με βοηθούσες πολύ... Θα περιμένω εδώ.";
                break;
            }
            case 4:
            {
                /*switch (branch)
                {
                    case 0:
                    {
                        text = "Ευχαριστώ για τα χρώματα καΐκτση, μπορώ να τελειώσο τον πίνακα τώρα.";
                        break;
                    }
                    case 1:
                    {
                        text = ".......";
                        break;
                    }
                }
                break;
            }
        }*/
        string text2 = "Διάλογος 2";
        string text3 = "Διάλογος 3";
        string text4 = "Διάλογος 4";
        string text5 = "Διάλογος 5";
        string text6 = "Διάλογος 6";
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text);
        DialogueManager.GetInstance().ScheduleDialogue(TalkerColaborator, text2);
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text3);
        DialogueManager.GetInstance().ScheduleDialogue(TalkerColaborator, text4);
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text5);
        DialogueManager.GetInstance().ScheduleDialogue(TalkerColaborator, text6);
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
