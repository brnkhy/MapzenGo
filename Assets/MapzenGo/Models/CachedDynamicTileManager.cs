using System.IO;
using System.Text;
using MapzenGo.Helpers;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models
{
    public class CachedDynamicTileManager : DynamicTileManager
    {
        public string RelativeCachePath = "../CachedTileData/{0}/";
        protected string CacheFolderPath;

        public override void Start()
        {
            CacheFolderPath = Path.Combine(Application.persistentDataPath, RelativeCachePath);
            CacheFolderPath = CacheFolderPath.Format(Zoom);
            if (!Directory.Exists(CacheFolderPath))
                Directory.CreateDirectory(CacheFolderPath);
            base.Start();
        }

        protected override void LoadTile(Vector2d tileTms, Tile tile)
        {
            var url = string.Format(_mapzenUrl, _mapzenLayers, Zoom, tileTms.x, tileTms.y, _mapzenFormat, _key);
            //this is temporary (hopefully), cant just keep adding stuff to filenames
            var tilePath = Path.Combine(CacheFolderPath, _mapzenLayers.Replace(',', '_') + "_" + tileTms.x + "_" + tileTms.y);
            if (File.Exists(tilePath))
            {
                using (var r = new StreamReader(tilePath, Encoding.Default))
                {
                    var mapData = r.ReadToEnd();
                    ConstructTile(mapData, tile);
                }
            }
            else
            {
                ObservableWWW.Get(url).Subscribe(
                    success =>
                    {
                        var sr = File.CreateText(tilePath);
                        sr.Write(success);
                        sr.Close();
                        ConstructTile(success, tile);
                    },
                    error =>
                    {
                        Debug.Log(error);
                    });
            }
        }
    }
}
