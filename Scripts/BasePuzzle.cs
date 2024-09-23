using Godot;
using System;
using System.Collections.Generic;

public class BasePuzzle : Spatial
{
    [Signal]
    public delegate void OnPuzzleFinished(bool Resault);

    public void Finished(bool r)
    {
        EmitSignal("OnPuzzleFinished", r);
    }
}
