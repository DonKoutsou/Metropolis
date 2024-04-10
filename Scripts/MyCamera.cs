using Godot;
using System;
using System.Collections.Generic;

public class MyCamera : Camera
{
    public static bool isclippingwithfloor;

    static List<RayCast> rays = new List<RayCast>();

    CameraPanPivot campiv;

    static Vector3 ClipDir;

    public static bool IsClipping(out Vector3 direction)
    {
        direction = ClipDir;
        return isclippingwithfloor;
    }
    static void UpdateCasts()
    {
        bool anycolliding = false;
        for (int i  = 0; i < rays.Count; i++)
        {
            rays[i].ForceRaycastUpdate();
            if (rays[i].IsColliding())
            {
                ClipDir = rays[i].GetCollisionPoint();
                anycolliding = true;
            }
                
        }
        if (anycolliding)
            isclippingwithfloor = true;

        else
            isclippingwithfloor = false;

    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if (campiv == null)
        {
            campiv = CameraPanPivot.GetInstance();
        }
        UpdateCasts();
        if (isclippingwithfloor)
        {
            campiv.Rotation += new Vector3(-0.05f, 0,0);
        }
        

    }
    public override void _Ready()
    {
        base._Ready();
        rays.Insert(0, GetParent().GetNode<RayCast>("CameraTerainCast"));
        rays.Insert(1, GetParent().GetNode<RayCast>("CameraTerainCast2"));
        /*rays.Insert(0, GetNode<RayCast>("RayCastDown"));
        rays.Insert(1, GetNode<RayCast>("RayCastBack"));
        rays.Insert(2, GetNode<RayCast>("RayCastUp"));
        rays.Insert(3, GetNode<RayCast>("RayCastFront"));
        rays.Insert(4, GetNode<RayCast>("RayCastLeft"));
        rays.Insert(5, GetNode<RayCast>("RayCastRight"));*/
    }


}
