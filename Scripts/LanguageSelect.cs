using Godot;
using System;

public class LanguageSelect : Control
{
    [Export]
    string GameScene = null;

    Language L;
    public override void _Ready()
    {
        var actual_size = OS.GetRealWindowSize();
        var centered = new Vector2(OS.GetScreenSize().x / 2 - actual_size.x / 2, OS.GetScreenSize().y / 2 - actual_size.y / 2);
        OS.WindowPosition = centered;
        if (ControllerInput.IsUsingController())
            SwitchedController(true);
        ControllerInput.GetInstance().Connect("OnControllerSwitched", this, "SwitchedController");
    }
    private void SwitchedController(bool Toggle)
    {
        if (Toggle)
            GetNode<Button>("Panel/HBoxContainer/EnglishButton").GrabFocus();
        else
        {
            GetFocusOwner().ReleaseFocus();
        }
    }
    private void SelectGreek()
    {
        L = Language.GREEK;
        PlayFadeOut();
    }
    private void SelectEnglish()
    {
        L = Language.ENGLISH;
        PlayFadeOut();
    }
    void PlayFadeOut()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }
    public void StartGame(string animname)
    {
        WorldRoot root = ResourceLoader.Load<PackedScene>(GameScene).Instance<WorldRoot>();
        GetTree().Root.AddChild(root);
        LocalisationHolder.Instance.Initialise(L);
        QueueFree();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
