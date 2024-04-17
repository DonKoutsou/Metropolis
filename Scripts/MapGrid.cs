using Godot;
using System;
using System.Collections.Generic;

public class MapGrid : GridContainer
{
    [Export]
    string TileScene;
    int times = 6561;

    PackedScene tilescene;

    List<ChildMapIleInfo> children = new List<ChildMapIleInfo>();
    Dictionary<Vector2, Control> MapIleList = new Dictionary<Vector2, Control>();

    static MapGrid Instance;

    public static MapGrid GetInstance()
    {
        return Instance;
    }
    public override void _Ready()
    {
        Instance = this;
        tilescene = GD.Load<PackedScene>(TileScene);
        
        while (times > 0)
        {
            Control maptile = (Control)tilescene.Instance();
            ChildMapIleInfo info = new ChildMapIleInfo();
            info.MapIle = maptile;
            AddChild(maptile);
            times -= 1;
            children.Insert(children.Count, info);
        }
        MarginLeft = -(RectSize.x / 2);
    }
    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
            if (!Visible)
                return;
            if (!Input.IsActionPressed("Select"))
				return;
            Control parent = (Control)GetParent();
            Vector2 boxminextends = parent.RectGlobalPosition;
            Vector2 boxmaxextends = parent.RectGlobalPosition + parent.RectSize;
            Vector2 mousepos = GetGlobalMousePosition();
            if (boxminextends.x > mousepos.x || boxminextends.y > mousepos.y || boxmaxextends.x < mousepos.x || boxmaxextends.y < mousepos.y)
                return;
            //if (!MapUI.IsMouseInMap())
               // return;
			

			Vector2 pos = new Vector2(RectPosition.x + ((InputEventMouseMotion)@event).Relative.x, RectPosition.y + ((InputEventMouseMotion)@event).Relative.y);
			RectPosition = pos;
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
                child.Modulate = new Color(0, 0, 1, 0);
            else if (Lighthouses.Contains(cells[i]))
                child.Modulate = new Color(1, 1, 0);
            else if (entries.Contains(cells[i]))
                child.Modulate = new Color(1, 0, 0);
            else
                child.Modulate = new Color(0, 1, 0, 0);
            MapIleList.Add((Vector2)cells[i], child);
        }
        SetProcess(false);
    }
}
struct ChildMapIleInfo
{
    public Control MapIle;
}