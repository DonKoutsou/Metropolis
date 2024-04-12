using Godot;
using System;
using System.Collections.Generic;

public class House : Spatial
{
	[Export]
	bool spawnItems = true;
	[Export]
	public string[] ItemSpawnPool;

	Item Item;
	public override void _Ready()
	{
		if (!spawnItems)
			return;
		if (ItemSpawnPool == null)
			return;
		Random random = new Random();
		int start2 = random.Next(0, ItemSpawnPool.Length + 1);
		if (start2 >= ItemSpawnPool.Length)
			return;
		var itemToSpawn = GD.Load<PackedScene>(ItemSpawnPool[start2]);

		Item itemToDrop = (Item)itemToSpawn.Instance();
		AddChild(itemToDrop);
		itemToDrop.GlobalTranslation = GetNode<Position3D>("ItemSpawnPos").GlobalTransform.origin;
		Item = itemToDrop;

	}
	public bool HasItem()
	{
		return Item != null;
	}
	public void OnItemPicked()
	{
		Item = null;
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
