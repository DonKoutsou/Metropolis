using Godot;
using System;

public class Explosive : Item
{
    [Signal]
    public delegate void OnExploded();
    public void StartExplosive()
    {
        GetNode<MeshInstance>("MeshInstance").Rotate(new Vector3(1,0,0) ,-Mathf.Pi/2);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("LightFlicker");
    }
    private void Exploded(string Name)
    {
        EmitSignal("OnExploded");
    }
    private void LightFlickerEnded(string Name)
    {
        GetNode<AnimationPlayer>("Explosion/AnimationPlayer").Play("Exp");
        GetNode<AudioStreamPlayer3D>("BombSound").Playing = true;
    }
    private void BombSoundFinished()
    {
        GetNode<AudioStreamPlayer3D>("BombSound").Playing = false;
    }
}
