using Godot;
using System;
using System.Collections.Generic;

public class MapGrid : GridContainer
{
    [Export]
    float MaxZoomScale = 2;
    [Export]
    PackedScene TileScene = null;
    [Export]
    PackedScene XGridTileScene = null;
    [Export]
    PackedScene YGridTileScene = null;
    [Export]
    List<Color> ColorList = null;

    List<ChildMapIleInfo> children = new List<ChildMapIleInfo>();
    Dictionary<Vector2, MapTile> MapIleList = new Dictionary<Vector2, MapTile>();

    GridContainer MapGridx;
    GridContainer MapGridy;

    static MapGrid Instance;

    bool MapActive = false;


    MapUI mapui;

    

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
        //Texture tex = (Texture)th.Texture;
        MapGridx = par.GetParent().GetNode<Panel>("Panel3").GetNode<GridContainer>("MapGridX");
        MapGridy = par.GetParent().GetNode<Panel>("Panel2").GetNode<GridContainer>("MapGridY");

        SetProcessInput(false);
        //MarginLeft = -(RectSize.x / 2);
        //RectPosition = RectScale/2; 
    }
    public void InitMap()
    {
         WorldMap map = WorldMap.GetInstance();

        var cells = map.GetUsedCells();

        int times = cells.Count;

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
            gridtilex.Name = "x" + (i-40);
            gridtiley.Name = "y" + (i-40);
            MapGridx.AddChild(gridtilex);
            MapGridy.AddChild(gridtiley);
            
        }
        

        if (RectScale.x <= 0.5f)
            SwitchGridValues(0);
        else if (RectScale.x <= 2)
            SwitchGridValues(1);
        else
            SwitchGridValues(2);
        
        for (int i = 0; i < children.Count; i++)
        {
            MapTile child =(MapTile)children[i].MapIle;
            MapIleList.Add((Vector2)cells[i], child);
        }

        CallDeferred("FrameMap");
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

            MapGridx.RectPosition = new Vector2(pos.x, 8);

            MapGridy.RectPosition = new Vector2(8, pos.y);
            RectPosition = new Vector2(pos.x, pos.y);

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
                MapGridx.AddConstantOverride("hseparation", (int)(12 * RectScale.x));
                MapGridy.AddConstantOverride("vseparation", (int)(12 * RectScale.y));
            }
				
		}
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (RectScale.x > 0.25f)
            {
                RectScale /= 2;
                
                RectPosition /= 2;
                Vector2 parentsize = ((Control)GetParent()).RectSize;
                RectPosition += parentsize /4;
                MapGridx.RectPosition = new Vector2(MapGridx.RectPosition.x / 2 + parentsize.x /4, MapGridx.RectPosition.y);
                MapGridy.RectPosition  = new Vector2(MapGridy.RectPosition.x, MapGridy.RectPosition.y / 2 + parentsize.y /4);
                MapGridx.AddConstantOverride("hseparation", (int)(12 * RectScale.x));
                MapGridy.AddConstantOverride("vseparation", (int)(12 * RectScale.y));
            }
		}
        if (@event.IsActionPressed("FrameCamera"))
        {
            FrameMap();
        }
        if (RectScale.x <= 0.25f)
            SwitchGridValues(0);
        else if (RectScale.x <= 0.5f)
            SwitchGridValues(1);
        else if (RectScale.x <= 2)
            SwitchGridValues(2);
        else
            SwitchGridValues(3);
    }
    public void FrameMap()
    {
        Vector2 parentsize = ((Control)GetParent()).RectSize;
        RectPosition = new Vector2( (- RectSize.x / 2 * RectScale.x) + parentsize.x / 2,(- RectSize.y / 2 * RectScale.y) + parentsize.y / 2);

        MapGridx.RectPosition = new Vector2(RectPosition.x, 8);

        MapGridy.RectPosition = new Vector2(8, RectPosition.y);
        
    }
    public void SwitchGridValues(int toggle)
    {
        if (toggle == 0)
        {
            int index = 0;
            foreach (Control child in MapGridx.GetChildren())
            {
                if (index == 0)
                {
                    index = 19;
                    child.RectClipContent = false;
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
                    index = 19;
                    child.RectClipContent = false;
                    continue;
                }

                index -= 1;
                child.RectClipContent = true;
            }
        }
        else if (toggle == 1)
        {
            int index = 0;
            foreach (Control child in MapGridx.GetChildren())
            {
                if (index == 0)
                {
                    index = 9;
                    child.RectClipContent = false;
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
                    index = 9;
                    child.RectClipContent = false;
                    continue;
                }

                index -= 1;
                child.RectClipContent = true;
            }
        }
        else if (toggle == 2)
        {  
            int index = 0;
            foreach (Control child in MapGridx.GetChildren())
            {
                if (index == 0)
                {
                    index = 4;
                    child.RectClipContent = false;
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
                    child.RectClipContent = false;
                    continue;
                }

                index -= 1;
                child.RectClipContent = true;
            }
        }
        else if (toggle == 3)
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
        MapTile MapIle;
        MapIleList.TryGetValue(info.Position, out MapIle);
        if (MapIle == null)
            return;
        Color col = MapIle.Modulate;
        col.a = 1;
        MapIle.Modulate = col;
    }
    
    public void UpdateIleInfo(Vector2 index, IleType type)
    {
        MapTile child;
        MapIleList.TryGetValue(index, out child);
        
        if (type == IleType.ENTRANCE)
        {
            child.Modulate = ColorList[0];
            child.GetNode<TextureRect>("TextureRect").HintTooltip = "Η Μητρόπολη";
        }
        else if (type == IleType.EXIT)
        {
            child.Modulate = ColorList[1];
            child.GetNode<TextureRect>("TextureRect").HintTooltip = "Σκάφος";
        }
        else if (type == IleType.LAND)
        {
            child.Modulate = ColorList[2];
            child.GetNode<TextureRect>("TextureRect").HintTooltip = string.Empty;
        } 
        else if (type == IleType.LIGHTHOUSE)
        {
            child.Modulate = ColorList[3];
            child.GetNode<TextureRect>("TextureRect").HintTooltip = "Φάρος";
        }
        else
        {
            child.Modulate = ColorList[4];
            child.GetNode<TextureRect>("TextureRect").HintTooltip = string.Empty;
        }
        child.type = (int)type;
    }
    public Dictionary<Vector2, int> GetSaveData()
    {
        Dictionary<Vector2, int> data = new Dictionary<Vector2, int>();
        foreach (KeyValuePair<Vector2, MapTile> entry in MapIleList)
        {
            data.Add(entry.Key, entry.Value.type);
        }
        return data;
    }
    public void LoadSaveData(Dictionary<Vector2, int> Data)
    {
        foreach (KeyValuePair<Vector2, int> entry in Data)
        {
            UpdateIleInfo(entry.Key, (IleType)entry.Value);
        }
    }
}
class ChildMapIleInfo
{
    public Control MapIle;
}