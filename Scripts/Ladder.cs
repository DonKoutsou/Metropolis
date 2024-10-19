using Godot;
using System;

[Tool]
public class Ladder : Spatial
{
    [Export]
	Vector3 TopPossition = new Vector3(0, 0.5f, 6);
    [Export]
	Vector3 BottomPossition = new Vector3(0, 0.5f, 6);
    [Export]
    bool dothing = false;
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
            float dif = TopPossition.y - BottomPossition.y;

            MultiMeshInstance mm = GetNode<MultiMeshInstance>("MultiMeshInstance");
            Mesh ladmesh = mm.Multimesh.Mesh;
            float Height = ladmesh.GetAabb().Size.y;
            int instancec = (int)(dif/Height);
            mm.Multimesh.InstanceCount = instancec;

            Transform t = mm.Transform;
            for (int i = 0; i < instancec; i++)
            {
                Transform instancet = t;
                instancet.origin = new Vector3(0, Height * i, 0);
                mm.Multimesh.SetInstanceTransform(i, instancet);
            }
            Vector3 ext = ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents;
            ext.y = dif / 2;
            ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents = ext;

            GetNode<CollisionShape>("CollisionShape").Translation = new Vector3(0, dif/2, 0);
        }
    }
    public override void _Process(float delta)
    {
		#if DEBUG
		if (Engine.EditorHint)
		{
			GetNode<Position3D>("Point1").Translation = TopPossition;
            GetNode<Position3D>("Point2").Translation = BottomPossition;

            if (!dothing)
                return;

            float dif = TopPossition.y - BottomPossition.y;

            MultiMeshInstance mm = GetNode<MultiMeshInstance>("MultiMeshInstance");
            Mesh ladmesh = mm.Multimesh.Mesh;
            float Height = ladmesh.GetAabb().Size.y;
            int instancec = (int)(dif/Height);
            mm.Multimesh.InstanceCount = instancec;

            Transform t = mm.Transform;
            for (int i = 0; i < instancec; i++)
            {
                Transform instancet = t;
                instancet.origin = new Vector3(0, Height * i, 0);
                mm.Multimesh.SetInstanceTransform(i, instancet);
            }
            Vector3 ext = ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents;
            ext.y = dif / 2;
            ((BoxShape)GetNode<CollisionShape>("CollisionShape").Shape).Extents = ext;

            GetNode<CollisionShape>("CollisionShape").Translation = new Vector3(0, dif/2, 0);
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
            GetNode<MultiMeshInstance>("MultiMeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MultiMeshInstance>("MultiMeshInstance").MaterialOverlay = null;
    }
    public void DoAction(Player pl)
	{
        TraverseLadder(pl);
    }
}
