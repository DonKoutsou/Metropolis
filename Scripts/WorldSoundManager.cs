using Godot;

using System.Collections.Generic;

using System.Linq;
using System.Security.Policy;
public class WorldSoundManager : Spatial
{
    static WorldSoundManager Instance;
    Dictionary<string, AudioStreamPlayer3D> Players = new Dictionary<string, AudioStreamPlayer3D>();
    public override void _Ready()
    {
        Instance = this;
        var children = GetChildren();
        foreach (Node child in children)
        {
            if (child is AudioStreamPlayer3D)
            {
                Players.Add(child.Name, (AudioStreamPlayer3D)child);
            }
        }
    }
    public static WorldSoundManager GetInstance()
    {
        return Instance;
    }
    public AudioStreamPlayer3D GetSound(string SoundName)
    {
        AudioStreamPlayer3D Sound;
        Players.TryGetValue(SoundName, out Sound);
        return Sound;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
