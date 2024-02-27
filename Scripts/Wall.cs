using Godot;
using System;

public class Wall : StaticBody
{
	virtual public bool Touch(object body)
	{
		return false;
	}

}



