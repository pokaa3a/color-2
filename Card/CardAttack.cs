using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack : Card
{
    private const int attackAmount = 1;

    public CardAttack() : base()
    {
        // Sprite
        spritePath = SpritePath.Card.Small.attack;
        bigSpritePath = SpritePath.Card.Big.attack;

        // Action
        action = new ActionAttack(attackAmount);
    }
}
