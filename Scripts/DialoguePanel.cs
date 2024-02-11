using Godot;
using System;
using System.Linq;

public class DialoguePanel : Panel
{
    RichTextLabel Textl;

    public bool showing = false;

    Timer turnoff;

    Timer DialoguePercTimer;

    Panel DiagOption;

    public bool waitingforoption;

    DialogueLine currentline;

    Character[] Talkers = new Character[0];

    Player pl;
    public override void _Ready()
    {
        base._Ready();
        Hide();
        Textl = GetNode<RichTextLabel>("RichTextLabel");
        turnoff = GetNode<Timer>("TurnOffTimer");
        DialoguePercTimer = GetNode<Timer>("DialoguePercTimer");
        DiagOption = (Panel)GetNode("DialogueOptionPanel");
        DiagOption.Hide();
        DiagOption.GetNode<Button>("Option1").Hide();
        DiagOption.GetNode<Button>("Option2").Hide();
        
    }
    public void DoDialogue(DialogueLine line, Node Owner, bool success)
    {
        if (pl == null)
        {
            var players = GetTree().GetNodesInGroup("Player");
            pl = (Player)players[0];
        }
        turnoff.Stop();
        currentline = line;
        currentline.Owner = Owner;
        Show();
        Textl.Text = currentline.line;
        showing = true;
        Textl.PercentVisible = 0;
        DialoguePercTimer.Start();
        if (currentline.options.Count() > 0)
        {
            DiagOption.Show();
            
            Button option2 = DiagOption.GetNode<Button>("Option2");
            for (int i = 0; i < currentline.options.Count(); i++)
            {
                Button option1 = DiagOption.GetNode<Button>("Option" + (i + 1).ToString());

                option1.Text = currentline.options[i].optionline;

                option1.Show();
            }
            waitingforoption = true;
        }
        else
        {
            waitingforoption = false;
            DiagOption.Hide();
            DiagOption.GetNode<Button>("Option1").Hide();
            DiagOption.GetNode<Button>("Option2").Hide();
        }
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
    }
    public void TurnOff()
    {
        if (waitingforoption)
            return;
        Hide();
        showing = false;
        Textl.Text = string.Empty;
        turnoff.Stop();
    }
    public void UpdatePerc()
    {
        float perc = Textl.PercentVisible;
        if (showing && perc < 1)
        {
            Textl.PercentVisible = Textl.PercentVisible + 0.1f;
        }
        if (showing && perc >= 1)
        {
            turnoff.Start();
            DialoguePercTimer.Stop();
        }
    }
    void OnDialogueButton1Pressed()
    {
        PerformDialogueOption(0);
    }
    void OnDialogueButton2Pressed()
    {
        PerformDialogueOption(1);
    }
    void PerformDialogueOption(int option)
    {
         if (currentline.options[option].LineBranch != null)
        {
            if (currentline.options[option].Action != null)
            {
                if (currentline.options[option].Action.Perform(currentline.Owner, pl))
                {
                    DoDialogue(currentline.options[option].LineBranch, currentline.Owner, true);
                }
                else
                {
                    DoDialogue(currentline.options[option].LineBranch, currentline.Owner, false);
                }
            }
            else  
                DoDialogue(currentline.options[option].LineBranch, currentline.Owner, true);
        }
        else
        {
            DiagOption.Hide();
            waitingforoption = false;
        }
    }
}
