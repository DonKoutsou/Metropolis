using Godot;
using System;

public class MapItem : Item
{
    public override void OnItemPickedUp()
    {
        base.OnItemPickedUp();
        WorldMap map = WorldMap.GetInstance();
        if (map != null)
            ((MapUI)PlayerUI.GetInstance().GetUI(PlayerUIType.MAP)).GetGrid().ForceIslandVisited(map.GetCurrentIleInfo());
    }
}
