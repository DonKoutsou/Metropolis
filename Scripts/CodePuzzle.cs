using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CodePuzzle : BasePuzzle
{
    int [] PuzzleSolution = new int[5];
    List<MemoryButton> Buttons = new List<MemoryButton>();
    List<StageLight> Lights = new List<StageLight>();
    List<Label> Numbers = new List<Label>();
    Timer T;

    bool UsingCont = false;
    int Focusedbutton = 0;
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
        foreach(Label b in GetNode<Control>("ViewportContainer/Viewport/CenterContainer/VBoxContainer").GetChildren())
        {
            Numbers.Add(b);
            b.Text = string.Empty;
        }
        ProduceSolution();
        ToggleClicking(true);
        T = new Timer(){
            WaitTime = 2,
            OneShot = true
        };
        T.Connect("timeout", this, "Reset");

        AddChild(T);
        
        if (ControllerInput.IsUsingController())
        {
            UsingCont = true;
            Buttons[Focusedbutton].FlashStatic();
        }
    }
    private void Reset()
    {
        foreach(StageLight b in Lights)
        {
            b.Reset();
        }

        foreach(Label b in Numbers)
        {
            b.Text = string.Empty;
        }

        ClickedSequence.Clear();

        ToggleClicking(true);

        CorrectNums = 0;
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionJustPressed("ToggleZoom"))
        {
            Buttons[Focusedbutton].Flash();
            ButtonClicked(Buttons[Focusedbutton]);
        }
        else if (Input.IsActionJustPressed("ui_up"))
        {
            int newbutton = Focusedbutton - 3;
            if (newbutton >= 0)
            {
                Buttons[Focusedbutton].Reset();
                Focusedbutton = newbutton;
                Buttons[Focusedbutton].FlashStatic();
            }
        }
        else if (Input.IsActionJustPressed("ui_down"))
        {
            int newbutton = Focusedbutton + 3;
            if (newbutton <= 8)
            {
                Buttons[Focusedbutton].Reset();
                Focusedbutton = newbutton;
                Buttons[Focusedbutton].FlashStatic();
            }
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            int newbutton = Focusedbutton - 1;
            if (newbutton >= 0)
            {
                Buttons[Focusedbutton].Reset();
                Focusedbutton = newbutton;
                Buttons[Focusedbutton].FlashStatic();
            }
        }
        else if (Input.IsActionJustPressed("ui_right"))
        {
            int newbutton = Focusedbutton + 1;
            if (newbutton <= 8)
            {
                Buttons[Focusedbutton].Reset();
                Focusedbutton = newbutton;
                Buttons[Focusedbutton].FlashStatic();
            }
        }
    }
    private void ProduceSolution()
    {
        Random r = new Random();
        for (int i = 0; i < 5; i++)
        {
            int d = r.Next(9);
            while (PuzzleSolution.Contains(d))
                d = r.Next(9);
            PuzzleSolution[i] = d;
        }
    }
    public void ToggleClicking(bool t)
    {
        foreach(MemoryButton b in Buttons)
        {
            b.Enabled = t;
        }
    }
    List<int> ClickedSequence = new List<int>();
    int CorrectNums = 0;
    private void ButtonClicked(MemoryButton ButtonNum)
    {
        if (UsingCont)
        {
            if (Buttons[Focusedbutton] != ButtonNum)
            {
                Buttons[Focusedbutton].Reset();
                Focusedbutton = ButtonNum.GetButtonNumber();
            }
        }
        
        int bnum = ButtonNum.GetButtonNumber();
        float newpitch = 1 + (((float)bnum - 5) / 50);
        GetNode<AudioStreamPlayer>("ButtonSound").PitchScale = newpitch;
        GetNode<AudioStreamPlayer>("ButtonSound").Play();
        if (ClickedSequence.Count < 5)
        {
            int n = ClickedSequence.Count;

            Numbers[n].Text = (bnum + 1).ToString();

            if (PuzzleSolution.Contains(bnum))
            {
                if (PuzzleSolution[n] == bnum)
                {
                    Lights[n].StagePassed();
                    CorrectNums ++;
                }
                else
                {
                    Lights[n].StageSemiPass();
                }
            }
            ClickedSequence.Add(bnum);
            if (ClickedSequence.Count == 5)
            {
                Ended();
            }
        }
    }
    private void Ended()
    {
        if (CorrectNums == 5)
            FinishGame();
        else
            T.Start();

        ToggleClicking(false);
    }
    private void FinishGame()
    {
        foreach(MemoryButton b in Buttons)
        {
            b.FlashStatic();
        }
        Finished(true);
    }
}