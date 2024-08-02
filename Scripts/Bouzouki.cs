using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Bouzouki : Instrument
{
    public override void HighLightObject(bool toggle, Material OutlineMat)
    {
        if (toggle)
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = OutlineMat;
        else
            GetNode<MeshInstance>("MeshInstance").MaterialOverlay = null;
    }
}

