using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawColor : Card
{
    public CardDrawColor(Color color) : base()
    {
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

        // Action
        action = new ActionDrawColor(color);
    }
}