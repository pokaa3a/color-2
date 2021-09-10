using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDrawColor : Action
{
    public Color color { get; protected set; }

    public ActionDrawColor(Color color)
    {
        type = ActionType.DrawColor;
        this.color = color;
    }

    public override bool Act(Vector2 xy)
    {
        // return: action finished or not

        if (Map.Instance.InsideMap(xy))
        {
            Map.Instance.SetTileColor(xy, color);
            return true;
        }

        return false;
    }
}
