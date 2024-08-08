using Godot;
using System;
using System.Collections.Generic;

public class KanarisScript : BaseDialogueScript
{
    int BooksGiven = 0;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {
        string text = string.Empty;

        if (Talker.IsPlayerInstrument())
        {
            text = "Δώσε μου λίγο, τελειώνω.";
        }
        else
        {
            switch(DialogueProg)
            {
                case 0:
                {
                    text = ".";
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
                    
                    break;
                }
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
    public override string Action1Done()
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Tollboxes;
        inv.GetItemsByType(out Tollboxes, ItemName.TOOLBOX);
        inv.DeleteItem(Tollboxes[0]);

        BooksGiven ++;

        if (BooksGiven == 3)
        {
            DialogueProg ++;
            return "Ευχαριστώ καΐκτση, θα μπορέσω να προχορήσω τον πίνακά μου λίγο ακόμη.";
        }

        return "Ωραίος, θα γίνει δουλειά με αυτά. Φέρε λίγα ακόμη και όπου νάνε τελειώνουμε.";
    }
    public override string Action2Done()
    {

        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Blood;
        inv.GetItemsByType(out Blood, ItemName.BLOOD_VIAL);
        inv.DeleteItem(Blood[0]);

        DialogueProg ++;

        return "Ενδιαφέρων, δεν περέμενα κάτι τέτοιο... είσαι σίγουρος οτι θες να το αποχοριστείς, είναι ένα αρκετά σπάνιο ανικείμενο στις ημέρες μας. Θα προσπαθήσω να το εκμετελευτό πλήρος, ευχαριστώ καΐκτση";
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"BooksGiven", BooksGiven}
        };
        return savedata;
    }
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
        BooksGiven = (int)Data["BooksGiven"];
    }
}
