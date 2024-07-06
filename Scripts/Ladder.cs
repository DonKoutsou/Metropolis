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
    public string GetActionName(Vector3 PlayerPos)
    {   
        string name = "Ανέβα";
        if (BottomPos.GlobalTranslation.DistanceTo(PlayerPos) > TopPos.GlobalTranslation.DistanceTo(PlayerPos))
        {
            name = "Κατέβα";
        }
        return name;
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
            rot.y = -TopPos.GlobalRotation.y;
        }
        pl.Teleport(closest.GlobalTranslation);
        pl.GlobalRotation = rot;

        pl.anim.PlayAnimation(anim);
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
        pl.Teleport(furthest.GlobalTranslation);
    }
    public void HighLightObject(bool toggle)
    {
		((ShaderMaterial)GetNode<MeshInstance>("Ladder").MaterialOverlay).SetShaderParam("enable",  toggle);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
