using Godot;
using System;
using System.Collections.Generic;

public class Book : Item
{
    [Export]
    BookSeries Series = BookSeries.MACHINES;
    [Export]
    int VolumeNumber = 1;

    public void SetVoluemeNumber(int Num)
    {
        VolumeNumber = Num;
    }
    public int GetVolumeNumber()
    {
        return VolumeNumber;
    }
    public BookSeries GetSeries()
    {
        return Series;
    }
    public override string GetItemName()
	{
		return BookEnumTranslator.TranslateBookEnum(Series);
	}
    public override string GetInventoryItemName()
    {
        return BookEnumTranslator.TranslateBookEnum(Series) + "\n Τόμος : " + VolumeNumber.ToString();
    }
    public override string GetItemDesc()
    {
        return LocalisationHolder.GetString(BookEnumTranslator.TranslateBookEnum(Series)) + "\n " + LocalisationHolder.GetString("Τόμος") + " : " + VolumeNumber.ToString() + "\n" + LocalisationHolder.GetString(ItemDesc);
    }
    public override void InputData(ItemInfo data)
	{
		base.InputData(data);

        int cap = (int)data.CustomData["VolumeNumber"];
        SetVoluemeNumber(cap);
        BookVolumeHolder.OnVolumeFound(GetSeries(), cap);

	}
    public override void GetCustomData(out string[] Keys, out object[] Values)
	{
		Keys = new string[1];
		Values = new object[1];
        
        Keys[0] = "VolumeNumber";
		Values[0] = GetVolumeNumber();
	}
}
public class BookVolumeHolder
{
    static List<int> MachineVolumesFound = new List<int>();
    static List<int> HistoryVolumesFound = new List<int>();
    static List<int> MotherCityVolumesFound = new List<int>();

    public static void OnVolumeFound(BookSeries Serie, int Volume)
    {
        switch(Serie)
        {
            case BookSeries.MACHINES:
            {
                MachineVolumesFound.Add(Volume);
                break;
            }
            case BookSeries.HISTORY:
            {
                HistoryVolumesFound.Add(Volume);
                break;
            }
            case BookSeries.MOTHERCITY:
            {
                MotherCityVolumesFound.Add(Volume);
                break;
            }
        }
    }
    public static bool IsVolumeFound(BookSeries Serie, int Volume)
    {
        bool found = false;
        switch(Serie)
        {
            case BookSeries.MACHINES:
            {
                found = MachineVolumesFound.Contains(Volume);
                break;
            }
            case BookSeries.HISTORY:
            {
                found = HistoryVolumesFound.Contains(Volume);
                break;
            }
            case BookSeries.MOTHERCITY:
            {
                found = MotherCityVolumesFound.Contains(Volume);
                break;
            }
        }
        return found;
    }
    public static int GetRandomUnfoundVolume(BookSeries Serie)
    {
        int volume = 1;
        switch(Serie)
        {
            case BookSeries.MACHINES:
            {
                if (MachineVolumesFound.Count > 8)
                {
                    volume = -1;
                }
                else
                {
                    while (MachineVolumesFound.Contains(volume))
                        volume = RandomContainer.Next(1, 9);
                }
                break;
            }
            case BookSeries.HISTORY:
            {
                if (HistoryVolumesFound.Count > 8)
                {
                    volume = -1;
                }
                else
                {
                    while (HistoryVolumesFound.Contains(volume))
                        volume = RandomContainer.Next(1, 9);
                }
                break;
            }
            case BookSeries.MOTHERCITY:
            {
                if (MotherCityVolumesFound.Count > 8)
                {
                    volume = -1;
                }
                else
                {
                    while (MotherCityVolumesFound.Contains(volume))
                        volume = RandomContainer.Next(1, 9);
                }
                break;
            }
        }
        return volume;
    }
    
}
public class BookEnumTranslator
{
    public static string TranslateBookEnum(BookSeries Serie)
    {
        string Name = string.Empty;
        switch(Serie)
        {
            case BookSeries.MACHINES:
            {
                Name = "B1";
                break;
            }
            case BookSeries.HISTORY:
            {
                Name = "B2";
                break;
            }
            case BookSeries.MOTHERCITY:
            {
                Name = "B3";
                break;
            }
        }
        return Name;
    }
}
public enum BookSeries
{
    MACHINES,
    HISTORY,
    MOTHERCITY,
    MUSIC,
}