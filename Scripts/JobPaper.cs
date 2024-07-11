using Godot;
using System;

public class JobPaper : Control
{
    [Signal]
    public delegate void OnTaskPickedUp(Control c);

    public Job PaperJob;
    public void SetData(Job job)
    {
        PaperJob = job;
        Control vbox = GetNode<Control>("Button/VBoxContainer");
        vbox.GetNode<Label>("JobName").Text = PaperJob.GetJobName();
        vbox.GetNode<Label>("Location").Text = string.Format("Προορισμός : X = {0} - Y = {1}", PaperJob.GetLocation().x, PaperJob.GetLocation().y);
        vbox.GetNode<Label>("Description").Text = string.Format("Μεταφορά προμηθειών στον φάρο {0}", PaperJob.GetOwnerName());
        vbox.GetNode<Label>("Reward").Text = string.Format("Αμοιβή : {0} Δραχμές", PaperJob.GetRewardAmmount());
    }
    private void OnPicked()
    {
        EmitSignal("OnTaskPickedUp", this);
    }
    public override void _Ready()
    {
        Hide();
    }
    public void ToggleAvailable(bool toggle)
    {
        GetNode<Button>("Button").Disabled = !toggle;
        GetNode<TextureRect>("Button/TextureRect").Visible = !toggle;
    }
}
