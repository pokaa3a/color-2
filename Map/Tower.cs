using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MapObject
{
    public Tower() { }

    public Tower(Vector2Int rc) : base(rc)
    {
        gameObject.name = "Tower";

        spritePath = SpritePath.Object.tower;
        spriteWH = new Vector2(
            Map.Instance.tileWH.x * 0.5f,
            Map.Instance.tileWH.y * 0.6f);

        SetPosition(rc);
        SetSprite(this.spritePath);
    }
}
