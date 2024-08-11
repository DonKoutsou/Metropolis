using Godot;
using System;
using System.Collections.Generic;

public class BoatEngineerScript : BaseDialogueScript
{
    int ToolboxesGiven = 0;
    int branch = 0;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        string text = string.Empty;
        
        switch(DialogueProg)
        {
            case 0:
            {
                text = "Έχω να δώ καΐκι να μπένει σε αυτό το λιμάνι πολύ καιρό. Κάποτε το μέρος έσφυζε από ζωή. Τώρα εγώ και 2 σαραβαλιασμένα καΐκια μείναμε.";
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
        Talker.GetTalkText().Talk(text);
    }
    public override bool ShouldShowExtraAction()
    {
        return DialogueProg == 3 && Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.TOOLBOX);
    }
    public override bool ShouldShowExtraAction2()
    {
        return false;
    }
    public override string GetExtraActionText()
    {
        return "Δώσε εργαλειοθήκη.";
    }
    public override string GetExtraActionText2()
    {
        return "Δώσε σταγώνα αίματος.";
    }
    public override string Action1Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Tollboxes;
        ItemName[] types = {ItemName.TOOLBOX};
        inv.GetItemsByType(out Tollboxes, types);
        inv.DeleteItem(Tollboxes[0]);

        ToolboxesGiven ++;

        if (ToolboxesGiven == 3)
        {
            DialogueProg ++;
            branch = 0;
            return "Ευχαριστώ καΐκτση, θα μπορέσω να προχορήσω τον πίνακά μου λίγο ακόμη.";
        }

        return "Ωραίος, θα γίνει δουλειά με αυτά. Φέρε λίγα ακόμη και όπου νάνε τελειώνουμε.";
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
            {"ToolboxesGiven", ToolboxesGiven},
            {"branch", branch}
        };
        return savedata;
    }
}
