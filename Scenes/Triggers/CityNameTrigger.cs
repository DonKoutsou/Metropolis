using Godot;
using System;
using System.Collections.Generic;

public class CityNameTrigger : Spatial
{
    [Export]
    NodePath Collision = null;
    bool IsIn = false;

    float dist;

    public override void _Ready()
    {
        base._Ready();
        SphereShape sh = (SphereShape)((CollisionShape)GetNode(Collision)).Shape;
        dist = sh.Radius;
    }

    private void On_Player_Entered(object body)
    {
        if (body is Player)
        {
            if (IsIn)
                return;

            IsIn = true;
            
            Player pl = (Player)body;
            CityNameUI UI = (CityNameUI)PlayerUI.GetInstance().GetUI(PlayerUIType.CITYNAME);
            UI.ShowName(((Island)GetParent()).IslandSpecialName);
            //
        }
    }
    private void On_Player_Left(object body)
    {
        if (((Spatial)body).GlobalTranslation.DistanceTo(GlobalTranslation) > dist)
            IsIn = false;
    }
}



