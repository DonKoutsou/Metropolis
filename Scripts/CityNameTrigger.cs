using Godot;
using System;
using System.Collections.Generic;

[Tool]
public class CityNameTrigger : Spatial
{
    [Export]
    float Radius = 3000;
    [Export]
    NodePath Collision = null;
    bool IsIn = false;

    float dist;

    public override void _Ready()
    {
        if (!Engine.EditorHint)
        {
            base._Ready();
            SphereShape sh = (SphereShape)((CollisionShape)GetNode(Collision)).Shape;
            sh.Radius = Radius;
            dist = sh.Radius;
            GetNode<Spatial>("MeshInstance").QueueFree();
        }
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
    public override void _Process(float delta)
    {
        if (Engine.EditorHint)
        {
            SphereMesh sph = (SphereMesh)GetNode<MeshInstance>("MeshInstance").Mesh;
            sph.Radius = Radius;
            sph.Height = Radius * 2;
        }
    }
}



