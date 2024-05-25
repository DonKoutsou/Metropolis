using Godot;
using System;

public class NPC : Character
{
    [Export]
    string AnimToPlay = null;

    [Export]
    bool spawnUncon = false;

    public override void _Ready()
    {
        base._Ready();
        if (AnimToPlay != null)
            anim.Play(AnimToPlay);
        if (spawnUncon)
        {
            CurrentEnergy = 0;
        }
    }
}
