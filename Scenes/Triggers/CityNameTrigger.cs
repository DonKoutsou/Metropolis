using Godot;
using System;
using System.Collections.Generic;

public class CityNameTrigger : Spatial
{
    bool IsIn = false;

    private void On_Player_Entered(object body)
    {
        if (body is Player)
        {
            if (IsIn)
                return;

            IsIn = true;
            
            Player pl = (Player)body;

            CityNameUI.ShowName(((Island)GetParent()).IslandSpecialName);
        }
    }
    private void On_Player_Left(object body)
    {
        IsIn = false;
    }
}



