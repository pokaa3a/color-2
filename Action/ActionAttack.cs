using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : Action
{
    private int attackAmount = 1;

    public ActionAttack(int attackAmount)
    {
        this.attackAmount = attackAmount;
    }

    public override bool Act(Vector2 xy)
    {
        if (Map.Instance.InsideMap(xy))
        {
            Vector2Int rc = Map.Instance.XYtoRC(xy);
            Enemy enemy = Map.Instance.GetTile(rc).GetObject<Enemy>();

            if (enemy != null)
            {
                enemy.BeAttacked(attackAmount);
                return true;
            }
        }
        return false;
    }
}
