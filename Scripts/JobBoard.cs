using Godot;
using System;
using System.Linq;

public class JobBoard : Control
{
    public void UpdateJobs()
    {
        Job[] availablejobs;
        GlobalJobManager.GetInstance().GetJobs(out availablejobs);

        var children = GetNode("Panel/GridContainer").GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            JobPaper p = (JobPaper)children[i];
            if (i > availablejobs.Count() - 1)
            {
                p.Hide();
            }
            else
            {
                Job j = availablejobs[i];
                p.SetData(j);
                p.Show();
            }
        }
    }
    JobPaper Paper;
    Job JobToTake;
    Port BoardPort;

    public void SetPort(Port p)
    {
        BoardPort = p;
    }
    private void OnTaskPicked(Control c)
    {
        if (GlobalJobManager.GetInstance().HasJobAssigned())
        {
            GetNode<AcceptDialog>("AcceptDialog").Popup_();
        }
        if (!BoardPort.PlayerHasBoatInPort())
        {
            GetNode<AcceptDialog>("NoBoatDialogue").Popup_();
        }
        else
        {
            Paper = (JobPaper)c;
            JobToTake = Paper.PaperJob;
            ConfirmationDialog diag = GetNode<ConfirmationDialog>("ConfirmationDialog");
            diag.GetCloseButton().Hide();
            diag.Popup_();
            diag.GetCancel().Connect("button_down", this, "DialogueCanceled");
        }
        
    }
    private void DialogueConfirmed()
    {
        Paper.ToggleAvailable(false);
        GlobalJobManager.GetInstance().OnJobAssigned(JobToTake);
        Paper = null;
        JobToTake = null;
    }
    private void DialogueCanceled()
    {
        ConfirmationDialog diag = GetNode<ConfirmationDialog>("ConfirmationDialog");
        diag.GetCancel().Disconnect("button_down", this, "DialogueCanceled");
        JobToTake = null;
        Paper = null;
    }
    public override void _Ready()
    {
        
    }
    private void CloseUI()
    {
        Hide();
    }
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
public enum JobType
{
    Rescue,
    Deliver,
    Escort,
}
public enum Difficulty
{
    Easy,
    Medium,
    Hard,
}
