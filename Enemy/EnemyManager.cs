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
        Enemy minion = new Enemy(new Vector2Int(3, 5));
        enemies.Add(minion);
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
