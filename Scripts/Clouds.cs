using Godot;
using System;

public class Clouds : Spatial
{
    float d = 0.5f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;
		if (d > 0)
            return;
        d = 0.5f;
        Vector3 org = ((Spatial)GetParent()).GlobalTranslation;
        GlobalTranslation = new Vector3 (org.x, 2100, org.z);
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        GetNode<Particles>("Clouds3").Explosiveness = 1;
        CallDeferred("ReduceExpl");
    }
    public void ReduceExpl()
    {
        GetNode<Particles>("Clouds3").Explosiveness = 0;
    }
}
