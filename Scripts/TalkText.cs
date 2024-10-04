using Godot;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

public class TalkText : Spatial
{
    //static TalkText instance;
    Timer TalkTimer;
    //Character Talking;

    string TextToShow;

    int CharactersShowing = 0;

    bool Talking = false;

    bool CharPar = false;

    Label TextL;

    Vector2 Vsize;
    AudioStreamPlayer Audio;

    bool DoingForcedDialogue = false;
    public void Talk(string diag, bool forced = false)
    {
        if (diag == string.Empty || diag == TextToShow)
        {
            return;
        }
        if (CharPar)
        {
            Player.GetInstance().BeingTalkedTo = true;
            GetParent<NPC>().HeadLook(Player.GetInstance().GetHeadGlobalPos());
        }
        TalkTimer.Stop();
        TextToShow = diag;
        //Text = string.Empty;
        CharactersShowing = 0;

        TextL.Text = string.Empty;
        Talking = true;


        TextL.Show();
        SetProcess(true);
        SetPhysicsProcess(true);

        Vsize = DViewport.GetInstance().Size;
        DoingForcedDialogue = forced;
    }
    public void TurnOff()
    {
        Talking = false;
        TextToShow = string.Empty;
        if (CharPar)
        {
            GetParent<NPC>().ResetLook();
            Player.GetInstance().BeingTalkedTo = false;
        }
        TextL.Hide();
        SetPhysicsProcess(false);
        DialogueManager.GetInstance().OnDialogueEnded(DoingForcedDialogue);
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
        Audio.PitchScale = RandomContainer.Next(85, 115) / 100;
        Audio.Play();

        TextL.Text = TextToShow.Substr(0, CharactersShowing);

        CharactersShowing ++;

        if (CharactersShowing == TextToShow.Length + 1)
        {
            TalkTimer.Start();
            SetProcess(false);
        }
        
        //GlobalTranslation = new Vector3(plpos.x, plpos.y + (5 *  CameraZoomPivot.GetInstance().GetZoomNormalised()) + 10, plpos.z);
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Vector3 pos = GlobalTranslation;
        Vector2 position2D = PlayerCamera.GetInstance().UnprojectPosition(pos);
        position2D = new Vector2(Mathf.Min(Vsize.x, Mathf.Max(0, position2D.x - (TextL.RectSize.x / 2))), Mathf.Min(Vsize.y, Mathf.Max(0, position2D.y - TextL.RectSize.y)));
        TextL.RectPosition = position2D;
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
        SetPhysicsProcess(false);
        

        TextL = GetNode<Label>("2DText");
        TextL.Hide();
        Audio =  GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        //instance = this;
    }
    
}
