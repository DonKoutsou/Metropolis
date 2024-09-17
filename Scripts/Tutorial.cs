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
    [Export]
    VideoStream MapTutorial = null;
    [Export]
    string MapTutorialText = null;

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
            GetNode<Label>("VideoPlayer/Panel/Label").Text = LocalisationHolder.GetString(SelectTutorialText);
            GetNode<VideoPlayer>("VideoPlayer").Play();
        }
        else if (stage == 2)
        {
            GetNode<VideoPlayer>("VideoPlayer").Stream = InventoryTutorial;
            GetNode<Label>("VideoPlayer/Panel/Label").Text = LocalisationHolder.GetString(InventoryTutorialText);
            GetNode<VideoPlayer>("VideoPlayer").Play();
        }
        else
        {
            PlayerUI.OnMenuToggled(false);
            QueueFree();
        }
            
    }
    public override void _Ready()
    {
        PlayerUI.OnMenuToggled(true);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Start");
        GetNode<VideoPlayer>("VideoPlayer").Stream = WalkTutorial;
        GetNode<Label>("VideoPlayer/Panel/Label").Text = LocalisationHolder.GetString(WalkTutorialText);
        GetNode<VideoPlayer>("VideoPlayer").Play();
    }
    public void OnMapPickup()
    {
        if (MapTutorial != null)
        {
            GetNode<VideoPlayer>("VideoPlayer").Stream = MapTutorial;
            GetNode<VideoPlayer>("VideoPlayer").Play();
        }
        GetNode<Label>("VideoPlayer/Panel/Label").Text = MapTutorialText;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Restart");
        Show();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
