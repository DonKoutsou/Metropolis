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
    [Export]
    NodePath MapUI = null;
    [Export]
    NodePath ControlsUI = null;
    [Export]
    NodePath ActionMenu = null;
    [Export]
    PackedScene InitialTutorialScene = null;

    static PlayerUI instance;

    static int OpenMenus = 0;

    Player Play;

    public static void OnMenuToggled(bool t)
    {
        if (t)
            OpenMenus ++;
        else
            OpenMenus --;
    }
    public void PlayTutorial(int index)
    {
        if (index == 0)
        {
            AddChild(InitialTutorialScene.Instance());
        }
    }
    public override void _Ready()
    {
        instance = this;
        SetProcessInput(false);
    }
    public void OnPlayerDisconnected()
    {
        SetProcessInput(false);
        MapUI map = (MapUI)GetUI(PlayerUIType.MAP);
            
        if (map.IsOpen)
            map.ToggleMap(false);

        InventoryUI inv = (InventoryUI)GetUI(PlayerUIType.INVENTORY);
        if (inv.IsOpen)
            inv.CloseInventory();

        Control_UI ui = (Control_UI)GetUI(PlayerUIType.CONTROLS);
        ui.DissableUI();

        ActionMenu AMenu = (ActionMenu)GetUI(PlayerUIType.ACTION_MENU);
        AMenu.DissconnectPlayer();
        
    }
    public static PlayerUI GetInstance()
    {
        return instance;
    }
    public Control GetUI(PlayerUIType type)
    {
        Control UIToReturn = null;
        if (type == PlayerUIType.CITYNAME)
            UIToReturn = GetNode<Control>(CityNameUI);
        else if (type == PlayerUIType.SCREENEFFECTS)
            UIToReturn = GetNode<Control>(ScreenEffects);
        else if (type == PlayerUIType.CHEATMENU)
            UIToReturn = GetNode<Control>(CheatMenu);
        //else if (type == PlayerUIType.JOBBOARD)
            //UIToReturn = GetNode(JobBoard);
        else if (type == PlayerUIType.INVENTORY)
            UIToReturn = GetNode<Control>(Inventory);
        else if (type == PlayerUIType.MAP)
            UIToReturn = GetNode<Control>(MapUI);
        else if (type == PlayerUIType.CONTROLS)
            UIToReturn = GetNode<Control>(ControlsUI);
        else if (type == PlayerUIType.ACTION_MENU)
            UIToReturn = GetNode<Control>(ActionMenu);

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
        Play = pl;
        SetProcessInput(true);
        Show();

        GetNode(Inventory).CallDeferred("ConnectPlayer", pl);
        GetNode(CheatMenu).CallDeferred("ConnectPlayer", pl);
        GetNode(MapUI).CallDeferred("ConnectPlayer", pl);
        GetNode(ActionMenu).CallDeferred("ConnectPlayer", pl);
        GetNode(ControlsUI).CallDeferred("EnableUI");
    }
    public override void _Input(InputEvent @event)
	{
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
                bool hasmap = Play.GetCharacterInventory().HasItemOfType(ItemName.MAP);
                if (!hasmap)
                {
                    DialogueManager.GetInstance().ForceDialogue(Play, LocalisationHolder.GetString("Δεν έχω χάρτη..."));
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
}
