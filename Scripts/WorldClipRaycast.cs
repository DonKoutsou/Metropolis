using Godot;
using System;

public class WorldClipRaycast : RayCast
{
    static bool enabled = false;
    Player pl;
    public override void _Ready()
    {
        pl = (Player)GetParent();
    }
    public static void EnableWorldClipRaycast()
    {
        enabled = true;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!enabled)
            return;
        ForceRaycastUpdate();
        if (IsColliding())
        {
            Vector3 collisionpoint = GetCollisionPoint();
            collisionpoint.y += 2;
            if (pl.HasVecicle)
            {
                Vehicle veh = pl.currveh;
                ((Spatial)veh.GetParent()).GlobalTranslation = collisionpoint;
            }
            else
            {
                pl.GlobalTranslation = collisionpoint;
            }
            
        }
    }
}
