using Godot;
using System;

public class Time_UI : RichTextLabel
{
    static Time_UI instance;
    public override void _EnterTree()
    {
        base._EnterTree();
        instance = this;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        instance = null;
    }
    public static Time_UI GetInstance()
    {
        return instance;
    }
    public void UpdateTime(float hours, float mins)
    {
        string minstr = Math.Round(mins).ToString();
        string Hstring = Math.Round(hours).ToString();
        if (Math.Round(mins) < 10)
            minstr = "0" + minstr;
        if (Math.Round(hours) < 10)
            Hstring = "0" + Hstring;
        Text = string.Format("{0} : {1}", Hstring,  minstr);
    }
    public override void _Ready()
    {
        base._Ready();
        instance = this;
    }
}
