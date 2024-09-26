using Godot;
using System;
using System.Resources;

public class PuzzleManager : Control
{
    [Export]
    string MemoryPuzzle = null;
    [Export]
    string CodePuzzle = null;
    [Export]
    string LockPuzzle = null;
    [Signal]
    public delegate void PuzzleResault(bool resault);

    BasePuzzle RunningPuzzle = null;

    /*public override void _Ready()
    {
        base._Ready();
        StartPuzzle(PuzzleTypes.LOCK);
    }*/
    public bool PuzzleRunning()
    {
        return RunningPuzzle != null;
    }
    public void StartPuzzle(PuzzleTypes type)
    {
        PlayerUI.OnMenuToggled(true);
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
            case PuzzleTypes.LOCK:
            {
                RunningPuzzle = ResourceLoader.Load<PackedScene>(LockPuzzle).Instance<BasePuzzle>();
                break;
            }
        }
        GetNode<Viewport>("ViewportContainer/PuzzleViewport").AddChild(RunningPuzzle);
        RunningPuzzle.Connect("OnPuzzleFinished", this, "PuzzleFinished");
    }
    
    private void PuzzleFinished(bool resault)
    {
        RunningPuzzle.QueueFree();
        RunningPuzzle = null;
        PlayerUI.OnMenuToggled(false);
        EmitSignal("PuzzleResault", resault);
        GD.Print("Puzzle resault : " + resault);
    }
}

public enum PuzzleTypes
{
    MEMORY,
    CODE,
    LOCK,
}