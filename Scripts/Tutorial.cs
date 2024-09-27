using Godot;
using System;
using System.Collections.Generic;

public class Tutorial : Control
{
    [Export]
    Dictionary<string, object> Tutorials = null;

    int stage = 0;

    private void VidFinished()
    {
        GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Play();
    }
    private void NextTurorial()
    {
        stage ++;
        int currentcheck = 0;
        foreach (KeyValuePair<string, object> tut in Tutorials)
        {
            if (stage != currentcheck)
            {
                currentcheck ++;
                continue;
            }
            if (tut.Value is VideoStream Vid)
            {
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Stream = Vid;
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Play();
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Show();
            GetNode<TextureRect>("Panel2/VBoxContainer/PanelContainer/TextureRect").Hide();
            }
            else if (tut.Value is StreamTexture Text)
            {
            GetNode<TextureRect>("Panel2/VBoxContainer/PanelContainer/TextureRect").Texture = Text;
            GetNode<TextureRect>("Panel2/VBoxContainer/PanelContainer/TextureRect").Show();
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Hide();
            }
                
            GetNode<Label>("Panel2/VBoxContainer/Label").Text = LocalisationHolder.GetString(tut.Key);
            return;
        }

        PlayerUI.OnMenuToggled(false);
        QueueFree();
            
    }
    public override void _Ready()
    {
        PlayerUI.OnMenuToggled(true);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Start");

        foreach (KeyValuePair<string, object> tut in Tutorials)
        {
            if (tut.Value is VideoStream Vid)
            {
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Stream = Vid;
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Play();
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Show();
            GetNode<TextureRect>("Panel2/VBoxContainer/PanelContainer/TextureRect").Hide();
            }
            else if (tut.Value is StreamTexture Text)
            {
            GetNode<TextureRect>("Panel2/VBoxContainer/PanelContainer/TextureRect").Texture = Text;
            GetNode<TextureRect>("Panel2/VBoxContainer/PanelContainer/TextureRect").Show();
            GetNode<VideoPlayer>("Panel2/VBoxContainer/PanelContainer/VideoPlayer").Hide();
            }
                
            GetNode<Label>("Panel2/VBoxContainer/Label").Text = LocalisationHolder.GetString(tut.Key);
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
