using Godot;
using System;
using System.Resources;

public class PuzzleManager : Control
{
    [Export]
    string MemoryPuzzle = null;
    [Export]
    string CodePuzzle = null;

    BasePuzzle RunningPuzzle = null;

    public void StartPuzzle(PuzzleTypes type)
    {
        switch (type)
        {
            case PuzzleTypes.MEMORY:
            {
                RunningPuzzle = ResourceLoader.Load<PackedScene>(MemoryPuzzle).Instance<BasePuzzle>();
                break;
            }
            case PuzzleTypes.CODE:
            {
                RunningPuzzle = ResourceLoader.Load<PackedScene>(CodePuzzle).Instance<BasePuzzle>();
                break;
            }
        }
        GetNode<Viewport>("ViewportContainer/PuzzleViewport").AddChild(RunningPuzzle);
        RunningPuzzle.Connect("OnPuzzleFinished", this, "PuzzleFinished");
    }
    private void PuzzleFinished(bool resault)
    {
        RunningPuzzle.QueueFree();
        GD.Print("Puzzle resault : " + resault);
    }
}

public enum PuzzleTypes
{
    MEMORY,
    CODE
}