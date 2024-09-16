using Godot;
using Godot.Collections;
using System;

[Tool]
public class LocalisationHolder : Node
{
    [Export]
    Dictionary<string, string[]> Localizations = null;
    static LocalisationHolder Instance;
    Language CurrentLanguage = Language.GREEK;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
    public void Initialise(Language l)
    {
        CurrentLanguage = l;
        LocaliseUI();
    }
    public static LocalisationHolder GetInstance()
    {
        return Instance;
    }
    public void ImportLocalisation()
    {
        GD.Print("Importing JSon");
        var LocDataFile = new File();
        LocDataFile.Open("res://Assets/Dialogue/DialogueTest.json", File.ModeFlags.Read);
        var LocDataJson = JSON.Parse(LocDataFile.GetAsText());
        LocDataFile.Close();
        if (LocDataJson.Result is Godot.Collections.Array)
        {
            Localizations = new Dictionary<string, string[]>();

            Godot.Collections.Array info = (Godot.Collections.Array)LocDataJson.Result;

            GD.Print("JSon contains " + info.Count + " entries");

            //GD.Print("Found Array of " + info[0].GetType());

            for (int i = 0; i < info.Count; i++)
            {
                Dictionary LocalisedStrings = (Dictionary)info[i];
                string DialogueName = (string)((Godot.Collections.Array)LocalisedStrings.Values)[0];
                //GD.Print("Dialogue Name is " + DialogueName);
                string GreekTranslation = (string)((Godot.Collections.Array)LocalisedStrings.Values)[1];
                //GD.Print("Greek Translation is " + GreekTranslation);
                string EnglishTranslation = (string)((Godot.Collections.Array)LocalisedStrings.Values)[2];
               // GD.Print("English Translation is " + EnglishTranslation);
                string[] translations = new string[2]{
                    GreekTranslation, EnglishTranslation
                };
                Localizations.Add(DialogueName, translations);
            }
            GD.Print("Localisations Imported");
        }
        else
        {
            GD.Print("Its Not A Dictionary");
        }
       // GD.Print(LocDataJson.Result);
    }
    public string GetString(string Name)
    {
        string Text = string.Empty;
        if (CurrentLanguage == Language.GREEK)
        {
            Text =  Localizations[Name][0];
        }
        else if (CurrentLanguage == Language.ENGLISH)
        {
            Text =  Localizations[Name][1];
        }
        return Text;
    }
    public void LocaliseUI()
    {
        var Buttons = GetTree().GetNodesInGroup("Translatables");

        for (int i = 0; i < Buttons.Count; i++)
        {
            //Button b = (Button)Buttons[i];
            string t = (string)((Control)Buttons[i]).Get("text");
            if (Localizations.ContainsKey(t))
            {
                ((Control)Buttons[i]).Set("text", GetString(t));
            }
        }
    }
}
public enum Language
{
    GREEK,
    ENGLISH
}