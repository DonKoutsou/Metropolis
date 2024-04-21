using Godot;
using System;

public class MapUI : Control
{
    RichTextLabel CoordText;
    static bool IsMouseInMapBool = false;

    public void UpdateCoordinates()
    {
        Vector2 tile = WorldMap.GetInstance().GetCurrentTile();
        CoordText.BbcodeText = "[center]" + tile.x + " / " + tile.y;
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
    }
    public void OnMapClosed()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Stop();
    }
    public override void _Ready()
    {
        base._Ready();
        CoordText = GetNode<Panel>("Panel4").GetNode<RichTextLabel>("CoordText");
    }
}
