using Godot;
using System;

public class Tutorial : Control
{
    [Export]
    VideoStream WalkTutorial;
    [Export]
    string WalkTutorialText;
    [Export]
    VideoStream SelectTutorial;
    [Export]
    string SelectTutorialText;
    [Export]
    VideoStream InventoryTutorial;
    [Export]
    string InventoryTutorialText;

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
