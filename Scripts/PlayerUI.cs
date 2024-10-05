using Godot;
using System;
using System.Collections.Generic;

public class PlayerUI : Control
{
    static Dictionary<string, Control> UIList = new Dictionary<string, Control>();
    static int OpenMenus = 0;
    static bool Working = false;
    public override void _Ready()
    {
        Godot.Collections.Array Children = GetChildren();
        for (int i = 0; i < Children.Count; i++)
        {
            if (Children[i] is Control C)
            {
                UIList.Add(C.Name, C);
            }
        }
    }
    public static void OnMenuToggled(bool t)
    {
        if (t)
            OpenMenus ++;
        else
            OpenMenus --;
    }
    public static void OnPlayerDisconnected()
    {
        Working = false;

        foreach(KeyValuePair<string, Control> UI in UIList)
        {
            UI.Value.CallDeferred("PlayerToggle", null);
        }
    }
    public static Control GetUI(PlayerUIType type)
    {
        return UIList[type.ToString()];;
    }
    public static bool HasMenuOpen()
    {
        //return GetNode<InventoryUI>(Inventory).IsOpen || GetNode<JobBoard>(JobBoard).IsOpen();
        //return GetNode<InventoryUI>(Inventory).IsOpen || Tutorial.IsRunning();
        return OpenMenus > 0;
    }
    public static void OnPlayerSpawned(Player pl)
    {
        Working = true;
        foreach(KeyValuePair<string, Control> UI in UIList)
        {
            if (UI.Value.HasMethod("PlayerToggle"))
                UI.Value.CallDeferred("PlayerToggle", pl);
        }
    }
    public override void _Input(InputEvent @event)
	{
        if (!Working)
            return;
		if (@event.IsActionPressed("Inventory"))
		{
            
			InventoryUI inv = (InventoryUI)GetUI(PlayerUIType.INVENTORY);
			if (inv.IsOpen)
				inv.CloseInventory();
			else
				inv.OpenInventory();
		}
        else if (@event.IsActionPressed("Map"))
        {
            MapUI map = (MapUI)GetUI(PlayerUIType.MAP);
            
			if (map.IsOpen)
				map.ToggleMap(false);
			else
            {
                bool hasmap = Player.GetInstance().GetCharacterInventory().HasItemOfType(ItemName.MAP);
                if (!hasmap)
                {
                    DialogueManager.GetInstance().ForceDialogue(Player.GetInstance(), "Δεν έχω χάρτη...");
                    //Play.GetTalkText().Talk("Δεν έχω χάρτη...");
                }
                else
                    map.ToggleMap(true);
            }
        }
	}
}
public enum PlayerUIType
{
    CITYNAME,
    //JOBBOARD,
    SCREENEFFECTS,
    CHEATMENU,
    INVENTORY,
    MAP,
    CONTROLS,
    ACTION_MENU,
    PUZZLE,
    TUTORIAL,
    ACHIEVEMENT_NOTIFICATION,
}