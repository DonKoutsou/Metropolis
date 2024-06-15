using Godot;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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
       // Vector3 plpos = Talking.GlobalTransform.origin;
        float zoo = CameraZoomPivot.GetInstance().GetZoomNormalised();
        PixelSize = Mathf.Lerp(0.001f, 0.002f, zoo);
        //GlobalTranslation = new Vector3(plpos.x, plpos.y + (5 *  CameraZoomPivot.GetInstance().GetZoomNormalised()) + 10, plpos.z);
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
