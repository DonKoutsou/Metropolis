using Godot;
using System;
using System.Collections.Generic;

public class Poseidon : Spatial
{
    MeshInstance SeaChild;
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
                SeaChild = (MeshInstance)s;
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
    float gesnervalue = 0.0f;
    bool Enabling = false;
    private void UpdateRotation()
    {
        float dir = DayNight.GetWindDirection();
        if (gesnervalue <= 0)
        {

            ShaderMaterial mat = (ShaderMaterial)SeaChild.GetActiveMaterial(0);
            mat.SetShaderParam("TextureRot", -180 - dir);

            Enabling = true;
        }
        if (gesnervalue >= 1)
        {

            ShaderMaterial mat = (ShaderMaterial)SeaChild.GetActiveMaterial(0);
            mat.SetShaderParam("TextureRot2", -180 - dir);

            Enabling = false;
            
        }
        if (Enabling)
        {
            gesnervalue += 0.02f;
        }
        else
        {
            gesnervalue -= 0.02f;
        }

        ShaderMaterial mat2 = (ShaderMaterial)SeaChild.GetActiveMaterial(0);
        mat2.SetShaderParam("gerstner_value", gesnervalue);

    }
    float d = 0.05f;
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        float str = DayNight.GetRainStr();

        ShaderMaterial mat = (ShaderMaterial)SeaChild.GetActiveMaterial(0);
        mat.SetShaderParam("RainInt", Mathf.Lerp(0.0f, 1.0f, str / 100));

        d -= delta;
		if (d <= 0)
		{
			d = 0.05f;
            UpdateRotation();
        }
        

    }
}
