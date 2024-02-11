using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

using MonoCustomResourceRegistry;

[RegisteredType(nameof(DialogueAction), "", nameof(Resource))]
public partial class DialogueAction : Resource
{
    [Export]
    string NameOfFunctionToRunOnOwner = null;
    public bool Perform(Node Owner, Node Performer)
    {
        if (NameOfFunctionToRunOnOwner == null)
            return false;
        Owner.CallDeferred(NameOfFunctionToRunOnOwner, Performer);
        Type thisType = Owner.GetType();
        MethodInfo theMethod = thisType.GetMethod(NameOfFunctionToRunOnOwner);
        var parameters = new object[1];
        parameters[0] = Performer;
        bool result = (bool)theMethod.Invoke(Owner, BindingFlags.InvokeMethod | BindingFlags.Public ,null,  parameters, null);
        return result;
    }
    public DialogueAction()
    {

    }
}

