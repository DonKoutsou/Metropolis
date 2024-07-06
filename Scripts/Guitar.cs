using Godot;
using System;

public class Guitar : Instrument
{
    public override void HighLightObject(bool toggle)
    {
        ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable",  toggle);
		((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable",  toggle);
    }
}
