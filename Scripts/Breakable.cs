using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Breakable : StaticBody
{
    [Export(PropertyHint.Layers3dPhysics)]
    public uint BreakAbleLayer { get; set; }

    Explosive AtatchedEx;
    public override void _Ready()
    {
        base._Ready();
        SetProcess(false);
    }
    public void AtatchExplosive(Player pl, Explosive ex)
    {

        var spacestate = GetWorld().DirectSpaceState;
        Vector3 rayor = pl.GlobalTranslation;
        rayor.y += 4;
        Vector3 rayend = GlobalTranslation;

        var rayar = spacestate.IntersectRay(rayor, rayend, null, BreakAbleLayer);
        
        if (rayar.Count == 0)
            return;

        ex.GetParent().RemoveChild(ex);
        AddChild(ex);

        Vector3 AttatchPos = (Vector3)rayar["position"];

        ex.GlobalTranslation = AttatchPos;
        ex.LookAt(rayor, Vector3.Up);
        ex.StartExplosive();
        ex.Connect("OnExploded", this, "AtatchedExplosiveGoneBoom");

        AtatchedEx = ex;
    }
    public void AtatchedExplosiveGoneBoom()
    {
        AtatchedEx.GetNode<MeshInstance>("MeshInstance").Hide();    
        GetNode<MeshInstance>("MeshInstance").Hide();
        SetProcess(true);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (!AtatchedEx.GetNode<Particles>("Explosion/Particles2").Emitting)
        {
            QueueFree();
        }
    }
    public virtual void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable", toggle);
    }
}
