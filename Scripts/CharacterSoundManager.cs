using Godot;

using System.Collections.Generic;

using System.Linq;
using System.Security.Policy;

public class CharacterSoundManager : Spatial
{
    Dictionary<string, AudioStreamPlayer3D> Players = new Dictionary<string, AudioStreamPlayer3D>();
    public override void _Ready()
    {
        var children = GetChildren();
        foreach (Node child in children)
        {
            if (child is AudioStreamPlayer3D)
            {
                Players.Add(child.Name, (AudioStreamPlayer3D)child);
            }
        }
    }
    public AudioStreamPlayer3D GetSound(string SoundName)
    {
        AudioStreamPlayer3D Sound;
        Players.TryGetValue(SoundName, out Sound);
        return Sound;
    }
}
