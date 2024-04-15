using Godot;
using System;
using System.Collections.Generic;

public class House : Spatial
{
	[Export]
	bool spawnItems = true;
	[Export]
	public string[] ItemSpawnPool;

	List<Furniture> FurnitureList = new List<Furniture>();

	public override void _Ready()
	{
		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				FurnitureList.Insert(FurnitureList.Count, (Furniture)nd);
		}
	}
	public void StartHouse(Random random)
	{
		if (!spawnItems)
			return;
		if (ItemSpawnPool == null)
			return;

		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				FurnitureList.Insert(FurnitureList.Count, (Furniture)nd);
		}
		for (int i = 0; i < FurnitureList.Count; i++)
		{
			int start = random.Next(0, ItemSpawnPool.Length + 1);
			if (start >= ItemSpawnPool.Length)
				continue;

			FurnitureList[i].SpawnItem(ItemSpawnPool[start]);
		}
	}
	public void GetFurniture(out List<Furniture> furniture)
	{
		furniture = new List<Furniture>();
		for (int i = 0; i < FurnitureList.Count; i++)
		{
			furniture.Insert(i, FurnitureList[i]);
		}
	}
	public void InputData(HouseInfo data)
	{
		foreach (Furniture furni in FurnitureList)
		{
			foreach(FurnitureInfo Finfo in data.furni)
			{
				if (furni.Name == Finfo.FunritureName)
				{
					furni.SetData(Finfo);
				}
			}
		}
	}
	/*public bool GetIsEmpty()
	{
		return m_bEmpty;
	}
	private void OnDoorKnocked(HouseDoor door)
	{
		if (!m_bEmpty && lines != null)
		{
			DiagPan.DoDialogue(lines[0], this, true);
		}
		if (m_bEmpty && EmptyLines != null)
		{
			DiagPan.DoDialogue(EmptyLines, this, true);
		}
	}
	public bool Search(Character Looter)
	{
		if (!HouseInventory.IsEmpty())
		{
			var items = new List<Item>();
			HouseInventory.GetContents(out items);
			for (int i = 0; i < items.Count; i++)
			{
				Looter.GetCharacterInventory().InsertItem(items[i]);
			}
			return true;
		}
		return false;
	}*/
}
