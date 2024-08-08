using Godot;

using System.Collections.Generic;

using System.Linq;
using System.Security.Policy;
public class WorldSoundManager : Spatial
{
    static WorldSoundManager Instance;
    Dictionary<string, AudioStreamPlayer> Players = new Dictionary<string, AudioStreamPlayer>();
    public override void _Ready()
    {
        Instance = this;
        var children = GetChildren();
        foreach (Node child in children)
        {
            if (child is AudioStreamPlayer)
            {
                Players.Add(child.Name, (AudioStreamPlayer)child);
            }
        }
    }
    public static WorldSoundManager GetInstance()
    {
        return Instance;
    }
    public AudioStreamPlayer GetSound(string SoundName)
    {
        AudioStreamPlayer Sound;
        Players.TryGetValue(SoundName, out Sound);
        return Sound;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
