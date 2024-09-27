using Godot;
using System;

public class AchievementNotification : PanelContainer
{
    public string AchievementName;
    public Texture Icon = null;
    SceneTreeTween tw;
    public override void _Ready()
    {
        base._Ready();
        GetNode<TextureRect>("VBoxContainer/HBoxContainer/Control/TextureRect").Texture = Icon;
        GetNode<Label>("VBoxContainer/HBoxContainer/Label").Text = AchievementName;
        Modulate = new Color(1,1,1,0.99f);
        tw = CreateTween();
        tw.TweenProperty(this, "modulate", new Color(1,1,1,1), 3);
        tw.Connect("finished", this, "StartFadeout");
    }
    private void StartFadeout()
    {
        tw = CreateTween();
        tw.TweenProperty(this, "modulate", new Color(1,1,1,0), 2);
        tw.Connect("finished", this, "FadeoutFin");
    }
    private void FadeoutFin()
    {
        QueueFree();
    }
}