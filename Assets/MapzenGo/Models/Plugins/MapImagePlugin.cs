using System;
using System.Collections.Generic;
using MapzenGo.Models.Plugins;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Plugins
{
    public class MapImagePlugin : Plugin
    {
        public enum TileServices
        {
            Default,
            Satellite,
            Terrain,
            Toner,
            Watercolor
        }

        public TileServices TileService = TileServices.Default;

        private string[] TileServiceUrls = new string[] {
            "http://b.tile.openstreetmap.org/",
            "http://b.tile.openstreetmap.us/usgs_large_scale/",
            "http://tile.stamen.com/terrain-background/",
            "http://a.tile.stamen.com/toner/",
            "https://stamen-tiles.a.ssl.fastly.net/watercolor/"
        };

        public override void Create(Tile tile)
        {
            base.Create(tile);

            var go = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
            go.name = "map";
            go.SetParent(tile.transform, true);
            go.localScale = new Vector3((float)tile.Rect.Width, (float)tile.Rect.Width, 1);
            go.rotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
            go.localPosition = Vector3.zero;
            go.localPosition -= new Vector3(0, 1, 0);
            var rend = go.GetComponent<Renderer>();
            rend.material = tile.Material;

            var url = TileServiceUrls[(int)TileService] + tile.Zoom + "/" + tile.TileTms.x + "/" + tile.TileTms.y + ".png";
            ObservableWWW.GetWWW(url).Subscribe(
                success =>
                {
                    if (rend)
                    {
                        rend.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT5, false);
                        rend.material.color = new Color(1f, 1f, 1f, 1f);
                        success.LoadImageIntoTexture((Texture2D) rend.material.mainTexture);
                    }
                },
                error =>
                {
                    Debug.Log(error);
                });

        }
    }
}
