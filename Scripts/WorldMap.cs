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

    [Export]
    PackedScene IntroScene = null;
    [Signal]
    public delegate void OnTransitionEventHandler(Island ile);

    public int currentile;

    List <float> rots = new List<float>{0f, 90f, 180f, -90f};


    //id of cells to be changed to events
    List <int> RandomisedEntryID = null;

    //id of exit
    int ExitID = 0;

    List <Vector2> OrderedCells;

    Random random;

    Vector2 CurrentTile;

    static Dictionary<Vector2, IslandInfo> ilemap = new Dictionary<Vector2, IslandInfo>();

    bool finishedspawning = false;

    IslandInfo entry;

    static WorldMap Instance;

    public Player pl;

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
        ulong ms = OS.GetSystemTimeMsecs();
        ArrangeCellsBasedOnDistance();
        ulong msaf = OS.GetSystemTimeMsecs();
        GD.Print("Aranging cells took : " + (msaf - ms).ToString() + " ms");
        for (int i = 0; i < scenestospawn.Count(); i++)
        {
            loadedscenes.Insert(i, scenestospawn[i]);
        }
        ms = OS.GetSystemTimeMsecs();
        MapGrid.GetInstance().InitMap();
        msaf = OS.GetSystemTimeMsecs();
        GD.Print("Initialising map grid took : " + (msaf - ms).ToString() + " ms"); ;
    }
    public Vector2 GetCurrentTile()
    {
        return WorldToMap(CurrentTile);
    }

    IslandInfo IleToSave;
    public override void _Process(float delta)
    {
        //ulong ms = OS.GetSystemTimeMsecs();
        if (!finishedspawning)
        {
            if (IleToSave == null)
            {
                IleToSave = GenerateIsland();
            }
            else
            {
                SaveIsland();
            }
            if (IleToSave == null)
            {
                if (currentile == 300)
                {
                    SpawnIntro();
                }
                currentile += 1;
                if (currentile >= OrderedCells.Count)
                    finishedspawning = true;
            }
            
        }
        
        CheckForTransition();
        //ulong msaf = OS.GetSystemTimeMsecs();
        //if (msaf - ms > 10)
            //GD.Print("World map processing took longer the 10 ms. Process time : " + (msaf - ms).ToString() + " ms. Island scene =  " + scene);
    }
    //spawning and
    IslandInfo GenerateIsland()
    {
        ulong ms = OS.GetSystemTimeMsecs();

        //Get Cell id
        int id = GetCell((int)OrderedCells[currentile].x, (int)OrderedCells[currentile].y);
        //Cell cords
        Vector2 cell = OrderedCells[currentile];

        //Scene using ID from cell
        PackedScene ilescene = GetSceneToSpawn(id);

        //Start the Data saving
        IslandInfo ileinfo = new IslandInfo();
        ilemap.Add(OrderedCells[currentile], ileinfo);
        //Spawndata to be used when spawning
        float rot;
        int index = random.Next(rots.Count);
        rot = rots[index];

        ileinfo.rottospawn = rot;
        ileinfo.IleType = ilescene;
        ileinfo.pos = cell;
        
        SpawnIsland(ileinfo);   

        //SaveEntry
        if (id == 0)
            entry = ileinfo;


        ulong msaf = OS.GetSystemTimeMsecs();
        if (msaf - ms > 10)
            GD.Print("Island Generated. Process time : " + (msaf - ms).ToString() + " ms. Island scene =  " + ilescene.ResourcePath);

        return ileinfo;
    }
    public void SaveIsland()
    {
        //ulong ms = OS.GetSystemTimeMsecs();

        Island ile = IleToSave.ile;

        ile.InitialSpawn(random);

        IleToSave.SetInfo(ile);

        ile.QueueFree();

        MapGrid.GetInstance().UpdateIleInfo(IleToSave.pos, IleToSave.Type);

        IleToSave = null;

        //ulong msaf = OS.GetSystemTimeMsecs();
        //GD.Print("Island Saved. Process time : " + (msaf - ms).ToString() + " ms");
    }
    public void SpawnIntro()
    {
        MyWorld.GetInstance().ToggleIsland(entry, true, true);
            
        Island island = entry.ile;

        Intro intr = (Intro)IntroScene.Instance();

        EmitSignal("OnTransitionEventHandler", island);

        island.AddChild(intr);
        
        intr.Translation = Vector3.Zero;
        intr.GlobalRotation = Vector3.Zero;

        CurrentTile = new Vector2 (island.GlobalTransform.origin.x ,island.GlobalTransform.origin.z);

        //List<IslandInfo> closestto2;
        //GetClosestIles(entry,out closestto2, 2);
        //foreach(IslandInfo info in closestto2)
           //MapGrid.GetInstance().OnIslandVisited(info);
    }
    private void CheckForTransition()
    {
        if (pl == null)
            return;
        Vector2 plpos = new Vector2(pl.GlobalTransform.origin.x, pl.GlobalTransform.origin.z);
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
                if (pl.HasVehicle())
                {
                    Vehicle veh = pl.currveh;
                    veh.ReparentVehicle(ileinfto.ile);
                    ileinfto.AddNewVehicle(veh);
                }
                
                MyWorld.IleTransition(ileinf, ileinfto);
                
                EmitSignal("OnTransitionEventHandler", ileinfto.ile);
            }
        }
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

        info.ile = Ile;

        MyWorld.GetInstance().AddChild(Ile);
        
        
        return Ile;
    }
    public Island ReSpawnIsland(IslandInfo info)
    {
        Island Ile = (Island)info.IleType.Instance();

        Vector3 pos;

        Vector2 postoput = MapToWorld(new Vector2(info.pos.x, info.pos.y));

        postoput += CellSize / 2;
        pos.x = postoput.x;
        pos.z = postoput.y;
        pos.y = 0.0f;
        Ile.SetSpawnInfo(pos, info.rottospawn);

        info.ile = Ile;

        MyWorld.GetInstance().AddChild(Ile);
        //Ile.InputData(info);
        return Ile;
    }
    void ArrangeCellsBasedOnDistance()
    {
        //arange all the cells by distance from center of world put in OrderedCells array
        var cells = GetUsedCells();
        OrderedCells = new List<Vector2>(cells.Count);
        OrderedCells.Insert(0, (Vector2)cells[0]);
        List<int> absolutesums = new List<int>(cells.Count);
        absolutesums.Add((int)(Math.Abs(OrderedCells[0].x) + Math.Abs(OrderedCells[0].y)));

        for (int x = 1; x < cells.Count; x++)
        {
            Vector2 cellArray = (Vector2)cells[x];
            int ind = (int)(Math.Abs(cellArray.x) + Math.Abs(cellArray.y));

            //Vector2 closest = OrderedCells[0];
            int closestind = Math.Abs(absolutesums.BinarySearch(ind)) - 1;
            closestind = Math.Max(0, closestind);
            OrderedCells.Insert(closestind, cellArray);

            absolutesums.Insert(closestind, ind);
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
        ExitID = OrderedCells.IndexOf(Exitpalcement);
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
        float animstage = Poseidon.GetInstance().GetAnimStage();
        foreach (Node sea in seas)
        {
            sea.GetNode<AnimationPlayer>("AnimationPlayer").Seek(animstage);
            sea.GetNode<AnimationPlayer>("AnimationPlayer").Play();
        }
    }
}



