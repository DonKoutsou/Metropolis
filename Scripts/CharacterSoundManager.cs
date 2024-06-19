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
                AudioStreamPlayer3D player = (AudioStreamPlayer3D)child;
                //player.Play();
                //player.StreamPaused = true;
                Players.Add(child.Name, player);
            }
        }
    }
    public void ToggleSound(bool toggle, string SoundName, float PitchScale = -99, float DB = -99)
    {
        if (Players[SoundName].Playing == !toggle)
            Players[SoundName].Playing = toggle;
        if (PitchScale != -99)
            Players[SoundName].PitchScale = PitchScale;
        if (DB != -99)
            Players[SoundName].UnitDb = DB;
    }
}
