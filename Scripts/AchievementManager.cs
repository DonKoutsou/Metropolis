using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class AchievementManager : Control
{
    [Export]
    PackedScene AchievementNotifScene = null;
    [Export]
    PackedScene AchievementIconScene = null;
    [Export]
    Resource[] Achievements = null;
    List<string> UnlockedAchievements = new List<string>();
    public static AchievementManager Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        Visible = false;

        GridContainer achcont = GetNode<GridContainer>("AchievementPanel/GridContainer");

        foreach (Resource Achiev in Achievements)
        {
            AchievementIcon achicon = AchievementIconScene.Instance<AchievementIcon>();

            achicon.GetNode<TextureRect>("Panel/TextureRect").Texture = (Texture)Achiev.Get("Icon");

            achcont.AddChild(achicon);

            achicon.Connect("OnSelected", this, "OnAchievementSelected");
        }
    }
    private void OnAchievementSelected(AchievementIcon ic)
    {
        GetNode<VBoxContainer>("AchievementPanel/Panel/VBoxContainer").Visible = true;
        int i = GetNode<GridContainer>("AchievementPanel/GridContainer").GetChildren().IndexOf(ic);
        string name = LocalisationHolder.GetString((string)Achievements[i].Get("Name"));
        if (UnlockedAchievements.Contains((string)Achievements[i].Get("Name")))
            GetNode<Label>("AchievementPanel/Panel/VBoxContainer/PanelContainer/AchievementStatus").Text = LocalisationHolder.GetString("AchUnlocked");
        else
            GetNode<Label>("AchievementPanel/Panel/VBoxContainer/PanelContainer/AchievementStatus").Text = LocalisationHolder.GetString("AchLocked");
        GetNode<Label>("AchievementPanel/Panel/VBoxContainer/PanelContainer2/Title").Text = name;
        GetNode<Label>("AchievementPanel/Panel/VBoxContainer/Desc").Text = LocalisationHolder.GetString((string)Achievements[i].Get("Description"));
    }
    public void ToggleAchievementsPanel()
    {
        Control achpan = GetNode<Control>("AchievementPanel");
        achpan.Visible = !achpan.Visible;
    }
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
                        newnotif.AchievementName = LocalisationHolder.GetString(Ach);
                        newnotif.Icon = (StreamTexture)Achiev.Get("Icon");
                        PlayerUI.GetUI(PlayerUIType.ACHIEVEMENT_NOTIFICATION).AddChild(newnotif);
                    }
                }
            }
        }
    }
    public void ClearAchievements()
    {
        UnlockedAchievements.Clear();
    }
    public void PlayerToggle(Player pl)
    {
    }
}