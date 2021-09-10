using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Tile
{
    public Color color { get; private set; } = Color.Empty;

    public Vector2Int rc { get; private set; }  // row-col
    public Vector2 srpiteWh;

    private GameObject gameObject;
    private List<MapObject> objects;

    public Tile(Vector2Int rc, Vector2 wh)
    {
        gameObject = new GameObject($"tile_{rc.x}_{rc.y}");
        objects = new List<MapObject>();

        this.rc = rc;
        this.srpiteWh = Map.Instance.tileWH;

        // Position
        Vector2 xy = Map.Instance.RCtoXY(rc);
        gameObject.transform.position = new Vector3(xy.x, xy.y, 0);

        // Sprite
        gameObject.AddComponent<SpriteRenderer>();
        SetColor(Color.Empty);
    }

    public void SetColor(Color color)
    {
        this.color = color;
        SpriteRenderer sprRend = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        if (color == Color.Empty)
        {
            sprRend.sprite = Resources.Load<Sprite>(SpritePath.Tile.empty);
        }
        else if (color == Color.Red)
        {
            sprRend.sprite = Resources.Load<Sprite>(SpritePath.Tile.red);
        }
        else if (color == Color.Yellow)
        {
            sprRend.sprite = Resources.Load<Sprite>(SpritePath.Tile.yellow);
        }
        else if (color == Color.Blue)
        {
            sprRend.sprite = Resources.Load<Sprite>(SpritePath.Tile.blue);
        }
        else
        {
            Assert.IsTrue(false, "Invalid tile color");
        }

        // Adjust scale
        gameObject.transform.localScale = new Vector2(
            srpiteWh.x / sprRend.size.x,
            srpiteWh.y / sprRend.size.y);
    }

    public T AddObject<T>() where T : MapObject, new()
    {
        T newObject = System.Activator.CreateInstance(typeof(T), rc) as T;
        objects.Add((MapObject)newObject);
        return newObject;
    }

    public void RemoveObject<T>()
    {
        for (int i = 0; i < objects.Count; ++i)
        {
            MapObject obj = objects[i];
            if (obj is T)
            {
                objects.Remove(obj);
                obj.component.CallDestroy();
                return;
            }
        }
    }

    public bool IsEmpty()
    {
        return objects.Count == 0;
    }
}
