using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Models
{
    public class RoadJunction
    {
        public Vector2 Coordinate { get; set; }
        public List<RoadJunction> Neighbours { get; set; }

        public RoadJunction(Vector2 c)
        {
            Neighbours = new List<RoadJunction>();
            Coordinate = c;
        }
    }
}
