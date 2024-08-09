using Godot;
using System;

public class MapItem : Item
{

    public override void OnItemPickedUp()
    {
        base.OnItemPickedUp();
       ((MapUI)PlayerUI.GetInstance().GetUI(PlayerUIType.MAP)).GetGrid().ForceIslandVisited(WorldMap.GetInstance().GetCurrentIleInfo());
    }
}
