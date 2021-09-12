using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<Enemy> enemies;
    private int numActionsCompleted = 0;

    // Singleton
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyManager();
            }
            return _instance;
        }
    }

    private EnemyManager()
    {
        enemies = new List<Enemy>();
    }

    public void Create()
    {
        Vector2Int rc = new Vector2Int(3, 5);
        Enemy minion = Map.Instance.AddObject<Enemy>(rc);
        enemies.Add(minion);
        // minion.SetPosition(rc);
    }

    public void StartTurn()
    {
        numActionsCompleted = 0;
        foreach (Enemy e in enemies)
        {
            e.Act();
        }
    }

    public void OneActionCompleted()
    {
        numActionsCompleted++;
        if (numActionsCompleted == enemies.Count)
        {
            TurnHandler.Instance.SwitchTurn();
        }
    }
}
