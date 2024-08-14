using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
public class DialogueManager : Node
{
    static DialogueManager Instance;
    public override void _Ready()
    {
        Instance = this;
    }
	//static bool IsTalking = false;
    //public static bool IsPlayerTalking()
    //{
    //    return IsTalking;
    //}
	bool DialogueRunning = false;

	List<Character> NextTalker = new List<Character>();
	List<string> NextLine = new List<string>();

	public void ScheduleDialogue(Character talker, string text)
	{
		NextTalker.Add(talker);
		NextLine.Add(text);

		if (DialogueRunning)
			return;
		if (NextTalker.Count == 1)
			DoNextDialogue();
	}
	public void DoNextDialogue()
	{
		NextTalker[0].GetTalkText().Talk(NextLine[0]);
		DialogueRunning = true;

		NextTalker.RemoveAt(0);
		NextLine.RemoveAt(0);
	}
	public void OnDialogueEnded()
	{
		DialogueRunning = false;
		if (NextTalker.Count > 0)
		{
			DoNextDialogue();
		}
	}
    public static DialogueManager GetInstance()
    {
        return Instance;
    }
}
