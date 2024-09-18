using Godot;
using System;
using System.Collections.Generic;

public class Tutorial : Control
{
    [Export]
    Dictionary<string, VideoStream> Tutorials = null;

    int stage = 0;

    private void VidFinished()
    {
        GetNode<VideoPlayer>("VideoPlayer").Play();
    }
    private void NextTurorial()
    {
        stage ++;
        int currentcheck = 0;
        foreach (KeyValuePair<string, VideoStream> tut in Tutorials)
        {
            if (stage != currentcheck)
            {
                currentcheck ++;
                continue;
            }
             if (tut.Value != null)
             {
                GetNode<VideoPlayer>("VideoPlayer").Stream = tut.Value;
                GetNode<VideoPlayer>("VideoPlayer").Play();
             }
                
            GetNode<Label>("VideoPlayer/Panel/Label").Text = LocalisationHolder.GetString(tut.Key);
            return;
        }

        PlayerUI.OnMenuToggled(false);
        QueueFree();
            
    }
    public override void _Ready()
    {
        PlayerUI.OnMenuToggled(true);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Start");

        foreach (KeyValuePair<string, VideoStream> tut in Tutorials)
        {
            if (tut.Value != null)
            {
                GetNode<VideoPlayer>("VideoPlayer").Stream = tut.Value;
                GetNode<VideoPlayer>("VideoPlayer").Play();
            }
                
            GetNode<Label>("VideoPlayer/Panel/Label").Text = LocalisationHolder.GetString(tut.Key);
            break;
        }
        //GetNode<VideoPlayer>("VideoPlayer").Stream = WalkTutorial;
        //GetNode<Label>("VideoPlayer/Panel/Label").Text = LocalisationHolder.GetString(WalkTutorialText);
        
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
