using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Bouzouki : Instrument
{
	public override void HighLightObject(bool toggle)
    {
		  ((ShaderMaterial)GetNode<MeshInstance>("MeshInstance").MaterialOverlay).SetShaderParam("enable",  toggle);
    }

}

