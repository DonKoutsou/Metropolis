using Godot;
using System;
using System.Collections.Generic;

public class BasePuzzle : Spatial
{
    [Export]
    PuzzleTypes Type = PuzzleTypes.LOCK;
    [Signal]
    public delegate void OnPuzzleFinished(bool Resault);

    public override void _Ready()
    {
        PlayerUI.OnMenuToggled(true);
    }
    public PuzzleTypes GetPuzzleType()
    {
        return Type;
    }

    public void Finished(bool r)
    {
        PlayerUI.OnMenuToggled(false);
        EmitSignal("OnPuzzleFinished", r);
    }
}
