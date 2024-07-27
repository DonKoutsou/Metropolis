using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public class Limb : Item
{
    [Export]
    LimbType Type = 0;
    [Export]
    LimbSlotType SlotType = 0;
    [Export]
    public bool RandomiseColor = false;
    [Export]
    Color LimbColor = new Color(1,1,1);

    //public bool Equiped = false;
    public override string GetInventoryItemName()
	{
		return string.Format(InventoryItemName, LimbRandomColorProvider.TranslateColor(LimbColor));
	}
    public void SetColor(Color col)
    {
        LimbColor = col;
    }
    public Color GetColor()
    {
        return LimbColor;
    }
    public LimbType GetLimbType()
    {
        return Type;
    }
    public LimbSlotType GetSlotType()
    {
        return SlotType;
    }
    public override void _Ready()
    {
        if(Engine.EditorHint)
            return;
        base._Ready();
        if (RandomiseColor)
        {
            LimbColor = LimbRandomColorProvider.GetRandomColor();
        }
        ((GradientTexture)((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, LimbColor);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(!Engine.EditorHint)
            return;
        
        ((GradientTexture)((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, LimbColor);
    }
}
public enum LimbType
{
    ARM_L,
    ARM_R,
    LEG_L,
    LEG_R,
    N01_LEG_L,
    N01_LEG_R,
}
public enum LimbSlotType
{
    ARM_L,
    ARM_R,
    LEG_L,
    LEG_R
}
static public class LimbTranslator
{
    public static string EnumToString(LimbType type)
    {
        string thing = string.Empty;
        switch(type)
        {
            case LimbType.ARM_L:
            {
                thing = "Arm_L";
                break;
            }
            case LimbType.ARM_R:
            {
                thing = "Arm_R";
                break;
            }
            case LimbType.LEG_L:
            {
                thing = "Leg_L";
                break;
            }
            case LimbType.LEG_R:
            {
                thing = "Leg_R";
                break;
            }
            case LimbType.N01_LEG_L:
            {
                thing = "Leg2_L";
                break;
            }
            case LimbType.N01_LEG_R:
            {
                thing = "Leg2_R";
                break;
            }
        }
        return thing;
    }
    public static Godot.Collections.Dictionary<string, object> GetLimbEffect(LimbType type, bool toggle)
    {
        Godot.Collections.Dictionary<string, object> effects = new Godot.Collections.Dictionary<string, object>();
        switch(type)
        {
            case LimbType.ARM_L:
            {
                break;
            }
            case LimbType.ARM_R:
            {
                break;
            }
            case LimbType.LEG_L:
            {
                effects.Add("RunSpeed", 1.6);
                break;
            }
            case LimbType.LEG_R:
            {
                effects.Add("RunSpeed", 1.6);
                break;
            }
            case LimbType.N01_LEG_L:
            {
                break;
            }
            case LimbType.N01_LEG_R:
            {
                break;
            }
        }
        return effects;
    }
}
static public class LimbRandomColorProvider
{
    static Color[] Colors = {
        new Color(0.67f, 0.05f, 0.05f,1),
        new Color(0.05f, 0.53f, 0.67f,1),
        new Color(0.87f, 0.83f, 0.04f,1),
        new Color(0.33f, 0.87f, 0.04f,1),
        new Color(0.04f, 0.14f, 0.87f,1),
        new Color(0, 0, 0, 1),
        new Color(1, 1, 1, 1),
    };
    public static string TranslateColor(Color c)
    {
        string ColorName = string.Empty;

        if (c == new Color(0.67f, 0.05f, 0.05f,1))
        {
            ColorName = "Κόκκινο";
        }
        else if (c == new Color(0.05f, 0.53f, 0.67f,1))
        {
            ColorName = "Ανοιχτό μπλε";
        }
        else if (c == new Color(0.87f, 0.83f, 0.04f,1))
        {
            ColorName = "Κίτρινο";
        }
        else if (c == new Color(0.33f, 0.87f, 0.04f,1))
        {
            ColorName = "Πράσινο";
        }
        else if (c == new Color(0.04f, 0.14f, 0.87f,1))
        {
            ColorName = "Μπλέ";
        }
        else if (c == new Color(0, 0, 0, 1))
        {
            ColorName = "Μαύρο";
        }
        else if (c == new Color(1, 1, 1, 1))
        {
            ColorName = "Λευκό";
        }

        return ColorName;
    }
    public static Color GetRandomColor()
    {
        return Colors[RandomContainer.Next(0, Colors.Count())];
    }
}
