using Godot;
using System;

public class Pod : StaticBody
{
    bool Opened = false;
    [Export]
    bool Destroyed = false;

    public override void _EnterTree()
    {
        base._EnterTree();
        Player pl = Player.GetInstance();
        if (pl == null)
            return;

        if (pl.HasBaby)
        {
            AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");

            anim.Play("PodOpen");

            Opened = true;
        }
    }
    public bool IsOpen()
    {
        return Opened;
    }
    public bool IsDestroyed()
    {
        return Destroyed;
    }
    public void OpenPod()
    {
        Player pl = Player.GetInstance();
        if (pl.HasBaby)
        {
            AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");
            anim.Play("PodOpen");
            Opened = true;
            pl.GetTalkText().Talk("Δεν έχει κάτι άλλο μέσα...");
        }
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Connect("FadeOutFinished", this, "InitialChoice");
        CameraAnimation.FadeInOut(1);
    }
    public void InitialChoice()
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "InitialChoice");

        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/HUD/BabyInitialChoise.tscn");
        BabyInitialChoise chouse = scene.Instance<BabyInitialChoise>();
        chouse.Connect("BabyPicked", this, "HandleInitialChoiceResault");
        WorldRoot.GetInstance().GetNode<CanvasLayer>("CanvasLayer").AddChild(chouse);
    }
    public void OpenHatch()
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "OpenHatch");
        Player.GetInstance().GetTalkText().Talk("Κλείσε τα μάτια σου μικρό είχες μεγάλο ταξίδι, σε λίγο θα είσαι σπίτι...");
        //CameraAnimation.FadeOut(1);

        AnimationPlayer anim = GetNode<AnimationPlayer>("AnimationPlayer");

        anim.Play("PodOpen");

        Opened = true;

        Player.GetInstance().OnBabyGot();
    }
    public void Leave()
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "Leave");
        Player.GetInstance().GetTalkText().Talk("Καλή τύχη μικρό...");
    }
    public void HandleInitialChoiceResault(bool b)
    {
        if (b)
        {
            CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
            CameraAnimation.Connect("FadeOutFinished", this, "OpenHatch");
            CameraAnimation.FadeInOut(1);
            
            //OpenHatch();
        }
        else
        {
            CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
            CameraAnimation.Connect("FadeOutFinished", this, "Leave");
            CameraAnimation.FadeInOut(1);
        }
        DepartureSystem.GetInstance().OnChoiceMade(b); 
    }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {

        if (toggle)
        {
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
            GetNode<MeshInstance>("MeshInstance2").MaterialOverlay = OutlineMat;
        } 
        else
        {
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
            GetNode<MeshInstance>("MeshInstance2").MaterialOverlay = null;
        }
            
    }
    public void DoAction(Player pl)
	{
        OpenPod();
    }
    public string GetActionName(Player pl)
    {
        return "Άνοιξε";
    }
    public bool ShowActionName(Player pl)
    {
        return !Opened;
    }
    public string GetObjectDescription()
    {
        string desc;
        if (IsDestroyed())
            desc = "Ένα σκάφος διαφυγής, δυστυχώς δεν τα κατάφερε.";
        else if (IsOpen())
            desc = "Ένα σκάφος διαφυγής.";
        else
            desc = "Το σκάφος που επέζησε, μπορώ να το ανοίξω.";
        return desc;
    }
}
