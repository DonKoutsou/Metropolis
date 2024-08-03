using Godot;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

public class TalkText : Label3D
{
    //static TalkText instance;
    Timer TalkTimer;
    //Character Talking;
    public void Talk(string diag)
    {
        Text = diag;
        TalkTimer.WaitTime = diag.Length / 8;
        TalkTimer.Start();
        Show();
        SetProcess(true);
    }
    public void TurnOff()
    {
        SetProcess(false);
        Hide();
    }
    public bool IsTalking()
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
    //public static TalkText GetInst()
    //{
    //    return instance;
    //}
    public override void _Ready()
    {
        base._Ready();
        TalkTimer = new Timer()
        {
            OneShot = true,
            Autostart = true,
        };
        TalkTimer.Connect("timeout", this, "TurnOff");
        AddChild(TalkTimer);
        //TalkTimer = GetNode<Timer>("UpdateTimer");
        SetProcess(false);
        Hide();
        //instance = this;
    }
    
}
