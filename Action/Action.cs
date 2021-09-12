using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    DrawColor
}

public abstract class Action
{
    public ActionType type;

    public Action()
    {

    }

    // Called if touching
    public abstract bool Act(Vector2 xy);
}
