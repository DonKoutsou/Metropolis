using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;


public class WorldMap : TileMap
{
    [Export]
    public PackedScene[] scenestospawn;

    [Export]
    public PackedScene[] Eventscenestospawn;

    List <PackedScene> loadedscenes = new List<PackedScene>();

    [Export]
    public PackedScene Entrytospawn;

    [Export]
    public PackedScene Exittospawn;

    [Export]
    public PackedScene Sea;

    [Export]
    public PackedScene LightHouse;

    [Export]
    public bool HideBasedOnState = false;

    [Export]
    public bool RandomRotation = true;

    [Export]
    int CellSizeOverride = 8000;

    [Export]
    public int seed = 69420;

    int currentile;

    List <float> rots = new List<float>{0f, 90f, 180f, -90f};


    //id of cells to be changed to events
    List <int> RandomisedEntryID = null;

    //id of exit
    int ExitID = 0;

    List <Vector2> OrderedCells = new List<Vector2>();

    Random random;

    Vector2 CurrentTile;

    static Dictionary<Vector2, IslandInfo> ilemap = new Dictionary<Vector2, IslandInfo>();

    bool finishedspawning = false;

    IslandInfo entry;

    static WorldMap Instance;

    public static WorldMap GetInstance()
    {
        return Instance;
    }
    public override void _Ready()
    {
        Instance = this;

        //MapGrid.GetInstance().InitMap();

        int seed = Settings.GetGameSettings().Seed;

        random = new Random(seed);
            
        CellSize = new Vector2(CellSizeOverride, CellSizeOverride);

        ArrangeCellsBasedOnDistance();

        for (int i = 0; i < scenestospawn.Count(); i++)
        {
            loadedscenes.Insert(i, scenestospawn[i]);
        }
        MapGrid.GetInstance().InitMap();
        //var pls = GetTree().GetNodesInGroup("player");
        //pl = (Player)pls[0];
    }
    public Vector2 GetCurrentTile()
    {
        return WorldToMap(CurrentTile);
    }
    public override void _Process(float delta)
	{
        ulong ms = OS.GetSystemTimeMsecs();
        if (!finishedspawning)
            RegisterIsland(currentile);
        if (Player.GetInstance() == null)
            return;
        Vector2 plpos = new Vector2(Player.GetInstance().GlobalTransform.origin.x, Player.GetInstance().GlobalTransform.origin.z);
        if (plpos.DistanceTo(CurrentTile) > CellSize.x/2)
        {
            Vector2 curt = FindClosestIslandPosition(plpos);

            if (CurrentTile != curt)
            {
                IslandInfo ileinf = null;
                ilemap.TryGetValue(WorldToMap(CurrentTile), out ileinf);
                IslandInfo ileinfto = null;
                ilemap.TryGetValue(WorldToMap(curt), out ileinfto);
                CurrentTile = curt;
                
                MyWorld.IleTransition(ileinf, ileinfto);
            }
        }
        ulong msaf = OS.GetSystemTimeMsecs();
        if (msaf - ms > 25)
            GD.Print("World map processing took longer the 25 ms. Process time : " + (msaf - ms).ToString() + " ms");
    }
    public Island SpawnIsland(IslandInfo info)
    {
        Island Ile = (Island)info.IleType.Instance();

        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

        Vector2 postoput = MapToWorld(new Vector2(info.pos.x, info.pos.y));

        postoput += CellSize / 2;
        pos.x = postoput.x;
        pos.z = postoput.y;
        Ile.SetSpawnInfo(pos, info.rottospawn);

        info.SetInfo(Ile);
        MapGrid.GetInstance().UpdateIleInfo(info.pos, info.type);
        return Ile;
    }
    public Island ReSpawnIsland(IslandInfo info)
    {
        Island Ile = (Island)info.IleType.Instance();

        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

        Vector2 postoput = MapToWorld(new Vector2(info.pos.x, info.pos.y));

        postoput += CellSize / 2;
        pos.x = postoput.x;
        pos.z = postoput.y;
        Ile.SetSpawnInfo(pos, info.rottospawn);

        info.ile = Ile;
        //Ile.InputData(info);
        return Ile;
    }
    void ArrangeCellsBasedOnDistance()
    {
        //arange all the cells by distance from center of world put in OrderedCells array
        var cells = GetUsedCells();
        OrderedCells.Insert(0, (Vector2)cells[0]);
        for (int x = 1; x < cells.Count; x++)
        {
            Vector2 cellArray = (Vector2)cells[x];
            float ind = Math.Abs(cellArray.x) + Math.Abs(cellArray.y);

            Vector2 closest = OrderedCells[0];
            float dif = Math.Abs(Math.Abs(closest.x) + Math.Abs(closest.y) - ind);
            for (int i = OrderedCells.Count - 1; i > -1; i--)
            {
                float newdif = Math.Abs(Math.Abs(OrderedCells[i].x) + Math.Abs(OrderedCells[i].y) - ind);
                if (dif > newdif)
                {
                    closest = OrderedCells[i];
                    dif = newdif;
                }
            }
            if (Math.Abs(closest.x) + Math.Abs(closest.y) < Math.Abs(cellArray.x) + Math.Abs(cellArray.y))
            {
                OrderedCells.Insert(OrderedCells.IndexOf(closest) + 1, cellArray);
                continue;
            }
            else
            {
                OrderedCells.Insert(OrderedCells.IndexOf(closest), cellArray);
                continue;
            }
        }
        //produce indexes of where tiles events will be placed on
        RandomisedEntryID = new List<int>();
        for (int i = 0; i < Eventscenestospawn.Count(); i++)
        {
            int SpawnIndex = random.Next(0, OrderedCells.Count);
            RandomisedEntryID.Insert(i, SpawnIndex);
        }
        var exitcells = GetUsedCellsById(2);
        int RandomExitIndex = random.Next(0, exitcells.Count);
        Vector2 Exitpalcement = (Vector2)exitcells[RandomExitIndex];
        ExitID = OrderedCells.IndexOf(Exitpalcement) + 1;
    }
    //takes in cell position gives out global transforms of closest island
    Vector2 FindClosestIslandPosition(Vector2 pos)
    {
        float dist = 999999999;
        Vector2 closest = Vector2.Zero;
        foreach(KeyValuePair<Vector2, IslandInfo> entry in ilemap)
        {
            Vector2 ilepos = MapToWorld(new Vector2(entry.Value.pos.x, entry.Value.pos.y));
            ilepos += CellSize / 2;
            
            float Itdist = ilepos.DistanceTo(pos);
            if (dist > Itdist)
            {
                closest = ilepos;
                dist = Itdist;
            }
        }
        return closest;
    }
    public void GetClosest(Vector2 pos, out List<Vector2> closeIles, int dist = 2)
    {
        closeIles = new List<Vector2>();
        Vector3 transform = new Vector3(pos.x, 0, pos.y);

        int ammount = 0;
        for (int i = 0; i < dist; i ++)
            ammount += ammount + 8;

        int offset = CellSizeOverride * dist;
        float row = transform.x - offset;
        float collumn = transform.z - offset;
        for (int i = 0; i < ammount + 1; i++)
        {
            closeIles.Insert(closeIles.Count, new Vector2(row, collumn));
            row += CellSizeOverride;
            if (row > transform.x + offset)
            {
                row = transform.x - offset;
                collumn += CellSizeOverride;
                if (collumn > transform.z + offset)
                    break;

            }
        }
    }
    public void GetClosestIles(IslandInfo info, out List<IslandInfo> closeIles, int dist = 2)
    {
        
        closeIles = new List<IslandInfo>();
        Vector3 transform = new Vector3(info.pos.x, 0, info.pos.y);

        int ammount = 0;
        for (int i = 0; i < dist; i ++)
            ammount += ammount + 8;

        int offset = 1 * dist;
        float row = transform.x - offset;
        float collumn = transform.z - offset;
        for (int i = 0; i < ammount + 1; i++)
        {
            IslandInfo ile;
            ilemap.TryGetValue(new Vector2(row, collumn), out ile);
            closeIles.Insert(closeIles.Count, ile);
            row += 1;
            if (row > transform.x + offset)
            {
                row = transform.x - offset;
                collumn += 1;
                if (collumn > transform.z + offset)
                    break;
            }
        }
    }
    
   
    IslandInfo RegisterIsland(int curtile)
    {
        int id = GetCell((int)OrderedCells[curtile].x, (int)OrderedCells[curtile].y);
        Vector2 cell = OrderedCells[curtile];

        currentile += 1;

        PackedScene ilescene = GetSceneToSpawn(id);
        IslandInfo ileinfo = new IslandInfo();
        
        float rot = 0;

        int index = random.Next(rots.Count);
        rot = rots[index];

        ileinfo.rottospawn = rot;
        ileinfo.IleType = ilescene;
        ileinfo.pos = cell;
        
        Island ile = SpawnIsland(ileinfo);
        MyWorld.GetInstance().RegisterIle(ileinfo);
        ile.InitialSpawn(random);
        List<House> houses;
        ile.GetHouses(out houses);
        ileinfo.AddHouses(houses);

        
        MyWorld.GetInstance().ToggleIsland(ileinfo, false, false);
        ilemap.Add(cell, ileinfo);

        if (id == 0)
        {
            entry = ileinfo;
        }
        if (currentile == 100)
        {
            MyWorld.GetInstance().ToggleIsland(entry, true, true);
        
            Island island = entry.ile;

            //Position3D spawnpos = island.GetNode<Position3D>("SpawnPosition");
            //pl.Teleport(spawnpos.GlobalTransform.origin);
            //WorldClipRaycast.EnableWorldClipRaycast();

            GetTree().Root.GetCamera().Fov = Settings.GetGameSettings().FOVOverride;
            CurrentTile = new Vector2 (island.GlobalTransform.origin.x ,island.GlobalTransform.origin.z);

            List<IslandInfo> closestto2;
		    GetClosestIles(entry,out closestto2, 3);
            foreach(IslandInfo info in closestto2)
                MapGrid.GetInstance().OnIslandVisited(info);
        }

        if (currentile >= OrderedCells.Count)
            finishedspawning = true;
        return ileinfo;
    }
    PackedScene GetSceneToSpawn(int type)
    {
        PackedScene scene = null;
        //0 entry
        if (type == 0)
            scene = Entrytospawn;

        //1 random or event
        else if (type == 1)
        {
            if (RandomisedEntryID.Contains(currentile))
                scene = Eventscenestospawn[RandomisedEntryID.IndexOf(currentile)];
            else
                scene = loadedscenes[random.Next(0, loadedscenes.Count)];
        }
        //2 exit
        else if (type == 2)
        {
            if (currentile == ExitID)
                scene =  Exittospawn;
            else
                scene = loadedscenes[random.Next(0, loadedscenes.Count)];
        }
        //3 sea
        else if (type == 3)
            scene =  Sea;
        else if (type == 4)
            scene = LightHouse;
        return scene;
    }
	public void SyncSeas()
    {
        var seas = GetTree().GetNodesInGroup("Sea");
        float animstage = 0;
        foreach (Node sea in seas)
        {
            if (animstage == 0)
            {
                animstage = sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimationPosition;
            }
            sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").Seek(animstage);
            sea.GetNode<MeshInstance>("Sea").GetNode<AnimationPlayer>("AnimationPlayer").Play();
        }
    }
}



