using Godot;
using System;
using System.Collections.Generic;
/////////////////////////////////////////////////////////////////////////////////
/*
███████╗ ██████╗ ██╗   ██╗███╗   ██╗██████╗     ███╗   ███╗ █████╗ ██████╗ 
██╔════╝██╔═══██╗██║   ██║████╗  ██║██╔══██╗    ████╗ ████║██╔══██╗██╔══██╗
███████╗██║   ██║██║   ██║██╔██╗ ██║██║  ██║    ██╔████╔██║███████║██████╔╝
╚════██║██║   ██║██║   ██║██║╚██╗██║██║  ██║    ██║╚██╔╝██║██╔══██║██╔═══╝ 
███████║╚██████╔╝╚██████╔╝██║ ╚████║██████╔╝    ██║ ╚═╝ ██║██║  ██║██║     
╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═════╝     ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝                                                                               
*/
/////////////////////////////////////////////////////////////////////////////////
[Tool]
public class SoundMap : GridMap
{
    [Export]
    List<PackedScene> SoundScenes = null;

    List<float> SampleLenghts = new List<float>();

    RandomNumberGenerator SeekRandom;

    

    [Export(PropertyHint.Layers3dPhysics)]
    public uint CheckLayer { get; set; }

    [Export]
    int MapScale = 12000;

    //[Export(PropertyHint.Layers3dPhysics)]
    //public uint SeaLayer { get; set; }

    Vector3 currenttile = new Vector3(0,0,0);

    float scale;

    Dictionary<Vector3, Spatial> Sounds = new Dictionary<Vector3, Spatial>();

    Position3D PlLoc;

    Godot.Collections.Array cells;
    public override void _Ready()
    {
        if (!Engine.EditorHint)
        {
            CylinderMesh mesh = new CylinderMesh(){TopRadius = 0, BottomRadius = 0};

            MeshLibrary.SetItemMesh(0, mesh);
            PlLoc = GetNode<Position3D>("PlayerLocator");

            Settings set = Settings.GetGameSettings();
            if (set != null)
            {
                 SeekRandom = new RandomNumberGenerator(){
                Seed = (ulong)set.Seed
                };
            }
           

            for (int i = 0; i < SoundScenes.Count; i++)
            {

                Spatial sound = SoundScenes[i].Instance<Spatial>();

                SampleLenghts.Add(sound.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream.GetLength());

                AddChild(sound);

                Vector3 offset = GlobalTranslation;
                offset.y -= 1000;
                sound.GlobalTranslation = offset;

                sound.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
            }
            scale = MapScale / CellSize.x;
            cells = GetUsedCells();
        }
        
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        if (!Engine.EditorHint)
            Visible = false;
        else
             Visible = true;
    }
    public void SpawnSoundAt(int sound, Vector3 at)
    {
        Spatial SoundScene = SoundScenes[sound].Instance<Spatial>();
            
        AddChild(SoundScene);

        SoundScene.Translation = MapToWorld((int)at.x, (int)at.y, (int)at.z);

        Sounds.Add(at, SoundScene);
        
        SoundScene.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Seek(SeekRandom.RandfRange(0, SampleLenghts[sound]));

        SoundScene.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
    }
    public void DespawnSoundAt(Vector3 at)
    {
        Spatial sound = Sounds[at];

        Sounds.Remove(at);

        sound.Free();
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Player pl = Player.GetInstance();

        if (pl == null)
            return;
        
        PlLoc.GlobalTranslation = pl.GlobalTranslation;

        Vector3 plpos = PlLoc.Translation;

        plpos.y = 0;

        Vector3 plposMap = WorldToMap(plpos);

        if (plposMap == currenttile)
        {
            return;
        }
        currenttile = plposMap;

        if (Mathf.Abs(plposMap.x) > scale/2 || Mathf.Abs(plposMap.z) > scale/2)
            ClearChildren();

        List <Vector3> Toremove = new List<Vector3>();
        foreach (KeyValuePair<Vector3, Spatial> sound in Sounds)
        {
            if (plposMap.DistanceTo(sound.Key) > 5)
            {
                Toremove.Add(sound.Key);
            }
        }
        foreach (Vector3 Cell in Toremove)
        {
            DespawnSoundAt(Cell);
        }
        Vector3 Checking = new Vector3(-4, 0 ,-4);
        for (int r = 0; r < 81; r++)
        {
            if (Checking.x >= 4)
            {
                Checking.x = -4;
                Checking.z += 1;
            }
            else
            {
                Checking.x += 1;
            }
            Vector3 postocheck = plposMap + Checking;
            if (cells.Contains(postocheck))
            {
                if (!Sounds.ContainsKey(postocheck))
                    SpawnSoundAt(GetCellItem((int)postocheck.x, (int)postocheck.y, (int)postocheck.z) ,postocheck);
            }
        }
        //foreach (KeyValuePair<Vector3, Spatial> sound in Sounds)
        //{
        //    AudioStreamPlayer3D player = sound.Value.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        //    if (!player.Playing)
        //        player.Play();
        //}
    }
    #if DEBUG
    [Export]
    bool RedoMapping = false;
    public override void _Process(float delta)
    {
        base._Process(delta);
        
        if (Engine.EditorHint)
        {
            
            if (RedoMapping)
            {
                Clear();
                scale = MapScale / CellSize.x;
                Vector3 startingpoint = new Vector3(- (scale / 2), 0, - (scale / 2));

                for (int i = 0; i < scale * scale; i ++)
                {
                    //bool HasGround = false;
                    //bool HasSea = false;

                    bool IsAboveSeaLevel = false;
                    bool IsBellowSeaLevel = false;
                    Vector3 global = MapToWorld((int)startingpoint.x, (int)startingpoint.y, (int)startingpoint.z);
                    
                    global.y += 2000;
                    global.x -= CellSize.x/2;
                    global.z -= CellSize.z/2;
                    var spaceState = GetWorld().DirectSpaceState;
                    

                    Vector3 RayTo = global;
                    RayTo.y -= 2500;

                    Vector3 Checking = new Vector3(-1, 0 ,-1);
                    for (int r = 0; r < 9; r++)
                    {
                        var Groundresult = spaceState.IntersectRay(global + (Checking * (CellSize / 2)), RayTo + (Checking * (CellSize / 2)), null, CheckLayer);
                        

                        if (Checking.x >= 1)
                        {
                            Checking.x = -1;
                            Checking.z += 1;
                        }
                        else
                        {
                            Checking.x += 1;
                        }

                        if (Groundresult.Count == 0)
                        {
                            IsBellowSeaLevel = true;
                            continue;
                        }
                            

                        Vector3 pos = (Vector3)Groundresult["position"];
                        
                        if (pos.y >= 0)
                            IsAboveSeaLevel = true;
                        else
                            IsBellowSeaLevel = true;

                        //var Searesult = spaceState.IntersectRay(global + (Checking * (CellSize / 2)), RayTo + (Checking * (CellSize / 2)), null, SeaLayer);

                        /*bool ItsSea = ((CollisionObject)Groundresult["collider"]).GetCollisionLayerBit(8);
                        if (ItsSea)
                        {
                            HasSea = true;
                        }
                        else
                        {
                            HasGround = true;
                        }
                        //var SeaResault = spaceState.IntersectRay(global + (OffsetsToCheck[r] * CellSize), RayTo + (OffsetsToCheck[r] * CellSize), null, SeaLayer);

                        if (HasGround && HasSea)
                            break;*/
                    }

                    //if (HasGround && HasSea)
                    if (IsBellowSeaLevel && IsAboveSeaLevel)
                        SetCellItem((int)startingpoint.x, (int)startingpoint.y, (int)startingpoint.z, 0);
                    else
                        SetCellItem((int)startingpoint.x, (int)startingpoint.y, (int)startingpoint.z, InvalidCellItem);
                    
                    if (startingpoint.x >= MapScale/ 100)
                    {
                        startingpoint.x = -MapScale/100;
                        startingpoint.z += 1;
                    }
                    else
                        startingpoint.x += 1;
                    
                }


                RedoMapping = false;
            }
        }
    }
    #endif
    private void ClearChildren()
    {
         foreach (KeyValuePair<Vector3, Spatial> sound in Sounds)
        {
            sound.Value.Free();
        }
        Sounds = new Dictionary<Vector3, Spatial>();
    }
}
