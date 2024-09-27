using Godot;
using System;
using System.Collections.Generic;

public class Poseidon : Spatial
{
    MeshInstance SeaChild;
    static Poseidon instance;

    AnimationPlayer anim;

    ShaderMaterial mat;
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
        
        //if (GetParent() is WorldMap map)
           //map.Connect("OnTransitionEventHandler", this, "SwitchPlaces");
        
        mat = (ShaderMaterial)SeaChild.GetActiveMaterial(0);
        if (mat == null)
            SetProcess(false);
    }
    public void SwitchPlaces(Island to)
    {
        //GlobalTranslation = new Vector3(to.GlobalTranslation.x, GlobalTranslation.y, to.GlobalTranslation.z);
        ((WorldMap)GetParent()).SyncSeas();
    }
    float gesnervalue = 0.0f;
    bool Enabling = false;
    private void UpdateRotation()
    {
        float dir = CustomEnviroment.GetWindDirection();
        if (gesnervalue <= 0)
        {
            mat.SetShaderParam("TextureRot", -180 - dir);

            Enabling = true;
        }
        if (gesnervalue >= 1)
        {
            mat.SetShaderParam("TextureRot2", -180 - dir);

            Enabling = false;
            
        }
        if (Enabling)
        {
            gesnervalue += 0.04f;
        }
        else
        {
            gesnervalue -= 0.04f;
        }
        mat.SetShaderParam("gerstner_value", gesnervalue);

    }
    float d = 0.05f;
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        d -= delta;
		if (d <= 0)
		{
			d = 0.05f;
            UpdateRotation();
            float str = CustomEnviroment.GetRainStr();

            mat.SetShaderParam("RainInt", Mathf.Lerp(0.0f, 1.0f, str / 100));
            mat.SetShaderParam("IsRaining", str > 0.1f);
        }
        Player pl = Player.GetInstance();
        if (pl != null && Godot.Object.IsInstanceValid(pl))
        {
            GlobalTranslation = new Vector3(pl.GlobalTranslation.x, GlobalTranslation.y, pl.GlobalTranslation.z);
        }
        if (GetTree().Paused)
            mat.SetShaderParam("ProgressTime", false);
        else
            mat.SetShaderParam("ProgressTime", true);
    }
}
