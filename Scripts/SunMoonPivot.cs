using Godot;
using System;

public class SunMoonPivot : Spatial
{
    MeshInstance Sun;
    MeshInstance Moon;
    public override void _Ready()
    {
        Sun = GetNode<MeshInstance>("Sun");
        Moon = GetNode<MeshInstance>("Moon");
    }
    public void SetSunColor(Color col)
    {
        ((SpatialMaterial)Sun.GetActiveMaterial(0)).AlbedoColor = col;
    }
    public void SetMoonColor(Color col)
    {
        ((SpatialMaterial)Moon.GetActiveMaterial(0)).AlbedoColor = col;
    }

}
