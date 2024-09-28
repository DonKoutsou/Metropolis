using Godot;
using System;
using System.Collections.Generic;

public class AchievementManager : Control
{
    [Export]
    PackedScene AchievementNotifScene;
    [Export]
    Resource[] Achievements = null;
    List<string> UnlockedAchievements = new List<string>();
    public void UnlockAchievement(string Ach, int times, bool Shownotif = false)
    {
        if (UnlockedAchievements.Contains(Ach))
            return;
        foreach (Resource Achiev in Achievements)
        {
            if ((string)Achiev.Get("Name") == Ach)
            {
                if ((int)Achiev.Get("Times") <= times)
                {
                    UnlockedAchievements.Add(Ach);
                    if (Shownotif)
                    {
                        AchievementNotification newnotif = AchievementNotifScene.Instance<AchievementNotification>();
                        newnotif.AchievementName = Ach;
                        newnotif.Icon = (StreamTexture)Achiev.Get("Icon");
                        GetNode<VBoxContainer>("VBoxContainer").AddChild(newnotif);
                    }
                }
            }
        }
    }
    public void ClearAchievements()
    {
        UnlockedAchievements.Clear();
    }
}
