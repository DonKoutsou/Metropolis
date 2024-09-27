using Godot;
using System;
using System.Collections.Generic;

public class TutorialManager : Control
{
    [Export]
    Dictionary<string, string> TutorialList = new Dictionary<string, string>();

    public void PlayTutorial(string TutorialName)
    {
        PackedScene Tut = ResourceLoader.Load<PackedScene>(TutorialList[TutorialName]);
        AddChild(Tut.Instance());
    }
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
