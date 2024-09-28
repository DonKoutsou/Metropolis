using Godot;
using System;
using System.Collections.Generic;

public class MemoryPuzzle : BasePuzzle
{
    int [] PuzzleSolution = new int[9];
    List<MemoryButton> Buttons = new List<MemoryButton>();
    List<StageLight> Lights = new List<StageLight>();
    int Stage = 0;
    public override void _Ready()
    {
        base._Ready();
        foreach(MemoryButton b in GetNode<Spatial>("Buttons").GetChildren())
        {
            Buttons.Add(b);
            b.Connect("OnPressed", this, "ButtonClicked");
        }
        foreach(StageLight b in GetNode<Spatial>("StageLights").GetChildren())
        {
            Lights.Add(b);
        }
        ProduceSolution();
        PlaySequence();
    }
    private void ProduceSolution()
    {
        Random r = new Random();
        for (int i = 0; i < 9; i++)
        {
            PuzzleSolution[i] = r.Next(9);
        }
    }
    public void ToggleClicking(bool t)
    {
        foreach(MemoryButton b in Buttons)
        {
            b.Enabled = t;
        }
    }
    public void PlaySequence()
    {
        ToggleClicking(false);
        SetProcess(true);
    }
    float d = 1;
    int ButtonSeq = 0;
    public override void _Process(float delta)
    {
        base._Process(delta);
        d -= delta;

        if (d > 0)
            return;
        
        d = 1;

        Buttons[PuzzleSolution[ButtonSeq]].Flash();

        ButtonSeq ++;

        if (ButtonSeq == GetNumbersForStage())
        {
            ButtonSeq = 0;
            SetProcess(false);
            ToggleClicking(true);
        }
    }
    List<int> ClickedSequence = new List<int>();
    private void ButtonClicked(MemoryButton ButtonNum)
    {
        if (PuzzleSolution[ClickedSequence.Count] == ButtonNum.GetButtonNumber())
        {
            ClickedSequence.Add(ButtonNum.GetButtonNumber());
            if (ClickedSequence.Count == GetNumbersForStage())
            {
                ClickedSequence.Clear();
                OnStagePassed();
                if (Stage == 3)
                    FinishGame();
                else
                    PlaySequence();
            }
        }
        else
        {
            ClickedSequence.Clear();
            PlaySequence();
            GD.Print("Clicked wrong button");
        }
    }
    private void FinishGame()
    {
        foreach(MemoryButton b in Buttons)
        {
            b.FlashStatic();
        }
        Finished(true);
    }
    private int GetNumbersForStage()
    {
        int num = 0;
        switch (Stage)
        {
            case 0:
            {
                num = 3;
                break;
            }
            case 1:
            {
                num = 6;
                break;
            }
            case 2:
            {
                num = 9;
                break;
            }
        }
        return num;
    }
    private void OnStagePassed()
    {
        GD.Print("Stage Passed");
        Stage++;
        Lights[Stage - 1].StagePassed();
    }
}
