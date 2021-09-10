using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    private Action selectedAction;

    // Singleton
    private static ActionManager _instance;
    public static ActionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ActionManager();
            }
            return _instance;
        }
    }

    private ActionManager()
    {

    }

    public void SelectAction(Action action)
    {
        selectedAction = action;
    }

    public void Act(Vector2 xy)
    {
        if (selectedAction == null)
        {
            return;
        }

        if (selectedAction.Act(xy))
        {
            selectedAction = null;
        }
    }
}
