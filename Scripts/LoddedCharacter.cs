using Godot;
using System;

public class LoddedCharacter : Skeleton
{
    int currentlod = 0;
    public override void _EnterTree()
    {
        AddToGroup("LODDEDCHAR");
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
                GetNode<MeshInstance>("BodyLOD").Visible = false;
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
                GetNode<MeshInstance>("BodyLOD").Visible = true;
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
