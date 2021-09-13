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

    private int maxLife = 5;
    private int _life = 5;
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
        GameObject textObject = new GameObject("leftText");
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
        Tuple<List<Vector2Int>, Vector2Int> moveAndAttack = PlanMoveAndAttack();
        return MoveThenAttemptToAttack(moveAndAttack.Item1, moveAndAttack.Item2);
    }

    private Tuple<List<Vector2Int>, Vector2Int> PlanMoveAndAttack()
    {
        // Decide final position
        Vector2Int rcMoveDst = rc;
        for (int i = 0; i < 20; ++i)
        {
            int r = UnityEngine.Random.Range(0, Map.rows);
            int c = UnityEngine.Random.Range(0, Map.rows);
            if (Map.Instance.IsEmpty(new Vector2Int(r, c)))
            {
                rcMoveDst = new Vector2Int(r, c);
                break;
            }
        }

        // Path planning
        List<Vector2Int> rcMove = new List<Vector2Int>();
        for (int d = 1; d <= Math.Abs(rcMoveDst.x - rc.x); ++d)
        {
            int x = rc.x + (rcMoveDst.x > rc.x ? d : -d);
            int y = rc.y;
            rcMove.Add(new Vector2Int(x, y));
        }
        int targetRow = rcMove.Count > 0 ? rcMove[rcMove.Count - 1].x : rc.x;
        for (int d = 1; d <= Math.Abs(rcMoveDst.y - rc.y); ++d)
        {
            int x = targetRow;
            int y = rc.y + (rcMoveDst.y > rc.y ? d : -d);
            rcMove.Add(new Vector2Int(x, y));
        }

        // Decide where to attack
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        Vector2Int rcAttackAttempt = rcMoveDst + Vector2Int.down;
        for (int i = 0; i < 4; ++i)
        {
            if (Map.Instance.InsideMap(rcMoveDst + directions[i]))
            {
                rcAttackAttempt = rcMoveDst + directions[i];
                break;
            }
        }

        return new Tuple<List<Vector2Int>, Vector2Int>(rcMove, rcAttackAttempt);
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
        var attackObj = Map.Instance.AddObject<Effect>(rcAttackAttempt);
        attackObj.spritePath = SpritePath.Effect.attackAttempt;
        rcAttack = rcAttackAttempt;

        EnemyManager.Instance.OneActionCompleted();
    }

    private IEnumerator Attack()
    {
        Map.Instance.DestroyObject<Effect>(rcAttack);
        var attackEffect = Map.Instance.AddObject<Effect>(rcAttack);
        attackEffect.spritePath = SpritePath.Effect.attack;

        yield return new WaitForSeconds(1f);
        Map.Instance.DestroyObject<Effect>(rcAttack);

        rcAttack = new Vector2Int(-1, -1);
    }

    public void BeAttacked(int attack)
    {
        life = life < attack ? 0 : life - attack;
    }
}
