using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Record all the statics in the game
public class GameStatics
{
    // [Public]
    public int score { get; protected set; }

    // [Private]
    private Text scoreText;

    // Singleton
    private static GameStatics _instance;
    public static GameStatics Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameStatics();
            }
            return _instance;
        }
    }

    private GameStatics()
    {
        scoreText = GameObject.Find(ObjectPath.score).GetComponent<Text>() as Text;
    }

    public void ComputeScore()
    {
        foreach (MapObject obj in Map.Instance.mapObjects)
        {
            if (obj is Tower)
            {
                if (Map.Instance.GetTile(obj.rc).color != Color.Empty)
                {
                    score += 1;
                }
            }
        }
        scoreText.text = score.ToString();
    }
}
