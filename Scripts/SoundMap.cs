using Godot;
using System;

public class SoundMap : GridMap
{
    [Export]
    PackedScene ShoreSound = null;
    public override void _Ready()
    {
        var usedcells = GetUsedCellsByItem(1);
        foreach(Vector3 cell in usedcells)
        {
            Spatial sound = ShoreSound.Instance<Spatial>();
            
            AddChild(sound);

            sound.GlobalTranslation = MapToWorld((int)cell.x, (int)cell.y, (int)cell.z);
        }
    }

}
