using System;
using System.Collections.Generic;
using Assets.Helpers;
using UniRx;
using UnityEngine;

namespace Assets.MapzenGo.Models.Plugins
{
    public class BackgroundImagePlugin : TilePlugin
    {
        public string MapImageUrlBase = "http://b.tile.openstreetmap.org/";

        public override void Run(Tile tile)
        {
            base.Run(tile);

            var go = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
            go.name = "map";
            go.SetParent(tile.transform, true);
            go.localScale = new Vector3((float)tile.Rect.Width, (float)tile.Rect.Width, 1);
            go.rotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
            go.localPosition = Vector3.zero;
            go.localPosition -= new Vector3(0, 1, 0);
            var rend = go.GetComponent<Renderer>();
            rend.material = tile._settings.Material;

            if (tile._settings.LoadImages)
            {
                var url = MapImageUrlBase + tile._settings.Zoom + "/" + tile._settings.TileTms.x + "/" + tile._settings.TileTms.y + ".png";
                ObservableWWW.GetWWW(url).Subscribe(
                    success =>
                    {
                        rend.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT5, false);
                        rend.material.color = new Color(1f, 1f, 1f, 1f);
                        success.LoadImageIntoTexture((Texture2D)rend.material.mainTexture);
                    },
                    error =>
                    {
                        Debug.Log(error);
                    });
            }
        }
    }
}
