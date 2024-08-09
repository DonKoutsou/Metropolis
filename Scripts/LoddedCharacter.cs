using Godot;
using System;

public class LoddedCharacter : Skeleton
{
    int currentlod = 0;

    Spatial headrotp;
    Spatial headrotp2;

    public override void _Ready()
    {
        base._Ready();
        headrotp =  GetNode<Spatial>("BoneAttachment/HeadRot");
        headrotp2 =  GetNode<Spatial>("HeadRot2Pivot/HeadRot2");

        SetProcess(false);
    }
    public override void _EnterTree()
    {
        AddToGroup("LODDEDCHAR");
        //SetBoneRest(FindBone("mixamorig_Head") ,GetNode<Spatial>("BoneAttachment/HeadRot").Transform);
        
    }
    public void HeadLookAt(Vector3 pos)
    {
        headrotp2.LookAt(pos, Vector3.Up);
        Vector3 rot = headrotp2.Rotation;
        Vector3 newrot = new Vector3(-Mathf.Clamp(rot.x, -1, 1), Mathf.Clamp(rot.y, -1, 1), -Mathf.Clamp(rot.z, -1, 1));
        var tw = CreateTween();
        tw.TweenProperty(headrotp, "rotation", newrot, 0.25f);
        //headrotp.Rotation = newrot;
        SetProcess(true);
    }
    public void ResetHead()
    {
        
        //headrotp.Rotation = new Vector3(0,0,0);

        var tw = CreateTween();
        tw.TweenProperty(headrotp, "rotation", new Vector3(0,0,0), 0.25f);

        SetProcess(true);
        //SetBoneRest(FindBone("mixamorig_Head") ,headrotp.Transform);
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        RemoveFromGroup("LODDEDCHAR");
    }
    public int GetCurrentLOD()
    {
        return currentlod;
    }
    public void SwitchLod(int LOD)
    {
        switch (LOD)
        {
            case 0:
            {
                if (currentlod == 0)
                    return;
                Visible = true;
                GetNode<MeshInstance>("Body").Visible = true;
                GetNode<MeshInstance>("Body_LOD").Visible = false;
                GetNode<MeshInstance>("eyes001").Visible = true;
                GetNode<MeshInstance>("Face").Visible = true;
                GetNode<MeshInstance>("Face_LOD").Visible = false;
                GetNode<MeshInstance>("LegsUpper").Visible = true;
                GetNode<MeshInstance>("LegsUpper_LOD").Visible = false;



                GetNode<MeshInstance>("Arm_L").Visible = GetNode<MeshInstance>("Arm_L_LOD").Visible;
                GetNode<MeshInstance>("Arm_L_LOD").Visible = false;
                GetNode<MeshInstance>("Arm_R").Visible = GetNode<MeshInstance>("Arm_R_LOD").Visible;
                GetNode<MeshInstance>("Arm_R_LOD").Visible = false;


                GetNode<MeshInstance>("Leg_L").Visible = GetNode<MeshInstance>("Leg_L_LOD").Visible;
                GetNode<MeshInstance>("Leg_L_LOD").Visible = false;
                GetNode<MeshInstance>("Leg_R").Visible = GetNode<MeshInstance>("Leg_R_LOD").Visible;
                GetNode<MeshInstance>("Leg_R_LOD").Visible = false;
                GetNode<MeshInstance>("Leg2_L").Visible = GetNode<MeshInstance>("Leg2_L_LOD").Visible;
                GetNode<MeshInstance>("Leg2_L_LOD").Visible = false;
                GetNode<MeshInstance>("Leg2_R").Visible = GetNode<MeshInstance>("Leg2_R_LOD").Visible;
                GetNode<MeshInstance>("Leg2_R_LOD").Visible = false;

                currentlod = 0;
                break;
            }
            case 1:
            {
                if (currentlod == 1)
                    return;
                
                Visible = true;
                GetNode<MeshInstance>("Body").Visible = false;
                GetNode<MeshInstance>("Body_LOD").Visible = true;
                GetNode<MeshInstance>("eyes001").Visible = false;
                GetNode<MeshInstance>("Face").Visible = false;
                GetNode<MeshInstance>("Face_LOD").Visible = true;
                GetNode<MeshInstance>("LegsUpper").Visible = false;
                GetNode<MeshInstance>("LegsUpper_LOD").Visible = true;

                GetNode<MeshInstance>("Arm_L_LOD").Visible = GetNode<MeshInstance>("Arm_L").Visible;
                GetNode<MeshInstance>("Arm_L").Visible = false;
                GetNode<MeshInstance>("Arm_R_LOD").Visible = GetNode<MeshInstance>("Arm_R").Visible;
                GetNode<MeshInstance>("Arm_R").Visible = false;

                GetNode<MeshInstance>("Leg_L_LOD").Visible = GetNode<MeshInstance>("Leg_L").Visible;
                GetNode<MeshInstance>("Leg_L").Visible = false;
                GetNode<MeshInstance>("Leg_R_LOD").Visible = GetNode<MeshInstance>("Leg_R").Visible;
                GetNode<MeshInstance>("Leg_R").Visible = false;
                GetNode<MeshInstance>("Leg2_L_LOD").Visible = GetNode<MeshInstance>("Leg2_L").Visible;
                GetNode<MeshInstance>("Leg2_L").Visible = false;
                GetNode<MeshInstance>("Leg2_R_LOD").Visible = GetNode<MeshInstance>("Leg2_R").Visible;
                GetNode<MeshInstance>("Leg2_R").Visible = false;
                
                currentlod = 1;
                break;
            }
            case 2:
            {
                Visible = false;
                break;
            }
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        int bone = FindBone("mixamorig_Head");
        if (GetBoneRest(bone) == headrotp.Transform)
            SetProcess(false);

        SetBoneRest(bone ,headrotp.Transform);
    }
}
