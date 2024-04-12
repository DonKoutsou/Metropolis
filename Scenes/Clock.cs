using Godot;
using System;

public class Clock : Item
{
    string time;

    public void UpdateTime()
    {
        float hour, mins;
        DayNight.GetTime(out hour, out mins);
        string minstr = Math.Round(mins).ToString();
        string Hstring = Math.Round(hour).ToString();
        if (Math.Round(mins) < 10)
            minstr = "0" + minstr;
        if (Math.Round(hour) < 10)
            Hstring = "0" + Hstring;
        time = string.Format("{0} : {1}", Hstring,  minstr);
    }
    public override string GetItemDesc()
    {
        UpdateTime();

        return ItemDesc + " \n Ώρα: " + time;
    }
}
