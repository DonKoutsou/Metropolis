using Godot;
using System;
using System.Collections.Generic;

public class PainterScript : BaseDialogueScript
{
    int ColorsGiven = 0;
    int branch = 0;
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
                switch (branch)
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
        }
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text);
    }
    public override bool ShouldShowExtraAction()
    {
        return DialogueProg == 3 && Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.PAINTCAN);
    }
    public override bool ShouldShowExtraAction2()
    {
        return DialogueProg == 3 && Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.BLOOD_VIAL);
    }
    public override string GetExtraActionText()
    {
        return "Δώσε κουβά με χρώμα.";
    }
    public override string GetExtraActionText2()
    {
        return "Δώσε σταγώνα αίματος.";
    }
    public override string Action1Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Cans;
        ItemName[] types = {ItemName.PAINTCAN};
        inv.GetItemsByType(out Cans, types);
        inv.DeleteItem(Cans[0]);

        ColorsGiven ++;

        if (ColorsGiven == 3)
        {
            DialogueProg ++;
            branch = 0;
        }

        return "Ευχαριστώ καΐκτση, θα μπορέσω να προχορήσω τον πίνακά μου λίγο ακόμη.";
    }
    public override string Action2Done(NPC owner, Player pl)
    {

        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Blood;
        ItemName[] types = {ItemName.BLOOD_VIAL};
        inv.GetItemsByType(out Blood, types);
        inv.DeleteItem(Blood[0]);

        DialogueProg ++;
        branch = 1;

        return "Ενδιαφέρων, δεν περέμενα κάτι τέτοιο... είσαι σίγουρος οτι θες να το αποχοριστείς, είναι ένα αρκετά σπάνιο ανικείμενο στις ημέρες μας. Θα προσπαθήσω να το εκμετελευτό πλήρος, ευχαριστώ καΐκτση";
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"ColorsGiven", ColorsGiven},
            {"branch", branch}
        };
        return savedata;
    }
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
        ColorsGiven = (int)Data["ColorsGiven"];
        branch = (int)Data["branch"];
    }
}
