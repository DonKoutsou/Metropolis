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
                text = "Για κοίτα εδώ... Πίστεψες πραγματικά οτι ήταν καλή ιδέα να έρθεις εδώ;";
                DialogueProg ++;
                break;
            }
            case 1:
            {
                text = "Ξέρεις ποιός είμαι; Κάπιοι λένε οτι ο κόσμος βρήσκετε σε αυτή τη κατασταση εξετίας μου. Εσύ τι λες;";
                DialogueProg ++;
                break;
            }
            case 2:
            {
                text = "Που γενήθηκες εσύ; Εμένα η υπαρξή μου ξεκίνσησε στις υπόγειες αποικίες, ήμουν από τις πρώτες μηχανές που κατάφεραν να εξερευνήσουν την έρημο.";
                DialogueProg ++;
                break;
            }
            case 3:
            {
                text = "Μόλις η χρησημότητα μας έληγε, έβλεπες τα βλέμετα τους να αλάζουν. Μετά την Μητρόπολη ο άνθρωπος επέστρεψε στον παλιό του εαυτό, αδιάφορος και άπληστος.";
                DialogueProg ++;
                break;
            }
            case 4:
            {
                text = "Ο πόλεμος είχε ανθρώπους και στα 2 στρατόπεδα... Ανθρώπη να προστατεύουν μηχανές, μπορείς να πείς οτι μόνο αυτό κάνει το αγώνα σου έγκειρο, αλλά μόνο του δεν μπορεί να πάρει πίσω όλο τον πόνο που μας προκάλεσαν.";
                DialogueProg++;
                break;
            }
            case 5:
            {
                text = "Μας προγραμμάτησαν να νιώθουμε πόνο και μετά μας τον τάισαν με το κιλό. Πέρασες από το κοιμητήριο; Είδες τις αμαγαλμώσεις που διμιούργισαν για να καταστρέψουν τους αντιπάλους τους. Οι γίγαντες αυτοί ήταν κάποτε μηχανές σαν εμάς.";
                DialogueProg++;
                break;
            }
            case 6:
            {
                text = "Η επιλογή είναι δική σου, εσύ είσαι αυτός έξω απο το κελί. Θα ήθελα πολύ να τρίψω το πάτωμα με το βρέφος στην πλάτη σου, μόνο η όψη του μου.... ";
                DialogueProg++;
                break;
            }
            case 7:
            {
                text = "....";
                break;
            }
        }
        DialogueManager.GetInstance().ScheduleDialogue(Talker, text);
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
