using Godot;
using System;
using System.Collections.Generic;

public class Credits : Control
{
    [Export]
    PackedScene CreditNameScene = null;
    [Signal]
    public delegate void OnCreditsEnded();
    static Godot.Collections.Dictionary<string, string> CreditList = null;
    public override void _Ready()
    {
        var LocDataFile = new File();
        LocDataFile.Open("res://Assets/Spreadsheet_Imports/CreditList.json", File.ModeFlags.Read);
        var LocDataJson = JSON.Parse(LocDataFile.GetAsText());
        LocDataFile.Close();
        if (LocDataJson.Result is Godot.Collections.Array)
        {
            CreditList = new Godot.Collections.Dictionary<string, string>();

            Godot.Collections.Array info = (Godot.Collections.Array)LocDataJson.Result;

            GD.Print("JSon contains " + info.Count + " entries");

            //GD.Print("Found Array of " + info[0].GetType());

            for (int i = 0; i < info.Count; i++)
            {
                Godot.Collections.Dictionary LocalisedStrings = (Godot.Collections.Dictionary)info[i];
                string DevTitle = (string)((Godot.Collections.Array)LocalisedStrings.Values)[0];
                //GD.Print("Dialogue Name is " + DialogueName);
                string DevName = (string)((Godot.Collections.Array)LocalisedStrings.Values)[1];

                CreditList.Add(DevTitle, DevName);
            }
            GD.Print("Credits Imported");
        }
        Hide();
    }
    public void PlayCredits()
    {
        foreach (KeyValuePair<string, string> Cred in CreditList)
        {
            Control CreditName = (Control)CreditNameScene.Instance();
            GetNode<Control>("VBoxContainer").AddChild(CreditName);
            CreditName.GetNode<Label>("VBoxContainer/DevTitle").Text = Cred.Key;
            CreditName.GetNode<Label>("VBoxContainer/DevName").Text = Cred.Value;
        }
        Show();
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Credit Roll");
        GetNode<AudioStreamPlayer>("OuttroMusic").Playing = true;
    }
    private void Credits_Finished(string anim)
    {
        Hide();
        EmitSignal("OnCreditsEnded");
    }
}