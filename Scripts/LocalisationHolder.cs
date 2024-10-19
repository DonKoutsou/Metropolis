using Godot;
using Godot.Collections;
using System;
using System.Linq;

public class LocalisationHolder : Node
{
    static Dictionary<string, string[]> Localizations = null;
    //static LocalisationHolder Instance;
    static Language CurrentLanguage = Language.GREEK;

	public static LocalisationHolder Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        Localizations = new Dictionary<string, string[]>();
        Directory dir = new Directory();
        dir.Open("res://Assets/Spreadsheet_Imports");
        dir.ListDirBegin(true, true);
        for (int i = 0; i < 10; i++)
        {
            string d = dir.GetNext();
            if (d.Length < 2)
                continue;
            GD.Print(d +  " |||||||||File found|||||||");
            ImportLocalisation(d);
            GD.Print(d +  " <<<<<<<Imported>>>>>>>");
        }
        GD.Print("!!!!!Localisations Imported!!!!!!");
        //Instance = this;
        //Localizations = TempLocalizations.Duplicate();
        //TempLocalizations.Clear();
    }
    public void Initialise(Language l)
    {
        CurrentLanguage = l;
        LocaliseUI();
    }
    //public static LocalisationHolder GetInstance()
    //{
    //    return Instance;
    //}
    public void ImportLocalisation(string JsonName)
    {
        GD.Print("Importing Localization JSon");
        var LocDataFile = new File();

        string JsonLocation = "res://Assets/Spreadsheet_Imports/" + JsonName;

        LocDataFile.Open(JsonLocation, File.ModeFlags.Read);
        var LocDataJson = JSON.Parse(LocDataFile.GetAsText());
        LocDataFile.Close();
        if (LocDataJson.Result is Godot.Collections.Array)
        {
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
            
        }
    }
    public static string GetString(string Name)
    {
        string Text = string.Empty;
        if (!Localizations.ContainsKey(Name))
        {
            //GD.Print("Key not found |" + Name);
            return "||String Not Localised||";
        }
        else if (CurrentLanguage == Language.GREEK)
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