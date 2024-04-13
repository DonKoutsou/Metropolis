using Godot;
using System;
using System.Collections.Generic;

public class House : Spatial
{
	[Export]
	bool spawnItems = true;
	[Export]
	public string[] ItemSpawnPool;

	public override void _Ready()
	{
		if (!spawnItems)
			return;
		if (ItemSpawnPool == null)
			return;
		Random random = new Random();

		List<Furniture> furni = new List<Furniture>();
		foreach (Node nd in GetChildren())
		{
			if (nd is Furniture)
				furni.Insert(furni.Count, (Furniture)nd);
		}
		for (int i = 0; i < furni.Count; i++)
		{
			int start = random.Next(0, ItemSpawnPool.Length + 1);
			if (start >= ItemSpawnPool.Length)
				continue;

			furni[i].SpawnItem(ItemSpawnPool[start]);
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
