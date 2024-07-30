using Godot;
using System;

public class PlayerUI : Control
{
    [Export]
    NodePath CityNameUI = null;
    [Export]
    NodePath ScreenEffects = null;
    [Export]
    NodePath CheatMenu = null;
    //[Export]
    //NodePath JobBoard = null;   
    [Export]
    NodePath Inventory = null;

    static PlayerUI instance;

    static int OpenMenus = 0;
    public static void OnMenuToggled(bool t)
    {
        if (t)
            OpenMenus ++;
        else
            OpenMenus --;
    }
    public override void _Ready()
    {
        instance = this;

    }
    public static PlayerUI GetInstance()
    {
        return instance;
    }
    public Node GetUI(PlayerUIType type)
    {
        Node UIToReturn = null;
        if (type == PlayerUIType.CITYNAME)
            UIToReturn = GetNode(CityNameUI);
        else if (type == PlayerUIType.SCREENEFFECTS)
            UIToReturn = GetNode(ScreenEffects);
        else if (type == PlayerUIType.CHEATMENU)
            UIToReturn = GetNode(CheatMenu);
        //else if (type == PlayerUIType.JOBBOARD)
            //UIToReturn = GetNode(JobBoard);
        else if (type == PlayerUIType.INVENTORY)
            UIToReturn = GetNode(Inventory);

        return UIToReturn;
    }
    public bool HasMenuOpen()
    {
        //return GetNode<InventoryUI>(Inventory).IsOpen || GetNode<JobBoard>(JobBoard).IsOpen();
        //return GetNode<InventoryUI>(Inventory).IsOpen || Tutorial.IsRunning();
        return OpenMenus > 0;
    }
    public void OnPlayerSpawned(Player pl)
    {
        Show();
        GetNode(Inventory).CallDeferred("OnPlayerSpawned", pl);
        GetNode(CheatMenu).CallDeferred("OnPlayerSpawned", pl);
    }
}
public enum PlayerUIType
{
    CITYNAME,
    //JOBBOARD,
    SCREENEFFECTS,
    CHEATMENU,
    INVENTORY
}
