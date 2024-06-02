using Godot;
using System;
using System.Linq;

public class CharacterSpawnLocations : Node
{
    [Export]
    public PackedScene[] CharSpawns = new PackedScene[0];
    public bool HasChars()
    {
        return CharSpawns.Count() > 0;
    }

}
