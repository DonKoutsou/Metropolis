using Godot;
using System;

public class VehicleBoostTrails : Spatial
{
    Spatial trail1;
    Spatial trail2;
    SpatialMaterial mat;
    SceneTreeTween t1;
    SceneTreeTween t2;
    SceneTreeTween t3;
    public void StartBoost()
    {
        if (t1 != null)
            t1.Kill();

        if (t2 != null)
            t2.Kill();

        if (t3 != null)
            t3.Kill();

        trail1.Set("lifespan", 0.5f);
        trail2.Set("lifespan", 0.5f);
        
        t1 = CreateTween();
        t1.TweenProperty(GetNode<Spatial>("MotionTrail"), "lifespan", 0, 40);
        t2 = CreateTween();
        t2.TweenProperty(GetNode<Spatial>("MotionTrail2"), "lifespan", 0, 40);

        GetNode<Particles>("BoostPart").Emitting = true;
        mat.AlbedoColor = new Color(1,1,1,1);

        t3 = CreateTween();
        t3.TweenProperty(mat, "albedo_color", new Color(1,1,1,0), 40);
        t3.Connect("finished", this, "MatTweenFin");
    }
    public override void _Ready()
    {
        base._Ready();
        trail1 = GetNode<Spatial>("MotionTrail");
        trail2 = GetNode<Spatial>("MotionTrail2");
        mat = (SpatialMaterial)GetNode<Particles>("BoostPart").MaterialOverride;
    }
    private void MatTweenFin()
    {
        GetNode<Particles>("BoostPart").Emitting = false;
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

}
