using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEraseColor : Card
{
    public CardEraseColor() : base()
    {
        spritePath = SpritePath.Card.Small.eraseColor;
        bigSpritePath = SpritePath.Card.Big.eraseColor;
    }

    public override bool Act(Vector2 xy)
    {
        if (Map.Instance.InsideMap(xy))
        {
            Vector2Int rc = Map.Instance.XYtoRC(xy);
            Map.Instance.GetTile(rc).SetColor(Color.Empty);
            return true;
        }
        return false;
    }
}
