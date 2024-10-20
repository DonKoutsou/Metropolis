using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MapGrid : GridContainer
{
    [Signal]
    public delegate void TileHovered();
    [Export]
    float MaxZoomScale = 2;
    [Export]
    PackedScene TileScene = null;
    [Export]
    PackedScene XGridTileScene = null;
    [Export]
    PackedScene YGridTileScene = null;

    
    Dictionary<Vector2, MapTile> MapIleList = new Dictionary<Vector2, MapTile>();

    GridContainer MapGridx;
    GridContainer MapGridy;

    public bool MapActive = false;

    MapUI mapui;

    Control PlayerIconPivot;

    bool first = true;

    Label IleNameLabel;
    public void ToggleMap(bool toggle)
    {
        if (MapActive == toggle)
            return;
        if (toggle)
        {
            if (first)
            {
                FrameMap();
                first = false;
            }
            //((Control)GetParent().GetParent()).Show();
            SetProcessInput(true);
            SetProcess(true);
            MapActive = true;
            
        }
        else
        {
            //((Control)GetParent().GetParent()).Hide();
            SetProcessInput(false);
            SetProcess(false);
            MapActive = false;
            IleNameLabel.Visible = false;
        }
            
    }
    public override void _Ready()
    {
        Panel par = (Panel)GetParent();
        mapui = (MapUI)par.GetParent();
        //Texture tex = (Texture)th.Texture;
        MapGridx = par.GetParent().GetNode<Panel>("Panel3").GetNode<GridContainer>("MapGridX");
        MapGridy = par.GetParent().GetNode<Panel>("Panel2").GetNode<GridContainer>("MapGridY");
        PlayerIconPivot = par.GetNode<Control>("PlayerIconPivot");
        SetProcessInput(false);
        SetProcess(false);
        //MarginLeft = -(RectSize.x / 2);
        //RectPosition = RectScale/2; 
        IleNameLabel = GetParent().GetParent().GetNode<Label>("IleName");
        IleNameLabel.Hide();
    }
    Player pl;
    public void ConnectPlayer(Player p)
    {
        pl = p;
    }
    public void ResetMap()
    {
        MapIleList.Clear();
        foreach(MapTile child in GetChildren())
        {
            child.QueueFree();
        }
    }
    string HoveredName = string.Empty;
    public void OnTileHovered(string name)
    {
        if (name != "No_Name")
            EmitSignal("TileHovered");
        HoveredName = name;
    }
    public void InitMap()
    {
        WorldMap map = WorldMap.GetInstance();

        var cells = map.GetUsedCells();

        int times = cells.Count;

        List<ChildMapIleInfo> children = new List<ChildMapIleInfo>();

        while (times > 0)
        {
            Control maptile = (Control)TileScene.Instance();
            ChildMapIleInfo info = new ChildMapIleInfo()
            {
                MapIle = maptile
            };
            AddChild(maptile);
            maptile.Connect("OnHovered", this, "OnTileHovered");
            times -= 1;
            children.Insert(children.Count, info);
        }
        for (int i = 0; i < Columns; i ++)
        {
            Control gridtilex = (Control)XGridTileScene.Instance();
            Control gridtiley = (Control)YGridTileScene.Instance();
            gridtilex.Name = "x" + (i-11);
            gridtiley.Name = "y" + (i-11);
            MapGridx.AddChild(gridtilex);
            MapGridy.AddChild(gridtiley);
            
        }

        if (RectScale.x <= 0.25f)
            SwitchGridValues(0);
        else if (RectScale.x <= 0.5f)
            SwitchGridValues(1);
        else if (RectScale.x <= 2)
            SwitchGridValues(2);
        else
            SwitchGridValues(3);

        for (int i = 0; i < children.Count; i++)
        {
            MapTile child =(MapTile)children[i].MapIle;
            MapIleList.Add((Vector2)cells[i], child);
        }
    }
    public bool IsMouseInMap()
    {
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
        //if (!IsMouseInMap())
            //return;
		if (@event is InputEventMouseMotion)
		{
            if (!Visible)
                return;
            if (!Input.IsActionPressed("Select"))
				return;

			Vector2 pos = new Vector2(RectPosition.x + ((InputEventMouseMotion)@event).Relative.x, RectPosition.y + ((InputEventMouseMotion)@event).Relative.y);

            Control parent = (Control)GetParent();

            Vector2 size = RectSize * RectScale;

            if (pos.x > 14)
                pos.x = 14;
            if (Mathf.Abs(pos.x) + parent.RectSize.x  > size.x + 14)
                pos.x = -(size.x + 14 - parent.RectSize.x);
            if (pos.y > 14)
                pos.y = 14;
            if (Mathf.Abs(pos.y) +  parent.RectSize.y> size.y + 14)
                pos.y = -(size.y + 14 - parent.RectSize.y);

            MapGridx.RectPosition = new Vector2(pos.x, 8);

            MapGridy.RectPosition = new Vector2(8, pos.y);
            RectPosition = new Vector2(pos.x, pos.y);
            PlayerIconPivot.RectPosition = RectPosition;
		}
        else if (@event is InputEventJoypadMotion)
        {
            Control parent = (Control)GetParent();

            Vector2 pos = new Vector2(
            Input.GetActionStrength("CameraRight") - Input.GetActionStrength("CameraLeft"),
            Input.GetActionStrength("CameraDown") - Input.GetActionStrength("CameraUp")
            ).LimitLength(1);

            pos = new Vector2((float)Math.Round(pos.x, 3), (float)Math.Round(pos.y, 3))* 10;

            Vector2 size = RectSize * RectScale;

            if (pos.x > 14)
                pos.x = 14;
            if (Mathf.Abs(pos.x) + parent.RectSize.x  > size.x + 14)
                pos.x = -(size.x + 14 - parent.RectSize.x);
            if (pos.y > 14)
                pos.y = 14;
            if (Mathf.Abs(pos.y) +  parent.RectSize.y> size.y + 14)
                pos.y = -(size.y + 14 - parent.RectSize.y);

            MapGridx.RectPosition += new Vector2(pos.x, 8);

            MapGridy.RectPosition += new Vector2(8, pos.y);
            RectPosition += new Vector2(pos.x, pos.y);
            PlayerIconPivot.RectPosition = RectPosition;
        }
        if (@event.IsActionPressed("ZoomIn"))
		{
			if (RectScale.x < MaxZoomScale)
            {
                RectScale *= 2;
                
                RectPosition *= 2;
                Vector2 parentsize = ((Control)GetParent()).RectSize;
                RectPosition -= parentsize /2;
                //PlayerIconPivot.RectPosition -= parentsize /2;
                PlayerIconPivot.RectPosition = RectPosition;
                MapGridx.RectPosition = new Vector2(MapGridx.RectPosition.x * 2 - parentsize.x /2, MapGridx.RectPosition.y);
                MapGridy.RectPosition  = new Vector2(MapGridy.RectPosition.x, MapGridy.RectPosition.y * 2 - parentsize.y /2);
                MapGridx.AddConstantOverride("hseparation", (int)(12 * RectScale.x));
                MapGridy.AddConstantOverride("vseparation", (int)(12 * RectScale.y));
            }
		}
		if (@event.IsActionPressed("ZoomOut"))
		{
			if (RectScale.x > 12)
            {
                RectScale /= 2;
                
                RectPosition /= 2;
                //PlayerIconPivot.RectPosition /= 2;
                Vector2 parentsize = ((Control)GetParent()).RectSize;
                RectPosition += parentsize /4;
                //PlayerIconPivot.RectPosition += parentsize /4;
                PlayerIconPivot.RectPosition = RectPosition;
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
        PlayerIconPivot.RectPosition = new Vector2( (- RectSize.x / 2 * RectScale.x) + parentsize.x / 2,(- RectSize.y / 2 * RectScale.y) + parentsize.y / 2);
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
    public void OnIslandToggled(Vector2 index, bool toggle)
    {
        MapIleList[index].GetNode<Panel>("DebugPanel").Visible = toggle;
    }
    public void SetPortVissible(Vector2 index, Vector2 portpos)
    {
        TextureRect child = MapIleList[index].GetNode<TextureRect>("TextureRect");
        Vector2 pos = new Vector2(6,6);
        pos += portpos/4000 * 6;
        foreach (TextureRect r in child.GetChildren())
        {
            if (r.RectPosition == pos)
            {
                r.Visible = true;
                return;
            }
        }
    }
    public void SetIslandVisited(IslandInfo ile)
    {
        ile.Island.SetVisited();
        TextureRect child = MapIleList[ile.Position].GetNode<TextureRect>("TextureRect");
        child.Visible = true;
    }
    public void UnlockIslandName(IslandInfo ile)
    {
        //ile.Island.UnlockName = true;
        MapTile child = MapIleList[ile.Position];
        child.IslandName = ile.SpecialName;
        Panel pan = child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("Panel");
        pan.Visible = true;
    }
    public void ForceIslandVisited(IslandInfo ile)
    {
        ile.Island.SetVisited();
        TextureRect child = MapIleList[ile.Position].GetNode<TextureRect>("TextureRect");
        child.Visible = true;
    }
    //public void UpdateIleInfo(Vector2 index, IleType type, bool HasPort, List<Vector2> portpos, float rot = 0, ImageTexture img = null, string name = null)
    public void UpdateIleInfo(Vector2 index, bool Visited, float rot = 0, ImageTexture img = null, string name = null)
    {
        MapTile child = MapIleList[index];

        /*for (int i = 0; i < 3; i++)
        {
            TextureRect t = child.GetNode<TextureRect>("TextureRect").GetNode<TextureRect>("PortTex" + i);
            if (portpos.Count - 1 < i)
            {
                t.QueueFree();
                continue;
            }
            
            
            Vector2 pos = new Vector2(6,6);
            pos += portpos[i].Location/4000 * 6;
            t.RectRotation = - rot;
            //postoput.x += 6; postoput.y += 6;
            t.RectPosition = pos;
            if (!portpos[i].Visited)
                t.Visible = false;
            //t.RectPosition = Vector2.Zero;
        }*/
        /*if (type == IleType.ENTRANCE)
        {
            child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("SignPanel").Visible = true;
            StyleBoxFlat theme = (StyleBoxFlat)child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("SignPanel").GetStylebox("panel");
            theme.BgColor = new Color(0,0,1);
            //child.GetNode<Panel>("SignPanel").HintTooltip = name;
            //child.GetNode<Panel>("SignPanel").RectRotation = -rot;
            //child.Modulate = ColorList[0];
            //child.GetNode<TextureRect>("TextureRect").HintTooltip = "Η Μητρόπολη";
        }
        else if (type == IleType.EXIT)
        {
            child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("SignPanel").Visible = true;
            StyleBoxFlat theme = (StyleBoxFlat)child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("SignPanel").GetStylebox("panel");
            theme.BgColor = new Color(1,0,0);
            //child.Modulate = ColorList[1];
            //child.GetNode<TextureRect>("TextureRect").HintTooltip = "Σκάφος";
        }
        else if (type == IleType.LAND)
        {
            //child.Modulate = ColorList[2];
            //child.GetNode<TextureRect>("TextureRect").HintTooltip = string.Empty;
        } 
        else if (type == IleType.LIGHTHOUSE)
        {
            child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("SignPanel").Visible = true;
            //child.GetNode<Panel>("SignPanel").HintTooltip = name;
            //child.GetNode<Panel>("SignPanel").RectRotation = -rot;
            //child.Modulate = ColorList[3];
            //child.GetNode<TextureRect>("TextureRect").HintTooltip = "Φάρος";
        }
        else
        {
            //child.Modulate = ColorList[4];
            //child.GetNode<TextureRect>("TextureRect").HintTooltip = string.Empty;
        }*/
        if (img != null)
        {
            TextureRect tex = child.GetNode<TextureRect>("TextureRect");
            tex.Texture = img;
            tex.RectRotation = rot;
            tex.Visible = Visited;
        }
        //child.type = (int)type;
        //float thing = Math.Max(Math.Abs(index.x), Math.Abs(index.y)) / 40;
        //Panel fogp = child.GetNode<Panel>("FogPanel");
        //Color c = fogp.Modulate;
        //c.a = thing;
        //fogp.Modulate = c;
        child.IslandName = name;
        if (name != "No_Name")
        {
            Panel pan2 = child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("Panel");
            pan2.Visible = true;
        }
        
        Panel pan = child.GetNode<TextureRect>("TextureRect").GetNode<Panel>("Panel");
        pan.Visible = name != "No_Name";

        child.Modulate = new Color(1,1,1,1);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (MapIleList.Count == 0)
            return;
        //player icon location is at 0,0 in the grid wich is top left corner. Get location of center of grid and treat that as 0,0
        Vector2 center = MapIleList[new Vector2(0,0)].RectPosition;
        GetParent().GetNode<Control>("PlayerIconPivot").GetNode<Panel>("PlayerIcon").RectPosition = (center + new Vector2( (pl.GlobalTranslation.x - 4000) * 0.0015f,  (pl.GlobalTranslation.z - 4000) * 0.0015f)) * RectScale;
        //GetParent().GetNode<Control>("PlayerIconPivot").GetNode<Panel>("PlayerIcon").RectPosition = (center + new Vector2( (pl.GlobalTranslation.x + (-MyWorld.GetInstance().Translation).x - 4000) * 0.0015f,  (pl.GlobalTranslation.z + (-MyWorld.GetInstance().Translation).z - 4000) * 0.0015f)) * RectScale;

        if (HoveredName == "No_Name")
        {
            IleNameLabel.Hide();
        }
        else
        {
            IleNameLabel.Show();
            IleNameLabel.Text = HoveredName;
            Vector2 mousepos = GetViewport().GetMousePosition();
            mousepos.x -= IleNameLabel.RectSize.x /2;
            mousepos.y -= IleNameLabel.RectSize.y;
            IleNameLabel.RectPosition = mousepos;
        }
    } 
}
class ChildMapIleInfo
{
    public Control MapIle;
}