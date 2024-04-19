using Godot;
using System;
using System.Collections.Generic;

public class MapGrid : GridContainer
{
    [Export]
    string TileScene = null;
    [Export]
    string XGridTileScene = null;
    [Export]
    string YGridTileScene = null;
    int times = 6561;

    PackedScene tilescene;

    List<ChildMapIleInfo> children = new List<ChildMapIleInfo>();
    Dictionary<Vector2, Control> MapIleList = new Dictionary<Vector2, Control>();

    GridContainer MapGridx;
    GridContainer MapGridy;

    static MapGrid Instance;

    bool MapActive = false;

    [Export]
    float MaxZoomScale = 2;

    public static MapGrid GetInstance()
    {
        return Instance;
    }
    public void ToggleMap(bool toggle)
    {
        
        if (toggle)
        {
            ((Control)GetParent()).Show();
            SetProcessInput(true);
            MapActive = true;
        }
        else
        {
            ((Control)GetParent()).Hide();
            SetProcessInput(false);
            MapActive = false;
        }
            
    }
    public override void _Ready()
    {
        Instance = this;
        PackedScene tilescene = GD.Load<PackedScene>(TileScene);
        Control par = (Control)GetParent();
        MapGridx = par.GetNode<Panel>("Panel").GetNode<GridContainer>("MapGridX");
        MapGridy = par.GetNode<Panel>("Panel2").GetNode<GridContainer>("MapGridY");
        while (times > 0)
        {
            Control maptile = (Control)tilescene.Instance();
            ChildMapIleInfo info = new ChildMapIleInfo();
            info.MapIle = maptile;
            AddChild(maptile);
            times -= 1;
            children.Insert(children.Count, info);
        }
        PackedScene gridtilexscene = GD.Load<PackedScene>(XGridTileScene);
        PackedScene gridtileyscene = GD.Load<PackedScene>(YGridTileScene);
        for (int i = 0; i < Columns; i ++)
        {
            Control gridtilex = (Control)gridtilexscene.Instance();
            Control gridtiley = (Control)gridtileyscene.Instance();
            MapGridx.AddChild(gridtilex);
            MapGridy.AddChild(gridtiley);
        }
        MarginLeft = -(RectSize.x / 2);
        RectPosition = RectScale/2; 
    }
    public bool IsMouseInMap()
    {
        if (!InventoryUI.GetInstance().IsOpen)
            return false;
        if (!MapActive)
            return false;
        bool mouseinmap = true;
        Control parent = (Control)GetParent();
        
        Vector2 boxminextends = parent.RectGlobalPosition;
        Vector2 boxmaxextends = parent.RectGlobalPosition + parent.RectSize;
        Vector2 mousepos = GetGlobalMousePosition();
        if (boxminextends.x > mousepos.x || boxminextends.y > mousepos.y || boxmaxextends.x < mousepos.x || boxmaxextends.y < mousepos.y)
            mouseinmap = false;
        return mouseinmap;
    }
    public override void _Input(InputEvent @event)
	{
        if (!IsMouseInMap())
            return;
		if (@event is InputEventMouseMotion)
		{
            if (!Visible)
                return;
            if (!Input.IsActionPressed("Select"))
				return;

			Vector2 pos = new Vector2(RectPosition.x + ((InputEventMouseMotion)@event).Relative.x, RectPosition.y + ((InputEventMouseMotion)@event).Relative.y);
            //if (pos.x < 8 && pos.y < 8)
            //{
                MapGridx.RectPosition = new Vector2(pos.x, 8);
                MapGridy.RectPosition = new Vector2(8, pos.y);
                RectPosition = pos;
           // }
            
		}
        Vector2 scale = RectScale;
        if (@event.IsActionPressed("ZoomIn"))
		{
			if (RectScale.x < MaxZoomScale)
            {
                RectScale *= 1.1f;
                RectPosition *= 1.1f;
                MapGridx.RectPosition = new Vector2(MapGridx.RectPosition.x * 1.1f, MapGridx.RectPosition.y);
                MapGridy.RectPosition  = new Vector2(MapGridy.RectPosition.x, MapGridy.RectPosition.y * 1.1f);
                MapGridx.RectScale = new Vector2(MapGridx.RectScale.x * 1.1f, MapGridx.RectScale.y);
                MapGridy.RectScale = new Vector2(MapGridy.RectScale.x, MapGridy.RectScale.y * 1.1f);
            }
				
		}
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (RectScale.x > 0.1f)
            {
                RectScale *= 0.9f;
                RectPosition *= 0.9f;
                MapGridx.RectPosition = new Vector2(MapGridx.RectPosition.x * 0.9f, MapGridx.RectPosition.y);
                MapGridy.RectPosition  = new Vector2(MapGridy.RectPosition.x, MapGridy.RectPosition.y * 0.9f);
                MapGridx.RectScale = new Vector2(MapGridx.RectScale.x * 0.9f, MapGridx.RectScale.y);
                MapGridy.RectScale = new Vector2(MapGridy.RectScale.x, MapGridy.RectScale.y * 0.9f);
            }
				
		}
        if (@event.IsActionPressed("FrameCamera"))
        {
            RectPosition = RectScale/2; 
        }
    }
    public void OnIslandVisited(IslandInfo info)
    {
        Control MapIle;
        MapIleList.TryGetValue(info.pos, out MapIle);
        if (MapIle == null)
            return;
        Color col = MapIle.Modulate;
        col.a = 1;
        MapIle.Modulate = col;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void InitMap()
    {
        WorldMap map = WorldMap.GetInstance();
        if (map == null)
            return;
        var entries = map.GetUsedCellsById(0);
        var seacells = map.GetUsedCellsById(3);
        var Lighthouses = map.GetUsedCellsById(4);
        var cells = map.GetUsedCells();
        for (int i = 0; i < cells.Count; i++)
        {
            Control child = children[i].MapIle;
            if (seacells.Contains(cells[i]))
                child.Modulate = new Color(0, 0, 1, 1);
            else if (Lighthouses.Contains(cells[i]))
                child.Modulate = new Color(1, 1, 0);
            else if (entries.Contains(cells[i]))
                child.Modulate = new Color(1, 0, 0);
            else
                child.Modulate = new Color(0, 1, 0, 1);
            MapIleList.Add((Vector2)cells[i], child);
        }
        SetProcess(false);
    }
}
struct ChildMapIleInfo
{
    public Control MapIle;
}