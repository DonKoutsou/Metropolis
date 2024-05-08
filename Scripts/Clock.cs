using Godot;
using System;

public class Clock : Item
{
    string time;

    public void UpdateTime()
    {
        int hour, mins;
        DayNight.GetTime(out hour, out mins);
        string minstr = mins.ToString();
        string Hstring = hour.ToString();
        if (mins < 10)
            minstr = "0" + minstr;
        if (hour < 10)
            Hstring = "0" + Hstring;
        time = string.Format("{0} : {1}", Hstring,  minstr);
    }
    public override string GetItemDesc()
    {
        UpdateTime();

        return ItemDesc + " \n Ώρα: " + time;
    }
}
