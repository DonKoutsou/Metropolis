using Godot;
using System;

public class PlayerUI : Spatial
{
    [Export]
    NodePath CityNameUI = null;
    [Export]
    NodePath TalkText = null;
    [Export]
    NodePath ActionMenu = null;
    [Export]
    NodePath ScreenEffects = null;
    [Export]
    NodePath CheatMenu = null;
    static PlayerUI instance;
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
        if (type == PlayerUIType.TALKTEXT)
            UIToReturn = GetNode(TalkText);
        if (type == PlayerUIType.ACTIONMENU)
            UIToReturn = GetNode(ActionMenu);
        if (type == PlayerUIType.SCREENEFFECTS)
            UIToReturn = GetNode(ScreenEffects);
        if (type == PlayerUIType.CHEATMENU)
            UIToReturn = GetNode(CheatMenu);

        return UIToReturn;
    }
}
public enum PlayerUIType
{
    CITYNAME,
    TALKTEXT,
    ACTIONMENU,
    SCREENEFFECTS,
    CHEATMENU,
}
