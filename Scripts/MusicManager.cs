using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class MusicManager : Node
{
        
    [Export]
	string[] BouzoukiSongExports = null;
    [Export]
	string[] GuitarSongExports = null;
	Dictionary<Song, AudioStream> BouzoukiSongs = new Dictionary<Song, AudioStream>();
    Dictionary<Song, AudioStream> GuitarSongs = new Dictionary<Song, AudioStream>();

    List <object> Instruments = new List<object>();

    static MusicManager Instance;

    public static MusicManager GetInstance()
    {
        return Instance;
    }
    public override void _Ready()
    {
        Instance = this;
        for (int i = 0; i < BouzoukiSongExports.Count(); i++)
		{
			BouzoukiSongs.Add((Song)i, ResourceLoader.Load<AudioStream>(BouzoukiSongExports[i]));
		}
        for (int i = 0; i < GuitarSongExports.Count(); i++)
		{
			GuitarSongs.Add((Song)i, ResourceLoader.Load<AudioStream>(GuitarSongExports[i]));
		}
    }
    public void RegisterInstrument(object instrument)
    {
        Instruments.Add(instrument);
    }
    public void RemoveInstrument(object instrument)
    {
        Instruments.Remove(instrument);
    }
    public AudioStream GetSong(Instrument inst, out float loc)
    {
        AudioStream song = null;
        loc = 0;
        Vector3 pos = inst.GlobalTranslation;
        foreach (Instrument instru in Instruments)
        {
            if (instru == inst)
                continue;
            if (pos.DistanceTo(instru.GlobalTranslation) <= 50 && instru.IsPlaying())
            {
                song = GetSongForInstrument(inst, (Song)SongToEnum(instru.GetCurrentSong()));
                loc = instru.GetCurrentSongLoc();
                break;
            }
        }
        if (song == null)
        {
            if (inst is Bouzouki)
                song = GetSongForInstrument(inst, (Song)RandomContainer.Next(0, BouzoukiSongs.Count));
            if (inst is Guitar)
                song = GetSongForInstrument(inst, (Song)RandomContainer.Next(0, GuitarSongs.Count));
        }
        return song;
    }
    int SongToEnum(AudioStream song)
    {
        int s = -1;
        if (BouzoukiSongs.ContainsValue(song))
        {
            foreach(KeyValuePair<Song, AudioStream> pair in BouzoukiSongs)
            {
                if (pair.Value == song)
                {
                    s = (int)pair.Key;
                    break;
                }
            }
        }
        if (GuitarSongs.ContainsValue(song))
        {
            foreach(KeyValuePair<Song, AudioStream> pair in GuitarSongs)
            {
                if (pair.Value == song)
                {
                    s = (int)pair.Key;
                    break;
                }
            }
        }
        return s;
    }
    AudioStream GetSongForInstrument(Instrument name, Song s)
    {
        AudioStream song = null;
        if (name is Bouzouki)
            song = BouzoukiSongs[s];
        if (name is Guitar)
            song = GuitarSongs[s];

        return song;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
public enum Song
{
    SONG1,
}
