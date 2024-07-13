using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class JobBoard : Control
{
    Port BoardPort;
    private bool Open = false;
    public bool IsOpen()
    {
        return Open;
    }
    public void SetPort(Port p)
    {
        BoardPort = p;
    }
    public void ToggleUI(bool toggle)
    {
        Open = toggle;
        Visible = toggle;
        if (toggle)
        {
            UpdateJobs();
        }
    }
    public void UpdateJobs()
    {
        List<Job> jobs = GlobalJobManager.GetInstance().GetJobs();
        List<Job> ajobs = GlobalJobManager.GetInstance().GetAssignedJobs();
        var children = GetNode("Panel/GridContainer").GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            JobPaper p = (JobPaper)children[i];
            if (i > jobs.Count() - 1)
            {
                p.Hide();
            }
            else
            {
                Job j = jobs[i];
                p.SetData(j);
                p.Show();
                if (ajobs.Contains(j))
                {
                    p.ToggleAvailable(false);
                }
                   
            }
        }
    }
    JobPaper Paper;
    Job JobToTake;
    

    
    private void OnTaskPicked(Control c)
    {
        if (GlobalJobManager.GetInstance().HasJobAssigned())
        {
            GetNode<AcceptDialog>("AcceptDialog").Popup_();
            
        }
        else if (!BoardPort.PlayerHasBoatInPort())
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
        if (JobToTake is DeliverJob del)
        {
            Spatial cargo = del.GetDeliveryObj().Instance<Spatial>();
            BoardPort.GetPlayerBoat().AddChild(cargo);
            cargo.Translation = Vector3.Zero;
            cargo.Rotation = Vector3.Zero;
        }
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
