using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePath
{
    static int test;
    public class Card
    {
        public class Small
        {
            public const string empty = "Sprites/card/small/empty";
            public const string drawRed = "Sprites/card/small/draw_red";
            public const string drawYellow = "Sprites/card/small/draw_yellow";
            public const string drawBlue = "Sprites/card/small/draw_blue";
            public const string attack = "Sprites/card/small/attack";
        }

        public class Big
        {
            public const string empty = "Sprites/card/big/empty";
            public const string drawRed = "Sprites/card/big/draw_red";
            public const string drawYellow = "Sprites/card/big/draw_yellow";
            public const string drawBlue = "Sprites/card/big/draw_blue";
            public const string attack = "Sprites/card/big/attack";
        }
    }

    public class Effect
    {
        public const string attack = "Sprites/effect/attack";
        public const string attackAttempt = "Sprites/effect/attack_attempt";
        public const string move = "Sprites/effect/move";
    }

    public class Enemy
    {
        public const string minion = "Sprites/enemy/minion";
    }

    public class Object
    {
        public const string tower = "Sprites/object/tower";
    }

    public class Tile
    {
        public const string empty = "Sprites/tile/tile_empty";
        public const string red = "Sprites/tile/tile_red";
        public const string yellow = "Sprites/tile/tile_yellow";
        public const string blue = "Sprites/tile/tile_blue";
    }

    public class UI
    {
        public class Button
        {
            public const string endTurnPressed = "Sprites/ui/button/end_turn_pressed";
            public const string endTurnUnpressed = "Sprites/ui/button/end_turn_unpressed";
            public const string enemyTurn = "Sprites/ui/button/enemys_turn";
        }
    }
}
