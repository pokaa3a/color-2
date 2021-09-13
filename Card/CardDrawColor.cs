using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawColor : Card
{
    public Color color { get; protected set; }
    private const int maxNumDrawings = 10;
    private int numDrawings = 0;

    public CardDrawColor(Color color) : base()
    {
        this.color = color;

        // Sprite
        if (color == Color.Red)
        {
            spritePath = SpritePath.Card.Small.drawRed;
            bigSpritePath = SpritePath.Card.Big.drawRed;
        }
        else if (color == Color.Blue)
        {
            spritePath = SpritePath.Card.Small.drawBlue;
            bigSpritePath = SpritePath.Card.Big.drawBlue;
        }
        else if (color == Color.Yellow)
        {
            spritePath = SpritePath.Card.Small.drawYellow;
            bigSpritePath = SpritePath.Card.Big.drawYellow;
        }
    }

    public override bool Act(Vector2 xy)
    {
        if (Map.Instance.InsideMap(xy) && numDrawings < maxNumDrawings)
        {
            Vector2Int rc = Map.Instance.XYtoRC(xy);
            Map.Instance.GetTile(rc).SetColor(color);
            if (++numDrawings == maxNumDrawings)
            {
                return true;
            }
        }
        return false;
    }
}