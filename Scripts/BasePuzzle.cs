using Godot;
using System;
using System.Collections.Generic;

public class BasePuzzle : Spatial
{
    [Export]
    PuzzleTypes Type = PuzzleTypes.LOCK;
    [Signal]
    public delegate void OnPuzzleFinished(bool Resault);

    public PuzzleTypes GetPuzzleType()
    {
        return Type;
    }

    public void Finished(bool r)
    {
        EmitSignal("OnPuzzleFinished", r);
    }
}
