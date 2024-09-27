using Godot;
using System;
using System.Collections.Generic;
public class WorldParticleManager : Spatial
{
    List<Particles> WorldParticles = new List<Particles>();
    static Spatial WindAllignedParticles;
    static Spatial ThunderMesh;
    static AnimationPlayer ThunderAnim;
    static NodePath path;
    public override void _Ready()
    {
        WindAllignedParticles = GetNode<Spatial>("WindAllignedParticles");
        var children = WindAllignedParticles.GetChildren();
        foreach (Node Child in children)
        {
            if (Child is Particles)
                WorldParticles.Add((Particles)Child);
        }
        
        ThunderMesh = GetNode<Spatial>("ThunderMesh");
        ThunderAnim = GetNode<AnimationPlayer>("AnimationPlayer");
        path = GetPath();
    }
    public static void PlayThunder()
    {
        if (ThunderAnim.IsPlaying())
            return;
        ThunderMesh.Translation = new Vector3(RandomContainer.Next(-8000, 8000), 3000, RandomContainer.Next(-8000, 8000));
        ThunderAnim.Play("ThunderStutter");
        WorldSoundManager.PlaySound("Thunder");
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        //Godot.Vector3 org = GlobalTranslation;
        //WindAllignedParticles.GlobalTranslation = new Godot.Vector3 (org.x, 120, org.z);
        float winddir = CustomEnviroment.GetWindDirection();
        //float windstr = DayNight.GetWindStr();
        float rot = Mathf.Deg2Rad(-360 - winddir);
        //Vector3 f = Vector3.Forward.Rotated(new Vector3(0,1,0), rot);
        /*foreach (Particles particle in WorldParticles)
        {
            particle.SpeedScale = windstr * 0.03f;
            ParticlesMaterial mat = (ParticlesMaterial)particle.ProcessMaterial;
            mat.Direction = f;
            //particle.Lifetime = ((100 - windstr) * 0.5f) + 20;
        }*/
       
        
        WindAllignedParticles.GlobalRotation = new Godot.Vector3(0, rot, 0);
    }
    public static void ToggleExternalParts(bool t)
    {
        WindAllignedParticles.Visible = t;
    }
}
