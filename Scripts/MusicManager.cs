using Godot;
using System;
using System.Collections.Generic;
public class MusicManager : Node
{
    [Export]
	public List <AudioStream> BouzoukiSongExports = null;
    [Export]
	public List <AudioStream> GuitarSongExports = null;
	static Dictionary<Song, AudioStream> BouzoukiSongs = new Dictionary<Song, AudioStream>();
    static Dictionary<Song, AudioStream> GuitarSongs = new Dictionary<Song, AudioStream>();

    static List <object> Instruments = new List<object>();
    public override void _Ready()
    {
        for (int i = 0; i < BouzoukiSongExports.Count; i++)
		{
			BouzoukiSongs.Add((Song)i, BouzoukiSongExports[i]);
		}
        for (int i = 0; i < GuitarSongExports.Count; i++)
		{
			GuitarSongs.Add((Song)i, GuitarSongExports[i]);
		}
    }
    public static void RegisterInstrument(object instrument)
    {
        Instruments.Add(instrument);
    }
    public static void RemoveInstrument(object instrument)
    {
        Instruments.Remove(instrument);
    }
    public static AudioStream GetSong(Instrument inst, out float loc)
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
            song = GetSongForInstrument(inst, (Song)RandomContainer.Next(0, BouzoukiSongs.Count));
        }
        return song;
    }
    static int SongToEnum(AudioStream song)
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
    static AudioStream GetSongForInstrument(Instrument name, Song s)
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
