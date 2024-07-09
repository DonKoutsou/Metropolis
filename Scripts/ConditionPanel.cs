using Godot;
using System;
using System.Linq;

public class ConditionPanel : Panel
{
    Control[] Conditions = new Control[3];
    public override void _Ready()
    {
        Conditions[0] = GetNode<Control>("Green");
        Conditions[1] = GetNode<Control>("Yellow");
        Conditions[2] = GetNode<Control>("Red");
    }
    public void SetCondition(float cond)
    {
        Condition co = TranslateCondition(cond);
        for (int i = 0; i < Conditions.Count(); i++)
        {
            if (i == (int)co)
                Conditions[i].Show();
            else
                Conditions[i].Hide();
        }
    }
    private Condition TranslateCondition(float cond)
    {
        Condition c = 0;
        if (cond > 66)
            c = Condition.GREEN;
        else if (cond > 33)
            c = Condition.YELLOW;
        else
            c = Condition.RED;
        return c;
    }
}
public enum Condition
{
    GREEN,
    YELLOW,
    RED
}