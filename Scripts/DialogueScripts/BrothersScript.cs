using Godot;
using System;
using System.Collections.Generic;

public class BrothersScript : BaseDialogueScript
{
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {   
        string text = string.Empty;
        
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
                }*/
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
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
    }
}
