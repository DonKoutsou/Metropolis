using Godot;
using System;
using System.Drawing;

public class TalkText : Label3D
{
    static TalkText instance;
    static Timer TalkTimer;
    Character Talking;
    public void Talk(string diag, Character Talker)
    {
        Text = diag;
        Talking = Talker;
        TalkTimer.Start();
        Show();
        SetProcess(true);
    }
    public void TurnOff()
    {
        SetProcess(false);
        Hide();
    }
    public static bool IsTalking()
    {
        return TalkTimer.TimeLeft > 0;
    }
    public override void _Process(float delta)
    {
        Vector3 plpos = Talking.GlobalTransform.origin;
        GlobalTranslation = new Vector3(plpos.x, plpos.y + 6, plpos.z);
    }
    public static TalkText GetInst()
    {
        return instance;
    }
    public override void _Ready()
    {
        base._Ready();
        TalkTimer = GetNode<Timer>("UpdateTimer");
        SetProcess(false);
        Hide();
        instance = this;
    }
    
}
