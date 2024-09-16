using Godot;
using System;

public class LanguageSelect : Control
{
    [Export]
    string GameScene = null;

    Language L;
    public override void _Ready()
    {
        OS.WindowSize = new Vector2(320, 110);
        //GetTree().Root.Size = new Vector2(320, 110);
        //ProjectSettings.SetSetting("display/window/size/width", 320);
        //ProjectSettings.SetSetting("display/window/size/height", 110);
        var actual_size = OS.GetRealWindowSize();
        var centered = new Vector2(OS.GetScreenSize().x / 2 - actual_size.x / 2, OS.GetScreenSize().y / 2 - actual_size.y / 2);
        OS.WindowPosition = centered;
    }
    private void SelectGreek()
    {
        L = Language.GREEK;
        StartGame();
    }
    private void SelectEnglish()
    {
        L = Language.ENGLISH;
        StartGame();
    }
    private void StartGame()
    {
        WorldRoot root = ResourceLoader.Load<PackedScene>(GameScene).Instance<WorldRoot>();
        LocalisationHolder lang = GetNode<LocalisationHolder>("LocalisationHolder");
        RemoveChild(lang);
        root.AddChild(lang);
        GetTree().Root.AddChild(root);
        lang.CallDeferred("Initialise", L);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
