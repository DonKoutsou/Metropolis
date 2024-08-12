using Godot;
using System;

public class ItemNotification : PanelContainer
{
    public string ItName;
    public Texture Icon = null;
    public int ammount = 1;
    public NotifType type = NotifType.POSSETIVE;
    SceneTreeTween tw;
    public override void _Ready()
    {
        base._Ready();
        GetNode<TextureRect>("HBoxContainer/Control/TextureRect").Texture = Icon;
        if (ammount > 0)
        {   
            GetNode<Label>("HBoxContainer/Label").Text = "+" +  ammount.ToString();
            type = NotifType.POSSETIVE;
        }
        else
        {
            GetNode<Label>("HBoxContainer/Label").Text = ammount.ToString();
            type = NotifType.NEGATIVE;
        }
        Modulate = new Color(1,1,1,0);
        tw = CreateTween();
        tw.TweenProperty(this, "modulate", new Color(1,1,1,1), 2);
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
    public void AddtoAmmount()
    {
        if (type == NotifType.POSSETIVE)
        {
            ammount ++;
            GetNode<Label>("HBoxContainer/Label").Text = "+" + ammount.ToString();
        }
        else
        {
            ammount --;
            GetNode<Label>("HBoxContainer/Label").Text = ammount.ToString();
        }

        tw.Kill();
        Modulate = new Color(1,1,1,1);

        tw = CreateTween();
        tw.TweenProperty(this, "modulate", new Color(1,1,1,0), 2);
        tw.Connect("finished", this, "FadeoutFin");
    
    }
}
public enum NotifType
{
    POSSETIVE,
    NEGATIVE
}