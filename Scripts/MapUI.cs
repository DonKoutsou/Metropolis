using Godot;
using System;

public class MapUI : Control
{
    [Signal]
    public delegate void TileHovered();
    static bool IsMouseInMapBool = false;

    public bool IsOpen = false;

    MapGrid Grid;

    public override void _Ready()
    {
        base._Ready();
        Grid = GetNode<MapGrid>("MapGridPanel/MapGrid");
    }
    public MapGrid GetGrid()
    {
        return Grid;
    }
    public void ToggleMap(bool toggle)
    {
        if (IsOpen == toggle)
            return;

        
        if (toggle)
        {
            OnMapOpened();
        }
        else
        {
            OnMapClosed();
        }
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
        PlayerUI.OnMenuToggled(true);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("MapOpen");
        GetNode<Panel>("MapGridPanel").GetNode<Control>("PlayerIconPivot").GetNode<AnimationPlayer>("PlayerIconAnim").Play("Blinking");
        Show();
        IsOpen = true;
       
        
    }
    public void OnMapClosed()
    {
        PlayerUI.OnMenuToggled(false);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("MapClose");
        GetNode<Panel>("MapGridPanel").GetNode<Control>("PlayerIconPivot").GetNode<AnimationPlayer>("PlayerIconAnim").Stop();
        AudioServer.SetBusEffectEnabled(0,0, false);
        Grid.ToggleMap(false);
        GetNode<AudioStreamPlayer>("MapClose").Play();
    }
    private void OnAnimFinished(string anim)
    {
        if (anim == "MapOpen")
        {
            AudioServer.SetBusEffectEnabled(0,0, true);
            Grid.ToggleMap(true);
             GetNode<AudioStreamPlayer>("MapOpen").Play();
        }
        if (anim == "MapClose")
        {
            
            Hide();
            IsOpen = false;
        }
    }
    private void MapTileHovered()
    {
        EmitSignal("TileHovered");
    }
    //public override void _Ready()
    //{
    //    base._Ready();
        //grid = GetNode<MapGrid>("MapGridPanel/MapGridPivot/MapGrid");
        //CoordText = GetNode<Panel>("Panel4").GetNode<RichTextLabel>("CoordText");
        //XCordText = GetNode<Panel>("Panel4").GetNode<Panel>("Panel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Label>("XCordNumb");
        //YCordText = GetNode<Panel>("Panel4").GetNode<Panel>("Panel").GetNode<HBoxContainer>("HBoxContainer").GetNode<Label>("YCordNumb");
    //}
}
