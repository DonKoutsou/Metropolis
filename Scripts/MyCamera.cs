using Godot;
using System;
using System.Collections.Generic;

public class MyCamera : Camera
{
    public static bool isclippingwithfloor;

    static List<RayCast> rays = new List<RayCast>();

    CameraMovePivot campiv;

    static Vector3 ClipDir;

    public static bool IsClipping(out Vector3 direction)
    {
        UpdateCasts();
        direction = ClipDir;
        return isclippingwithfloor;
    }
    static void UpdateCasts()
    {
        bool anycolliding = false;
        for (int i  = 0; i < 4; i++)
        {
            rays[i].ForceRaycastUpdate();
            if (rays[i].IsColliding())
            {
                ClipDir = rays[i].CastTo;
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
            campiv = CameraMovePivot.GetInstance();
        }
        UpdateCasts();
        while (isclippingwithfloor)
        {
            campiv.Translation = new Vector3(campiv.Translation.x, campiv.Translation.y + 0.1f, campiv.Translation.z);
            UpdateCasts();
        }
    }
    public override void _Ready()
    {
        base._Ready();
        rays.Insert(0, GetNode<RayCast>("RayCastDown"));
        rays.Insert(1, GetNode<RayCast>("RayCastBack"));
        rays.Insert(2, GetNode<RayCast>("RayCastUp"));
        rays.Insert(3, GetNode<RayCast>("RayCastFront"));
        rays.Insert(4, GetNode<RayCast>("RayCastLeft"));
        rays.Insert(5, GetNode<RayCast>("RayCastRight"));
    }


}
