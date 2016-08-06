using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Models
{
    public class Road
    {
        public Vector2 Key { get; set; }
        public Vector2 V1 { get; set; }
        public Vector2 V2 { get; set; }
        public int Width { get; set; }

        public Road(Vector2 v1, Vector2 v2, int width = 1)
        {
            V1 = v1;
            V2 = v2;
            Width = width;
            Key = new Vector2((v1.x + v2.x) / 2, (v1.y + v2.y) / 2);
        }

        public bool Contains(Vector2 b)
        {
            if (b.x > Math.Min(V1.x, V2.x) && b.x < Math.Max(V1.x, V2.x) &&
                b.y > Math.Min(V1.y, V2.y) && b.y < Math.Max(V1.y, V2.y))
            {
                return
                    Math.Abs(((b.y - V1.y)) / ((b.x - V1.x)) -
                             ((V2.y - V1.y)) / ((V2.x - V1.x))) < 0.1;
            }
            return false;
        }

    }
}
