using Godot;
using System;

public class Explosive : Item
{
    [Signal]
    public delegate void OnExploded();
    public void StartExplosive()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("LightFlicker");
    }
    private void Exploded(string Name)
    {
        EmitSignal("OnExploded");
    }
    private void LightFlickerEnded(string Name)
    {
        GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("Exp");
    }
}
