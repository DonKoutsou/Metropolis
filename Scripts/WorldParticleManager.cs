using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class WorldParticleManager : Spatial
{
    public static WorldParticleManager Instance { get; private set; }
    [Export]
    public int ThunderLight = 0;
    [Export]
    PackedScene ThunderScene = null;
    [Export]
    AudioStream[] ThunderVariations = null;
    List<Particles> WorldParticles = new List<Particles>();
    static Spatial WindAllignedParticles;
    static AnimationPlayer ThunderAnim;
    public override void _Ready()
    {
        Instance = this;
        WindAllignedParticles = GetNode<Spatial>("WindAllignedParticles");
        var children = WindAllignedParticles.GetChildren();
        foreach (Node Child in children)
        {
            if (Child is Particles)
                WorldParticles.Add((Particles)Child);
        }
        ThunderAnim = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void PlayThunder()
    {
        ThunderAnim.Play("ThunderStutter");
        
        Thunder t = ThunderScene.Instance<Thunder>();

        t.InputData(ThunderVariations[RandomContainer.Next(0, ThunderVariations.Count())]);
        AddChild(t);
        t.Translation = new Vector3(RandomContainer.Next(-8000, 8000), 3000, RandomContainer.Next(-8000, 8000));
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
