using Godot;
using System;

public class Guitar : Instrument
{
    public override void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0).NextPass).SetShaderParam("enable",  toggle);
		((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(1).NextPass).SetShaderParam("enable",  toggle);
    }
}
