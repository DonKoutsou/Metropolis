using Godot;

using System.Collections.Generic;

using System.Linq;
using System.Security.Policy;
public class WorldSoundManager : Spatial
{
    static Dictionary<string, AudioStreamPlayer> Players = new Dictionary<string, AudioStreamPlayer>();
    public override void _Ready()
    {
        var children = GetChildren();
        foreach (Node child in children)
        {
            if (child is AudioStreamPlayer)
            {
                Players.Add(child.Name, (AudioStreamPlayer)child);
            }
        }
    }
    public static void PlaySound(string SoundName)
    {
        if (Players.ContainsKey(SoundName))
            Players[SoundName].Play();
    }
    public static AudioStreamPlayer GetSound(string SoundName)
    {
        if (!Players.ContainsKey(SoundName))
            return null;
        return Players[SoundName];
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
