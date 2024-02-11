using Godot;
using System;

using MonoCustomResourceRegistry;

[RegisteredType(nameof(DialogueLine), "", nameof(Resource))]
 public partial class DialogueLine : Resource
{
    [Export]
    public string line {get; set;}
    [Export]
    public string Failline {get; set;}
     [Export]
    public DialogueOptions [] options  {get; set;}
    public Node Owner;
    public DialogueLine()
    {
        line = "Put Dialogue Here";
    }
}
