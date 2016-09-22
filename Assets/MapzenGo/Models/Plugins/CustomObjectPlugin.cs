using Assets.MapzenGo.Models.Plugins;
using MapzenGo.Helpers;
using MapzenGo.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapzenGo.Models.Plugins
{
    public class CustomObjectPlugin : Plugin
    {
        private readonly List<Vector2d> _customObjects = new List<Vector2d>()
        {
            //new Vector2d(33.830426, -84.264398),
            //new Vector2d(33.830435, -84.262948),
            new Vector2d(33.829375, -84.262900),
            //new Vector2d(33.829582, -84.264110),
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
                    go.transform.localScale = Vector3.one * 50;
                    go.transform.SetParent(tile.transform, false);
                }
            }
        }
    }
}