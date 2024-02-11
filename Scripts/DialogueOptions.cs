using Godot;
using System;

using MonoCustomResourceRegistry;

[RegisteredType(nameof(DialogueOptions), "", nameof(Resource))]
public partial class DialogueOptions : Resource
{
    [Export]
    public string optionline {get; set;}

    [Export]
    public DialogueLine LineBranch {get; set;}

    [Export]
    public DialogueAction Action {get; set;}

    public DialogueOptions()
    {
        optionline = "Put option text here";
    }
}

