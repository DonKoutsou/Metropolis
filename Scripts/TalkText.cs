using Godot;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

public class TalkText : Label3D
{
    //static TalkText instance;
    Timer TalkTimer;
    //Character Talking;

    string TextToShow;

    int CharactersShowing = 0;

    bool Talking = false;

    bool CharPar = false;
    public void Talk(string diag)
    {
        if (CharPar)
        {
            Player.GetInstance().BeingTalkedTo = true;
            GetParent<NPC>().HeadLook(Player.GetInstance().GetHeadGlobalPos());
        }
        TextToShow = diag;
        Text = string.Empty;
        CharactersShowing = 0;
        Talking = true;
        Show();
        SetProcess(true);
    }
    public void TurnOff()
    {
        Talking = false;
        if (CharPar)
        {
            GetParent<NPC>().ResetLook();
            Player.GetInstance().BeingTalkedTo = false;
        }
        Hide();
    }
    public bool IsTalking()
    {
        return Talking;
    }
    float d = 0.06f;
    public override void _Process(float delta)
    {
        d -= delta;
        if (d > 0)
            return;
        d = 0.06f;
       // Vector3 plpos = Talking.GlobalTransform.origin;
        //float zoo = CameraZoomPivot.GetInstance().GetZoomNormalised();
        //PixelSize = Mathf.Lerp(0.001f, 0.002f, zoo);

        Text = TextToShow.Substr(0, CharactersShowing);

        CharactersShowing ++;

        if (CharactersShowing == TextToShow.Length + 1)
        {
            TalkTimer.Start();
            SetProcess(false);
        }
        //GlobalTranslation = new Vector3(plpos.x, plpos.y + (5 *  CameraZoomPivot.GetInstance().GetZoomNormalised()) + 10, plpos.z);
    }
    //public static TalkText GetInst()
    //{
    //    return instance;
    //}
    public override void _Ready()
    {
        base._Ready();

        if (GetParent() is NPC)
        {
            CharPar = true;
        }

        TalkTimer = new Timer()
        {
            OneShot = true,
            //Autostart = true,
        };
        TalkTimer.Connect("timeout", this, "TurnOff");
        TalkTimer.WaitTime = 2;
        AddChild(TalkTimer);
        //TalkTimer = GetNode<Timer>("UpdateTimer");
        SetProcess(false);
        Hide();
        //instance = this;
    }
    
}
