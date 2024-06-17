using Godot;
using System;

public class Limb : Item
{
    [Export]
    LimbType Type = 0;

    public bool Equiped = false;
    public override void _Ready()
    {
        
    }
    public LimbType GetLimbType()
    {
        return Type;
    }
}
public enum LimbType
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
        }
        return thing;
    }
}