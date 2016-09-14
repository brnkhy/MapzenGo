using System;
using System.Collections.Generic;
using MapzenGo.Helpers;
using MapzenGo.Models;
using UnityEngine;

namespace MapzenGo.Models.Plugins
{
    public class CustomObjectPlugin : Plugin
    {
        private readonly List<Vector2d> _customObjects = new List<Vector2d>()
        {
            new Vector2d(40.753176, -73.982229),
            new Vector2d(40.769759, -73.975537),
            new Vector2d(40.740304, -73.972425),
            new Vector2d(40.728664, -74.032011),
        };

        public override void Create(Tile tile)
        {
            base.Create(tile);

            foreach (var pos in _customObjects)
            {
                var meters = GM.LatLonToMeters(pos.x, pos.y);

                if (tile.Rect.Contains(meters))
                {
                    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    go.transform.position = (meters - tile.Rect.Center).ToVector3();
                    go.transform.localScale = Vector3.one * 1000;
                    go.transform.SetParent(tile.transform, false);
                }
            }
        }
    }
}
