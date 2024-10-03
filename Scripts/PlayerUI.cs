using Godot;
using System;
using System.Collections.Generic;

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
    [Export]
    NodePath MapUI = null;
    [Export]
    NodePath ControlsUI = null;
    [Export]
    NodePath ActionManager = null;
    [Export]
    NodePath PuzzleManager = null;
    [Export]
    NodePath TutorialManager = null;
    [Export]
    NodePath AchievementManager = null;
    static Dictionary<string, Control> UIList = new Dictionary<string, Control>();
    static int OpenMenus = 0;
    static bool Working = false;
    public override void _Ready()
    {
        UIList.Add(PlayerUIType.ACHIEVEMENT.ToString(), GetNode<Control>(AchievementManager));
        UIList.Add(PlayerUIType.ACTION_MENU.ToString(), GetNode<Control>(ActionManager) );
        UIList.Add(PlayerUIType.CHEATMENU.ToString(), GetNode<Control>(CheatMenu));
        UIList.Add(PlayerUIType.CITYNAME.ToString(), GetNode<Control>(CityNameUI));
        UIList.Add(PlayerUIType.CONTROLS.ToString(), GetNode<Control>(ControlsUI));
        UIList.Add(PlayerUIType.INVENTORY.ToString(), GetNode<Control>(Inventory));
        UIList.Add(PlayerUIType.MAP.ToString(), GetNode<Control>(MapUI));
        UIList.Add(PlayerUIType.PUZZLE.ToString(), GetNode<Control>(PuzzleManager));
        UIList.Add(PlayerUIType.SCREENEFFECTS.ToString(), GetNode<Control>(ScreenEffects));
        UIList.Add(PlayerUIType.TUTORIAL.ToString(), GetNode<Control>(TutorialManager));
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
    ACHIEVEMENT,
}