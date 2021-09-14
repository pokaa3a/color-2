using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MapObject
{
    private int _id = 0;
    public int id
    {
        get => _id;
        set
        {
            _id = value;
            gameObject.name = $"enemy_{_id}";
        }
    }
    private Vector2Int rcAttack = new Vector2Int(-1, -1);

    private const int maxLife = 5;
    private int _life = maxLife;
    private int life
    {
        get => _life;
        set
        {
            _life = value;
            lifeText.text = $"{_life}/{maxLife}";
        }
    }
    private TextMesh lifeText;

    public Enemy()
    {

    }

    public Enemy(Vector2Int rc) : base(rc)
    {
        gameObject.name = "enemy";

        spriteWH = new Vector2(
            Map.Instance.tileWH.x * 0.7f,
            Map.Instance.tileWH.y * 0.7f);

        this.spritePath = SpritePath.Enemy.minion;

        // Set up life text
        GameObject textObject = new GameObject("lifeText");
        textObject.transform.SetParent(this.gameObject.transform);
        lifeText = textObject.AddComponent<TextMesh>() as TextMesh;
        textObject.transform.localPosition = new Vector2(0f, 0.7f);
        textObject.transform.localScale = new Vector2(0.04f, 0.08f);
        lifeText.fontSize = 100;
        lifeText.text = $"{life}/{maxLife}";
        lifeText.anchor = TextAnchor.MiddleCenter;
        lifeText.color = new Color32(255, 100, 0, 255);   // red
    }

    public void Act()
    {
        if (Map.Instance.InsideMap(rcAttack))
        {
            component.CallStartCoroutine(Attack());
        }
        Map.Instance.GetTile(rc).CallStartCoroutine(PlanNextAction());
    }

    private IEnumerator PlanNextAction()
    {
        // this enemy has only one action
        Tuple<List<Vector2Int>, Vector2Int> moveAndAttack = PlanMoveAndAttack();
        return MoveThenAttemptToAttack(moveAndAttack.Item1, moveAndAttack.Item2);
    }

    private Tuple<List<Vector2Int>, Vector2Int> PlanMoveAndAttack()
    {
        // Look for closest tower
        int closestDist = Map.rows * 100;
        Vector2Int closestTowerRc = new Vector2Int(100, 100);
        for (int r = 0; r < Map.rows; ++r)
        {
            for (int c = 0; c < Map.rows; ++c)
            {
                if (Map.Instance.GetTile(new Vector2Int(r, c)).GetObject<Tower>() != null)
                {
                    if (Math.Abs(r - rc.x) + Math.Abs(c - rc.y) < closestDist)
                    {
                        closestTowerRc = new Vector2Int(r, c);
                        closestDist = Math.Abs(r - rc.x) + Math.Abs(c - rc.y);
                    }
                }
            }
        }

        // Path planning
        const int maxSteps = 2;
        List<Vector2Int> rcMoves = new List<Vector2Int>();
        while (rcMoves.Count < maxSteps &&
            Math.Abs(closestTowerRc.x - rc.x) + Math.Abs(closestTowerRc.y - rc.y) > 1)
        {
            Vector2Int lastRc = rcMoves.Count > 0 ? rcMoves[rcMoves.Count - 1] : rc;

            if (Math.Abs(closestTowerRc.x - lastRc.x) > 0)
            {
                int r = lastRc.x + (closestTowerRc.x > lastRc.x ? 1 : -1);
                int c = lastRc.y;
                rcMoves.Add(new Vector2Int(r, c));
            }
            else
            {
                int r = lastRc.x;
                int c = lastRc.y + (closestTowerRc.y > rc.y ? 1 : -1);
                rcMoves.Add(new Vector2Int(r, c));
            }
        }

        // Decide where to attack
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        Vector2Int rcAttackAttempt = new Vector2Int(-1, -1);
        Vector2Int dstRc = rcMoves.Count > 0 ? rcMoves[rcMoves.Count - 1] : rc;
        for (int i = 0; i < 4; ++i)
        {
            Vector2Int maybeAttackAttempt = dstRc + directions[i];
            if (maybeAttackAttempt == closestTowerRc)
            {
                rcAttackAttempt = maybeAttackAttempt;
                break;
            }
        }
        return new Tuple<List<Vector2Int>, Vector2Int>(rcMoves, rcAttackAttempt);
    }

    private IEnumerator MoveThenAttemptToAttack(List<Vector2Int> rcMove, Vector2Int rcAttackAttempt)
    {
        yield return new WaitForSeconds(0.2f);

        // Draw path
        for (int i = 0; i < rcMove.Count; ++i)
        {
            var pathObj = Map.Instance.AddObject<Effect>(rcMove[i]);
            pathObj.spritePath = SpritePath.Effect.move;
        }
        yield return new WaitForSeconds(1f);

        // Move
        for (int i = 0; i < rcMove.Count; ++i)
        {
            // rc = rcMove[i];
            Map.Instance.MoveObject<Enemy>(rc, rcMove[i]);
            yield return new WaitForSeconds(0.2f);
        }

        // Erase path
        for (int i = 0; i < rcMove.Count; ++i)
        {
            Map.Instance.DestroyObject<Effect>(rcMove[i]);
        }
        yield return new WaitForSeconds(0.5f);

        // Draw attack attempt
        if (Map.Instance.InsideMap(rcAttackAttempt))
        {
            var attackObj = Map.Instance.AddObject<Effect>(rcAttackAttempt);
            attackObj.spritePath = SpritePath.Effect.attackAttempt;
            rcAttack = rcAttackAttempt;
        }
        EnemyManager.Instance.OneActionCompleted();
    }

    private IEnumerator Attack()
    {
        if (Map.Instance.InsideMap(rcAttack))
        {
            Map.Instance.DestroyObject<Effect>(rcAttack);
            var attackEffect = Map.Instance.AddObject<Effect>(rcAttack);
            attackEffect.spritePath = SpritePath.Effect.attack;

            yield return new WaitForSeconds(1f);
            Map.Instance.DestroyObject<Effect>(rcAttack);
            rcAttack = new Vector2Int(-1, -1);
        }
        yield return null;
    }

    public void BeAttacked(int attack)
    {
        life = life < attack ? 0 : life - attack;
    }
}
