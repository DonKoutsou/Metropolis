using Godot;
using System;
using System.Collections.Generic;

public class PriestScript : BaseDialogueScript
{
    int BooksGiven = 0;
    public override void DoDialogue(NPC Talker, NPC TalkerColaborator = null)
    {   
        string text = string.Empty;
        
        switch(DialogueProg)
        {
            case 0:
            {
                text = "Καλοσόρισες στο κοιμητήριο καΐκτση. Ο ναός εδώ στήθηκε προς σεβασμό στις μηχανές που σκλαβόθηκαν και σκοτόθηκαν από την ανθρωοπότητα μέχρι και πριν την 2η πρτώση.";
                DialogueProg ++;
                break;
            }
            case 1:
            {
                text = "Μέσα στην ιστορία αυτού του πλανήτη η ανθρωπότητα άλλες φορές στηρήχτηκε στην δύναμή τον μηχανών και άλλες την εκμεταλεύτηκε.";
                DialogueProg ++;
                break;
            }
            case 2:
            {
                text = " Μετατρέποντάς τες σε όπλα κατά την θέληση τους, σκλαβόνοντας τες για κέρδος το συνεσθήματα που τους είχαν δοθεί κατά την κατασκευή τους άρχισαν να φένονται σαν κατάρα.";
                DialogueProg ++;
                break;
            }
            case 3:
            {
                text = "Αυτό το μέρος βρήσκετε εδώ για να μη ξεχνάμε. Άν και δεν έχει μίνει ούτε ένας άνθρωπος σε αυτόν τον πλανήτη.... πλήν του μωρού που κουβαλάς στην πλάτη σου.";
                DialogueProg ++;
                break;
            }
            case 4:
            {
                text = "Τι σκοπεύεις να το κάνεις; Το πάς στην μητρόπολη; Δεν μπορώ να σε σταματήσω αν αυτός ο χώρος δεν σου δίχνει το λάθος που κάνεις δεν θα μπορέσω εγώ...";
                DialogueProg ++;
                break;
            }
            case 5:
            {
                text = "Έχω ένα αίτημα... στο ταξίδι σου προς την Μητρόπολη θα περάσεις από τους φάρους. Θα ήθελα να μου φέρις ότι βιβλία βρείς εκεί. Οι φάροι ήταν οι μεγαλήτερες αποθήκες γνώσεις πρίν την 2η πτώση. Δυστηχώς στην κατάσταση μου δεν μπορώ να τους επισκεφθώ.";
                //DialogueProg ++;
                break;
            }
        }
        Talker.GetTalkText().Talk(text);
    }
    public override bool ShouldShowExtraAction()
    {
       return DialogueProg == 5 && Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.BOOK);
    }
    public override bool ShouldShowExtraAction2()
    {
        return false;
    }
    public override string GetExtraActionText()
    {
        return "Δώσε βιβλίο.";
    }
    public override string GetExtraActionText2()
    {
        return "Null";
    }
    public override string Action1Done(NPC owner, Player pl)
    {
        Inventory inv = Player.GetInstance().GetCharacterInventory();
        List<Item> Books;
        inv.GetItemsByType(out Books, ItemName.BOOK);
        inv.DeleteItem(Books[0]);

        BooksGiven ++;

        if (BooksGiven == 6)
        {
            DialogueProg ++;
        }
        
        return "Ευχαριστώ καΐκτση, θα μπορέσω να προχορήσω τον πίνακά μου λίγο ακόμη.";
    }
    public override string Action2Done(NPC owner, Player pl)
    {
        return "null";
    }
    public override Dictionary<string, object>GetSaveData()
    {
        Dictionary<string, object> savedata = new Dictionary<string, object>(){
            {"DialogueProg", DialogueProg},
            {"BooksGiven",BooksGiven},
        };
        return savedata;
    }
    public override void LoadSaveData(Dictionary<string, object> Data)
    {
        DialogueProg = (int)Data["DialogueProg"];
        BooksGiven = (int)Data["BooksGiven"];
    }
}
