using Godot;
using System;

public class Tutorial : Control
{
    [Export]
    VideoStream WalkTutorial = null;
    [Export]
    string WalkTutorialText = null;
    [Export]
    VideoStream SelectTutorial = null;
    [Export]
    string SelectTutorialText = null;
    [Export]
    VideoStream InventoryTutorial = null;
    [Export]
    string InventoryTutorialText = null;

    int stage = 0;
    private void VidFinished()
    {
        
        GetNode<VideoPlayer>("VideoPlayer").Play();
    }
    private void NextTurorial()
    {
        stage ++;
        if (stage == 1)
        {
            GetNode<VideoPlayer>("VideoPlayer").Stream = SelectTutorial;
            GetNode<Label>("VideoPlayer/Panel/Label").Text = SelectTutorialText;
            GetNode<VideoPlayer>("VideoPlayer").Play();
        }
        else if (stage == 2)
        {
            GetNode<VideoPlayer>("VideoPlayer").Stream = InventoryTutorial;
            GetNode<Label>("VideoPlayer/Panel/Label").Text = InventoryTutorialText;
            GetNode<VideoPlayer>("VideoPlayer").Play();
        }
        else
            QueueFree();
    }
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Start");
        GetNode<VideoPlayer>("VideoPlayer").Stream = WalkTutorial;
        GetNode<Label>("VideoPlayer/Panel/Label").Text = WalkTutorialText;
        GetNode<VideoPlayer>("VideoPlayer").Play();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
