using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class Island : Spatial
{
	[Export]
	public bool m_bOriginalIle = false;
	[Export]
	IleType type = IleType.LAND;

	public bool Inited = false;

	private Vector3 SpawnGlobalLocation;

	private float SpawnRotation;

	List<House> Houses = new List<House>();

    List<Vehicle> Vehicles = new List<Vehicle>();

	List<WindGenerator> Generators = new List<WindGenerator>();
	
	public override void _Ready()
	{
		GlobalTranslation = SpawnGlobalLocation;

		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(SpawnRotation));
		//Transform.Rotated(new Vector3(0, 1, 0), SpawnRotation);
		//FindHouses(this);
		//FindGenerators(this);
        //FindVehicles(this);
        GetNode<StaticBody>("SeaBed").GetNode<MeshInstance>("Sea").QueueFree();

        //FindChildren(this);
	}
	public void SetSpawnInfo(Vector3 SpawnPos, float SpawnRot)
	{
		SpawnGlobalLocation = SpawnPos;
		SpawnRotation = SpawnRot;
	}
	public IleType GetIslandType()
	{
		return type;
	}
	public void InputData(IslandInfo data)
	{
		foreach (House hou in Houses)
		{
			foreach(HouseInfo Hnfo in data.Houses)
			{
				if (hou.Name == Hnfo.HouseName)
				{
					hou.InputData(Hnfo);
				}
			}
		}
		foreach (WindGenerator gen in Generators)
		{
			foreach(WindGeneratorInfo GenInfo in data.Generators)
			{
				if (gen.Name == GenInfo.WindGeneratorName)
				{
					gen.SetData(GenInfo);
				}
			}
		}
        List <VehicleInfo> D = new List<VehicleInfo>();

        foreach (Vehicle veh in Vehicles)
		{
            VehicleInfo myinfo = null;
			foreach(VehicleInfo Vnfo in data.Vehicles)
			{
                Spatial par = (Spatial)veh.GetParent();
				if (par.Name == Vnfo.VehName)
				{
                    myinfo = Vnfo;
				}
			}
            if( myinfo != null)
            {
                if (myinfo.removed)
                    veh.GetParent().QueueFree();
                else
                    veh.InputData(myinfo);
                D.Add(myinfo);
            }
            else
                veh.GetParent().QueueFree();
		}
        foreach(VehicleInfo inf in data.Vehicles)
        {
            if (inf.removed)
                continue;
            if (!D.Contains(inf))
            {
                PackedScene vehscene = GD.Load<PackedScene>(inf.scenedata);
                Spatial veh = vehscene.Instance<Spatial>();
                Vehicle V = veh.GetNode<Vehicle>("VehicleBody");
                AddChild(veh);
                V.InputData(inf);
            }
        }
	}
	public void InitialSpawn(Random r)
	{
		foreach(House h in Houses)
		{
			h.StartHouse(r);
		}
	}
    public void RegisterChild(Node child)
    {
        if (child is House)
				Houses.Insert(Houses.Count, (House)child);
        else if (child is WindGenerator)
            Generators.Insert(Generators.Count, (WindGenerator)child);
        else if (child is Vehicle)
            Vehicles.Insert(Vehicles.Count, (Vehicle)child);
    }
    private void FindChildren(Node node)
    {
        //ulong ms = OS.GetSystemTimeMsecs();
		foreach (Node child in node.GetChildren())
		{
			if (child is House)
				Houses.Insert(Houses.Count, (House)child);
            else if (child is WindGenerator)
				Generators.Insert(Generators.Count, (WindGenerator)child);
            else if (child is Vehicle)
				Vehicles.Insert(Vehicles.Count, (Vehicle)child);
			else
				FindChildren(child);
		}
        //ulong msaf = OS.GetSystemTimeMsecs();
        //GD.Print("FoundChildren. Process time : " + (msaf - ms).ToString() + " ms");
	}

	public void GetHouses(out List<House> hs)
	{
		hs = new List<House>();
		for (int i = 0; i < Houses.Count; i++)
		{
			hs.Insert(i, Houses[i]);
		}
	}

	public void GetGenerator(out List<WindGenerator> wg)
	{
		wg = new List<WindGenerator>();
		for (int i = 0; i < Generators.Count; i++)
		{
			wg.Insert(i, Generators[i]);
		}
	}

    public void GetVehicles(out List<Vehicle> vhs)
	{
		vhs = new List<Vehicle>();
		for (int i = 0; i < Vehicles.Count; i++)
		{
            if (Vehicles[i] == null)
                continue;
                
			vhs.Insert(i, Vehicles[i]);
		}
	}
}
public class IslandInfo
{
    public Island ile;
    public IleType Type;
    public Vector2 pos;
    public PackedScene IleType;
    public List<HouseInfo> Houses = new List<HouseInfo>();
    public List<WindGeneratorInfo> Generators = new List<WindGeneratorInfo>();

    public List<VehicleInfo> Vehicles = new List<VehicleInfo>();
    public float rottospawn;
    public void SetInfo(Island Ile)
    {
        ile = Ile;
        Type = Ile.GetIslandType();
        List<House> hous;
        Ile.GetHouses(out hous);
        List<WindGenerator> Gen;
        Ile.GetGenerator(out Gen);
        List<Vehicle> veh;
        Ile.GetVehicles(out veh);
        AddHouses(hous);
        AddGenerators(Gen);
        AddVehicles(veh);
    }
    public void AddNewVehicle(Vehicle veh)
    {
        int vehammount = 0;
        for(int i = 0; i < Vehicles.Count; i++)
        {
            if (!Vehicles[i].removed)
                vehammount += 1;
        }
        veh.GetParent().Name = "Vehicle" + (vehammount + 1).ToString();
        VehicleInfo data = new VehicleInfo();
        data.SetInfo(veh);
        Vehicles.Insert(Vehicles.Count, data);
    }
    public void UpdateInfo(Island island)
    {
        List<House> hous;
        island.GetHouses(out hous);
        foreach(HouseInfo HInfo in Houses)
        {
            House h = null;
            foreach (House hou in hous)
            {
                if (hou.Name == HInfo.HouseName)
                {
                    h = hou;
                    break;
                }
            }
            List<Furniture> funriture;
            h.GetFurniture(out funriture);
            HInfo.UpdateInfo(funriture);
        }
        List<WindGenerator> gens;
        island.GetGenerator(out gens);
        foreach(WindGeneratorInfo WGInfo in Generators)
        {
            WindGenerator g = null;
            foreach (WindGenerator gen in gens)
            {
                if (gen.Name == WGInfo.WindGeneratorName)
                {
                    g = gen;
                    break;
                }
            }
            WGInfo.UpdateInfo(g);
        }
        List<Vehicle> vehs;
        island.GetVehicles(out vehs);
        foreach(VehicleInfo VHInfo in Vehicles)
        {
            Vehicle v = null;
            foreach (Vehicle veh in vehs)
            {
                Spatial par = (Spatial)veh.GetParent();
                if (par.Name == VHInfo.VehName)
                {
                    v = veh;
                    break;
                }
            }
            if (v == null)
            {
                VHInfo.removed = true;
                continue;
            }
            VHInfo.UpdateInfo(v);
        }
    }


    public void AddHouses(List<House> HouseToAdd)
    {
        for (int i = 0; i < HouseToAdd.Count; i++)
        {
            HouseInfo info = new HouseInfo();
            List<Furniture> furni = new List<Furniture>();
            House h = HouseToAdd[i];
            h.GetFurniture(out furni);
            List<FurnitureInfo> finfo = new List<FurnitureInfo>();
            for (int f = 0; f < furni.Count; f++)
            {
                Furniture furn = furni[f];
                FurnitureInfo inf = new FurnitureInfo();
                ItemName itn = 0;
                if (furn.HasItem())
                {
                    itn = furn.GetItemName();
                }
                inf.SetInfo(furn.Name, furn.HasBeenSearched(), furn.HasItem(), itn);
                finfo.Insert(f, inf);
            }
            info.SetInfo(HouseToAdd[i].Name, finfo);
            Houses.Insert(Houses.Count, info);
        }
    }
    public void AddGenerators(List<WindGenerator> GeneratorToAdd)
    {
        for (int i = 0; i < GeneratorToAdd.Count; i++)
        {
            WindGeneratorInfo info = new WindGeneratorInfo();
            info.SetInfo(GeneratorToAdd[i].Name, GeneratorToAdd[i].GetCurrentEnergy());
            Generators.Insert(Generators.Count, info);
        }
    }
    public void AddVehicles(List<Vehicle> VehicleToAdd)
    {
        for (int i = 0; i < VehicleToAdd.Count; i++)
        {
            VehicleInfo info = new VehicleInfo();
            info.SetInfo(VehicleToAdd[i]);
            Vehicles.Insert(Vehicles.Count, info);
        }
    }
    public bool IsIslandSpawned()
    {
        if (ile == null)
            return false;
        else
        {
            return Godot.Object.IsInstanceValid(ile);
        }
        
    }
}

public class WindGeneratorInfo
{
    public string WindGeneratorName;
    public float CurrentEnergy;
    public int DespawnDay;

    public int Despawnhour;
    public int Despawnmins;
    public void UpdateInfo(WindGenerator gen)
    {
        DayNight.GetDay(out DespawnDay);
        DayNight.GetTime(out Despawnhour, out Despawnmins);
        CurrentEnergy = gen.GetCurrentEnergy();
    }
    public void SetInfo(string name, float CurEn)
    {
        WindGeneratorName = name;
        CurrentEnergy = CurEn;
    }
}
public enum IleType
{
	ENTRANCE,
	LAND,
	EXIT,
	SEA,
	LIGHTHOUSE
}