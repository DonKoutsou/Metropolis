using Godot;
using System;

public class LoddedCharacter : Skeleton
{
    int currentlod = 0;
    public override void _EnterTree()
    {
        AddToGroup("LODDEDCHAR");
        //SetBoneRest(FindBone("mixamorig_Head") ,GetNode<Spatial>("BoneAttachment/HeadRot").Transform);
    }
    public void HeadLookAt(Vector3 pos)
    {
        Spatial headrotp =  GetNode<Spatial>("BoneAttachment/HeadRot");
        Spatial headrotp2 =  GetNode<Spatial>("HeadRot2Pivot/HeadRot2");
        headrotp2.LookAt(pos, Vector3.Up);
        Vector3 rot = headrotp2.Rotation;
        Vector3 newrot = new Vector3(-Mathf.Clamp(rot.x, -1, 1), Mathf.Clamp(rot.y, -1, 1), -Mathf.Clamp(rot.z, -1, 1));
        headrotp.Rotation = newrot;
        SetBoneRest(FindBone("mixamorig_Head") ,headrotp.Transform);
    }
    public void ResetHead()
    {
        Spatial headrotp =  GetNode<Spatial>("BoneAttachment/HeadRot");
        
        headrotp.Rotation = new Vector3(0,0,0);

        SetBoneRest(FindBone("mixamorig_Head") ,headrotp.Transform);
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
    public void SwitchLod(bool LOD)
    {
        switch (LOD)
        {
            case false:
            {
                if (currentlod == 0)
                    return;
                GetNode<MeshInstance>("Body").Visible = true;
                GetNode<MeshInstance>("Body_LOD").Visible = false;
                GetNode<MeshInstance>("eyes").Visible = true;
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
            case true:
            {
                if (currentlod == 1)
                    return;
                
                GetNode<MeshInstance>("Body").Visible = false;
                GetNode<MeshInstance>("Body_LOD").Visible = true;
                GetNode<MeshInstance>("eyes").Visible = false;
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
        }
    }
}
