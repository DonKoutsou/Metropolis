using Godot;
using System;
using System.Collections.Generic;

public class Poseidon : Spatial
{
    List<MeshInstance> SeaChild = new List<MeshInstance>();
    static Poseidon instance;

    AnimationPlayer anim;
    public static Poseidon GetInstance()
    {
        return instance;
    }
    public float GetAnimStage()
    {
        return anim.CurrentAnimationPosition;
    }
    public override void _Ready()
    {
        instance = this;
        var children = GetChildren();
        foreach (Node s in children)
        {
            if (s is MeshInstance)
            {
                SeaChild.Add((MeshInstance)s);
            }
        }
        
        anim = GetNode<AnimationPlayer>("AnimationPlayer");

        anim.Play("Wave");
        WorldMap map = (WorldMap)GetParent();
        map.Connect("OnTransitionEventHandler", this, "SwitchPlaces");
    }
    public void SwitchPlaces(Island to)
    {
        GlobalTranslation = new Vector3(to.GlobalTranslation.x, GlobalTranslation.y, to.GlobalTranslation.z);
        ((WorldMap)GetParent()).SyncSeas();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        float str = DayNight.GetRainStr();
		float dir = DayNight.GetWindDirection();

        foreach(MeshInstance s in SeaChild)
        {
            ShaderMaterial mat = (ShaderMaterial)s.GetActiveMaterial(0);
            mat.SetShaderParam("RainInt", Mathf.Lerp(0.0f, 0.5f, str / 100));
            mat.SetShaderParam("TextureRot", -180 - dir);

        }
    }
}
