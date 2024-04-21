using Godot;
using System;
using System.Collections.Generic;

public class MapGrid : GridContainer
{
    [Export]
    PackedScene TileScene = null;
    [Export]
    PackedScene XGridTileScene = null;
    [Export]
    PackedScene YGridTileScene = null;


    [Export]
    List<Color> ColorList = null;
    int times = 6561;


    List<ChildMapIleInfo> children = new List<ChildMapIleInfo>();
    Dictionary<Vector2, Control> MapIleList = new Dictionary<Vector2, Control>();

    GridContainer MapGridx;
    GridContainer MapGridy;

    static MapGrid Instance;

    bool MapActive = false;

    StyleBoxTexture th;

    MapUI mapui;

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
            ((Control)GetParent().GetParent()).Show();
            SetProcessInput(true);
            MapActive = true;
            mapui.OnMapOpened();
        }
        else
        {
            ((Control)GetParent().GetParent()).Hide();
            SetProcessInput(false);
            MapActive = false;
            mapui.OnMapClosed();
        }
            
    }
    public override void _Ready()
    {
        Instance = this;
        Panel par = (Panel)GetParent();
        mapui = (MapUI)par.GetParent();
        th = (StyleBoxTexture)par.GetStylebox("normal");
        //Texture tex = (Texture)th.Texture;
        MapGridx = par.GetNode<GridContainer>("MapGridX");
        MapGridy = par.GetNode<GridContainer>("MapGridY");
        while (times > 0)
        {
            Control maptile = (Control)TileScene.Instance();
            ChildMapIleInfo info = new ChildMapIleInfo();
            info.MapIle = maptile;
            AddChild(maptile);
            times -= 1;
            children.Insert(children.Count, info);
        }
        for (int i = 0; i < Columns; i ++)
        {
            Control gridtilex = (Control)XGridTileScene.Instance();
            Control gridtiley = (Control)YGridTileScene.Instance();
            
            MapGridx.AddChild(gridtilex);
            MapGridy.AddChild(gridtiley);
            gridtilex.Name = "x" + (i-40);
            gridtiley.Name = "y" + (i-40);
        }
        //MarginLeft = -(RectSize.x / 2);
        //RectPosition = RectScale/2; 
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
            //when scale is 1 max position is -500
            float posallowance = -500 * RectScale.x;
            //if (pos.x < 0 && pos.x > posallowance)
            //{
            MapGridx.RectPosition = new Vector2(pos.x, 8);
                // den doulevei
                //Rect2 rec = new Rect2(new Vector2 (pos.x, RectPosition.y), new Vector2(512, 512));
                //th.RegionRect = rec;
            //}
            //if  (pos.y < 0 && pos.y > posallowance)
           // {
            MapGridy.RectPosition = new Vector2(8, pos.y);
            RectPosition = new Vector2(pos.x, pos.y);
                // den doulevei
                //Rect2 rec = new Rect2(new Vector2 (RectPosition.x, pos.y), new Vector2(512, 512));
                //th.RegionRect = rec;
            //}
		}
        Vector2 scale = RectScale;
        if (@event.IsActionPressed("ZoomIn"))
		{
			if (RectScale.x < MaxZoomScale)
            {
                RectScale *= 2;
                
                RectPosition *= 2;
                Vector2 parentsize = ((Control)GetParent()).RectSize;
                RectPosition -= parentsize /2;
                MapGridx.RectPosition = new Vector2(MapGridx.RectPosition.x * 2 - parentsize.x /2, MapGridx.RectPosition.y);
                MapGridy.RectPosition  = new Vector2(MapGridy.RectPosition.x, MapGridy.RectPosition.y * 2 - parentsize.y /2);
                MapGridx.AddConstantOverride("hseparation", (int)(15 * RectScale.x));
                MapGridy.AddConstantOverride("vseparation", (int)(15 * RectScale.y));
                //MapGridx.RectScale = new Vector2(MapGridx.RectScale.x * 1.1f, MapGridx.RectScale.y * 1.1f);
                //MapGridy.RectScale = new Vector2(MapGridy.RectScale.x * 1.1f, MapGridy.RectScale.y * 1.1f);
            }
				
		}
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (RectScale.x > 1)
            {
                RectScale /= 2;
                
                RectPosition /= 2;
                Vector2 parentsize = ((Control)GetParent()).RectSize;
                RectPosition += parentsize /4;
                MapGridx.RectPosition = new Vector2(MapGridx.RectPosition.x / 2 + parentsize.x /4, MapGridx.RectPosition.y);
                MapGridy.RectPosition  = new Vector2(MapGridy.RectPosition.x, MapGridy.RectPosition.y / 2 + parentsize.y /4);
                MapGridx.AddConstantOverride("hseparation", (int)(15 * RectScale.x));
                MapGridy.AddConstantOverride("vseparation", (int)(15 * RectScale.y));
                //MapGridx.RectScale = new Vector2(MapGridx.RectScale.x * 0.9f, MapGridx.RectScale.y * 0.9f);
                //MapGridy.RectScale = new Vector2(MapGridy.RectScale.x * 0.9f, MapGridy.RectScale.y * 0.9f);
            }
		}
        if (RectScale.x <= 2)
            SwitchGridValues(true);
        else
            SwitchGridValues(false);
        if (@event.IsActionPressed("FrameCamera"))
        {
            float sc = RectScale.x;
            float sz = RectSize.x;
            float amm = sz/2 * sc;
            MarginBottom = amm;
            MarginLeft = -amm;
            MarginRight = amm;
            MarginTop = -amm;
            MapGridx.RectPosition = new Vector2(RectPosition.x, 8);
            MapGridy.RectPosition = new Vector2(8, RectPosition.y);
        }
    }
    public void SwitchGridValues(bool toggle)
    {
        if (toggle)
        {  
            int index = 0;
            foreach (Control child in MapGridx.GetChildren())
            {
                if (index == 0)
                {
                    index = 4;
                    continue;
                }

                index -= 1;
                child.RectClipContent = true;
            }
            index = 0;
            foreach (Control child in MapGridy.GetChildren())
            {
                if (index == 0)
                {
                    index = 4;
                    continue;
                }

                index -= 1;
                child.RectClipContent = true;
            }
        }
        else
        {
            foreach (Control child in MapGridx.GetChildren())
            {
                child.RectClipContent = false;
            }
            foreach (Control child in MapGridy.GetChildren())
            {
                child.RectClipContent = false;
            }
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

        var cells = map.GetUsedCells();
        for (int i = 0; i < children.Count; i++)
        {
            Control child = children[i].MapIle;
            MapIleList.Add((Vector2)cells[i], child);
        }
        SetProcess(false);
    }
    public void UpdateIleInfo(Vector2 index, IleType type)
    {
        Control child;
        MapIleList.TryGetValue(index, out child);
        if (type == IleType.ENTRANCE)
            child.Modulate = ColorList[0];
        else if (type == IleType.EXIT)
            child.Modulate = ColorList[1];
        else if (type == IleType.LAND)
            child.Modulate = ColorList[2];
        else if (type == IleType.LIGHTHOUSE)
            child.Modulate = ColorList[3];
        else
            child.Modulate = ColorList[4];
    }
}
class ChildMapIleInfo
{
    public Control MapIle;
}