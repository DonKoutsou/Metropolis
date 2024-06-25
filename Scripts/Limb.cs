using Godot;
using System;
using System.Linq;

[Tool]
public class Limb : Item
{
    [Export]
    LimbType Type = 0;
    [Export]
    bool RandomiseColor = false;
    [Export]
    Color LimbColor = new Color(1,1,1);

    public bool Equiped = false;

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
    public override void _Ready()
    {
        if(Engine.EditorHint)
            return;
        base._Ready();
        if (RandomiseColor)
        {
            LimbColor = LimbRandomColorProvider.GetRandomColor();
            ((GradientTexture)((SpatialMaterial)GetNode<MeshInstance>("MeshInstance").GetActiveMaterial(0)).DetailAlbedo).Gradient.SetColor(0, LimbColor);
        }
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
}
static public class LimbRandomColorProvider
{
    static Color[] Colors = {
        new Color(0.670588f, 0.05098f, 0.05098f,1),
        new Color(0.05098f, 0.532629f, 0.670588f,1),
        new Color(0.876953f, 0.834666f, 0.044227f,1),
        new Color(0.333729f, 0.876953f, 0.044227f,1),
        new Color(0.044227f, 0.145065f, 0.876953f,1),

    };
    static Random rand = new Random();

    public static Color GetRandomColor()
    {
        return Colors[rand.Next(0, Colors.Count())];
    }
}