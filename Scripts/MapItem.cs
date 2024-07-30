using Godot;
using System;

public class MapItem : Item
{

    public override void OnItemPickedUp()
    {
        base.OnItemPickedUp();
        MapGrid.GetInstance().SetIslandVisited(WorldMap.GetInstance().GetCurrentIleInfo());
    }
}
