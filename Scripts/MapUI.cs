using Godot;
using System;

public class MapUI : Control
{
    //RichTextLabel CoordText;
    Label XCordText;
    Label YCordText;
    static bool IsMouseInMapBool = false;

    public void UpdateCoordinates()
    {
        Vector2 tile = WorldMap.GetInstance().GetCurrentTile();
        //CoordText.BbcodeText = "[center]X:" + tile.x + " / Y:" + tile.y;
        XCordText.Text = tile.x.ToString();
        YCordText.Text = tile.y.ToString();
    }
    public static bool IsMouseInMap()
    {
        return IsMouseInMapBool;
    }       
    private void OnMouseEntered()
    {
        IsMouseInMapBool = true;
    }
    private void OnMouseLeft()
    {
        IsMouseInMapBool = false;
    }
    public void OnMapOpened()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("static");
        GetNode<Panel>("MagGridPanel").GetNode<Control>("PlayerIconPivot").GetNode<AnimationPlayer>("PlayerIconAnim").Play("Blinking");
    }
    public void OnMapClosed()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
        GetNode<Panel>("MagGridPanel").GetNode<Control>("PlayerIconPivot").GetNode<AnimationPlayer>("PlayerIconAnim").Stop();
    }
    public override void _Ready()
    {
        base._Ready();
        //CoordText = GetNode<Panel>("Panel4").GetNode<RichTextLabel>("CoordText");
        XCordText = GetNode<Panel>("Panel4").GetNode<Panel>("Panel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Label>("XCordNumb");
        YCordText = GetNode<Panel>("Panel4").GetNode<Panel>("Panel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Label>("YCordNumb");
    }
}
