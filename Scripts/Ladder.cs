using Godot;
using System;

[Tool]
public class Ladder : Spatial
{
    [Export]
	Vector3 TopPossition = new Vector3(0, 0.5f, 6);
    [Export]
	Vector3 BottomPossition = new Vector3(0, 0.5f, 6);
    Position3D BottomPos;
    Position3D TopPos;
    public override void _Ready()
    {
        if (!Engine.EditorHint)
		{
            BottomPos = GetNode<Position3D>("Point1");
            TopPos = GetNode<Position3D>("Point2");
            TopPos.Translation = TopPossition;
            BottomPos.Translation = BottomPossition;
            SetProcess(false);
        }
    }
    public override void _Process(float delta)
    {
		#if DEBUG
		if (Engine.EditorHint)
		{
			GetNode<Position3D>("Point1").Translation = TopPossition;
            GetNode<Position3D>("Point2").Translation = BottomPossition;
			return;
		}
		#endif
		base._Process(delta);
        
    }
    public string GetActionName(Player pl)
    {   
        string name = "Ανέβα";
        Vector3 PlayerPos = pl.GlobalTranslation;
        if (BottomPos.GlobalTranslation.DistanceTo(PlayerPos) > TopPos.GlobalTranslation.DistanceTo(PlayerPos))
        {
            name = "Κατέβα";
        }
        return name;
    }
    public bool ShowActionName(Player pl)
    {
        return true;
    }
    public string GetActionName2(Player pl)
    {
		return "Null.";
    }
    public string GetActionName3(Player pl)
    {
		return "Null.";
    }
    public bool ShowActionName2(Player pl)
    {
        return false;
    }
    public bool ShowActionName3(Player pl)
    {
        return false;
    }
    public string GetObjectDescription()
    {
        return "Σκάλα";
    }
    public Vector3 GetActionPos(Vector3 PlayerPos)
    {
        Position3D closest = BottomPos;
        if (BottomPos.GlobalTranslation.DistanceTo(PlayerPos) > TopPos.GlobalTranslation.DistanceTo(PlayerPos))
        {
            closest = TopPos;
        }
        return closest.GlobalTranslation;
    }
    public void TraverseLadder(Player pl)
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Connect("FadeOutFinished", this, "EndTraversal");
        CameraAnimation.FadeInOut(1);
        Position3D closest = BottomPos;
        Vector3 plpos = pl.GlobalTranslation;
        E_Animations anim = E_Animations.ClimbUp;
        Vector3 rot = BottomPos.GlobalRotation;
        //rot.y = -BottomPos.GlobalRotation.y;
        if (BottomPos.GlobalTranslation.DistanceTo(plpos) > TopPos.GlobalTranslation.DistanceTo(plpos))
        {
            closest = TopPos;
            anim = E_Animations.ClimbDown;
            rot = TopPos.GlobalRotation;
            //rot.y = -TopPos.GlobalRotation.y;
        }
        pl.Teleport(closest.GlobalTranslation, rot);
        //pl.GlobalRotation = rot;

        pl.Anims().PlayAnimation(anim);
        //pl.global
    }
    public void EndTraversal()
    {
        CameraAnimationPlayer CameraAnimation = CameraAnimationPlayer.GetInstance();
        CameraAnimation.Disconnect("FadeOutFinished", this, "EndTraversal");
        Position3D furthest = BottomPos;
        Player pl = Player.GetInstance();
        Vector3 plpos = pl.GlobalTranslation;
        if (BottomPos.GlobalTranslation.DistanceTo(plpos) < TopPos.GlobalTranslation.DistanceTo(plpos))
        {
            furthest = TopPos;
        }
        //pl.Rotation = Vector3.Zero;
        pl.Teleport(furthest.GlobalTranslation, Vector3.Zero);
    }
    public void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
            GetNode<MeshInstance>("Ladder").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("Ladder").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
        TraverseLadder(pl);
    }
}
