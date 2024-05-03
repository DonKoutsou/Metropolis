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

	List<WindGenerator> Generators = new List<WindGenerator>();
	
	public override void _Ready()
	{
		GlobalTranslation = SpawnGlobalLocation;

		Rotate(new Vector3(0, 1, 0), Mathf.Deg2Rad(SpawnRotation));
		Transform.Rotated(new Vector3(0, 1, 0), SpawnRotation);

		StaticBody waterbody = GetNodeOrNull<StaticBody>("SeaBed");
		if (waterbody != null)
			waterbody.GlobalRotation = new Vector3 (0.0f, 0.0f, 0.0f);
		FindHouses(this);
		FindGenerators(this);
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
	}
	public void InitialSpawn(Random r)
	{
		foreach(House h in Houses)
		{
			h.StartHouse(r);
		}
	}
	private void FindHouses(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is House)
				Houses.Insert(Houses.Count, (House)child);
			else
				FindHouses(child);
		}
	}
	public void GetHouses(out List<House> hs)
	{
		hs = new List<House>();
		for (int i = 0; i < Houses.Count; i++)
		{
			hs.Insert(i, Houses[i]);
		}
	}
	private void FindGenerators(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is WindGenerator)
				Generators.Insert(Generators.Count, (WindGenerator)child);
			else
				FindGenerators(child);
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
	
}
public class IslandInfo
{
    public Island ile;
    public IleType type;
    public Vector2 pos;
    public PackedScene IleType;
    public List<HouseInfo> Houses = new List<HouseInfo>();
    public List<WindGeneratorInfo> Generators = new List<WindGeneratorInfo>();
    public float rottospawn;
    public void SetInfo(Island Ile)
    {
        ile = Ile;
        type = Ile.GetIslandType();
        List<House> hous = new List<House>();
        Ile.GetHouses(out hous);
        List<WindGenerator> Gen = new List<WindGenerator>();
        Ile.GetGenerator(out Gen);
        AddHouses(hous);
        AddGenerators(Gen);
    }
    public void UpdateInfo(Island island)
    {
        List<House> hous = new List<House>();
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
        List<WindGenerator> gens = new List<WindGenerator>();
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
    }
    public void GetInfo(out List<HouseInfo> Houss, out float rot, out PackedScene ilet, out Vector2 position)
    {
        Houss = new List<HouseInfo>();
        for (int i = 0; i < Houses.Count; i ++)
        {
            Houss.Insert(i, Houses[i]);
        }
        rot = rottospawn;
        ilet = IleType;
        position = pos;
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
            Generators.Insert(Houses.Count, info);
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
    public void GetHouses(out List<HouseInfo> GotHouses)
    {
        GotHouses = new List<HouseInfo>();
        for (int i = 0; i < Houses.Count; i++)
        {
            GotHouses.Insert(i, Houses[i]);
        }
    }
}

public class WindGeneratorInfo
{
    public string WindGeneratorName;
    public float CurrentEnergy;
    public void UpdateInfo(WindGenerator gen)
    {
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