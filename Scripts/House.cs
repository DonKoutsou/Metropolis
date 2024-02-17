using Godot;
using System;
using System.Collections.Generic;

public class House : Spatial
{
    [Export]
    public string[] ItemSpawnPool;

    [Export]
	public DialogueLine[] lines;

    [Export]
	public DialogueLine EmptyLines;


    //bool m_bEmpty;

    //Inventory HouseInventory;

    //DialoguePanel DiagPan;

    public NavigationMeshInstance navmesh;
    public override void _Ready()
    {
        /*HouseInventory = GetNode<Inventory>("Inventory");
        var panels = GetTree().GetNodesInGroup("DialoguePanel");
		DiagPan = (DialoguePanel)panels[0];

        var Random = new Random();
        m_bEmpty = Random.Next(2) == 1;
        //m_bEmpty = true;
        if (ItemSpawnPool == null)
            return;
        Random random = new Random();
        int start2 = random.Next(0, ItemSpawnPool.Length);
        var itemToSpawn = GD.Load<PackedScene>(ItemSpawnPool[start2]);
        if (itemToSpawn == null)
            return;
        itemToDrop = (Item)itemToSpawn.Instance();
        HouseInventory.InsertItem(itemToDrop);*/
        //navmesh = GetNode<NavigationMeshInstance>("NavigationMeshInstance");
        
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
