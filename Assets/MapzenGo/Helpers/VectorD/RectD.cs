using UnityEngine;
using System.Collections;

public class RectD
{
    public Vector2d Min { get; private set; }
    public Vector2d Size { get; private set; }

    public Vector2d Center
    {
        get
        {
            return new Vector2d(Min.x + Size.x / 2, Min.y + Size.y / 2);
        }
        set
        {
            Min = new Vector2d(value.x - Size.x / 2, value.x - Size.y / 2);
        }
    }

    public double Height
    {
        get { return Size.y; }
    }

    public double Width
    {
        get { return Size.x; }
    }

    public RectD(Vector2d min, Vector2d size)
    {
        Min = min;
        Size = size;
    }
}
