using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Assets.Helpers;
using Assets.Models.Factories;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Models
{
    public class CachedDynamicTileManager : DynamicTileManager
    {
        protected string CacheFolderPath;

        public override void Init(List<Factory> factories, World.Settings settings)
        {
            CacheFolderPath = Path.Combine(Application.dataPath, "MapzenGo/CachedTileData/");
            if (!Directory.Exists(CacheFolderPath))
                Directory.CreateDirectory(CacheFolderPath);
            base.Init(factories, settings);
        }

        protected override void LoadTile(Vector2 tileTms, Tile tile)
        {
            var url = string.Format(_mapzenUrl, _mapzenLayers, Zoom, tileTms.x, tileTms.y, _mapzenFormat, _key);

            var tilePath = Path.Combine(CacheFolderPath, Zoom + "_" + tileTms.x + "_" + tileTms.y);
            string mapData;
            if (File.Exists(tilePath))
            {
                var r = new StreamReader(tilePath, Encoding.Default);
                mapData = r.ReadToEnd();
                tile.ConstructTile(mapData);
            }
            else
            {
                ObservableWWW.Get(url).Subscribe(
                    success =>
                    {
                        var sr = File.CreateText(tilePath);
                        sr.Write(success);
                        sr.Close();
                        tile.ConstructTile(success);
                    },
                    error =>
                    {
                        Debug.Log(error);
                    });
            }
        }
    }
}
