using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActionTracker : Node
{
    static Dictionary<string, int> DoneActions = new Dictionary<string, int>();
    public static void OnActionDone(string Action)
    {
        if (DoneActions.ContainsKey(Action))
        {
            DoneActions[Action] ++;
        }
        else
        {
            DoneActions.Add(Action, 1);
        }
        ResolveAction(Action, DoneActions[Action]);
    }
    public static Dictionary<string, int> GetActions()
    {
        return DoneActions;
    }
    static void ResolveAction(string Action, int amm)
    {
        AchievementManager.Instance.UnlockAchievement(Action, amm, true);
        switch  (Action)
        {
            case "Portal":
            {
                if (amm > 1)
                    return;
                ((TutorialManager)PlayerUI.GetUI(PlayerUIType.TUTORIAL)).PlayTutorial("Portal");
                break;
            }
            case "CodePuzzle":
            {
                if (amm > 1)
                    return;
                ((TutorialManager)PlayerUI.GetUI(PlayerUIType.TUTORIAL)).PlayTutorial("CodePuzzle");
                break;
            }
            case "LockPuzzle":
            {
                if (amm > 1)
                    return;
                ((TutorialManager)PlayerUI.GetUI(PlayerUIType.TUTORIAL)).PlayTutorial("LockPuzzle");
                break;
            }
        }
    }
    public static void LoadActions()
    {
        var ResourceLoaderSafe = ResourceLoader.Load("res://Scripts/safe_resource_loader.gd") as Script;
		Resource save = (Resource)ResourceLoaderSafe.Call("load", "user://SavedActions.tres");
        if (save == null)
			return;
        
        string[] Actions = (string[])save.Get("DoneActions");
        int[] ActionAmm = (int[])save.Get("DoneActionCount");

        for (int i = 0; i < Actions.Count(); i++)
        {
            DoneActions.Add(Actions[i], ActionAmm[i]);
            AchievementManager.Instance.UnlockAchievement(Actions[i], ActionAmm[i]);
        }
    }
    public static void SaveActions()
    {
		GDScript SaveGD = GD.Load<GDScript>("res://Scripts/Saved_Actions.gd");
		Godot.Object save = (Godot.Object)SaveGD.New();
        
        
        string[] Actions = new string[DoneActions.Count];
        int[] ActionAmmount = new int[DoneActions.Count];
        int i = 0;
        foreach (KeyValuePair<string, int> Action in DoneActions)
        {
            Actions[i] = Action.Key;
            ActionAmmount[i] = Action.Value;
            i ++;
        }
        Dictionary<string, object> data = new Dictionary<string, object>(){
            {"DoneActions", Actions},
            {"DoneActionCount", ActionAmmount}
        };
        save.Call("_SetData", data);

        ResourceSaver.Save("user://SavedActions.tres", (Resource)save);
    }
    public static void ClearActions()
    {
        Directory dir = new Directory();
		dir.Remove("user://SavedActions.tres");
        DoneActions.Clear();
    }
}