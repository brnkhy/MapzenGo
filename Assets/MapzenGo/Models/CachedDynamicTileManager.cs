using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using MapzenGo.Helpers;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models
{
    public class CachedDynamicTileManager : DynamicTileManager
    {
        public string RelativeCachePath = "../CachedTileData/{0}/";
        protected string CacheFolderPath;
        private Queue<Tile> _readyToProcess;

        public override void Start()
        {
            _readyToProcess = new Queue<Tile>();
#if UNITY_ANDROID || UNITY_IPHONE
            CacheFolderPath = Path.Combine(Application.persistentDataPath, RelativeCachePath);
#else
            CacheFolderPath = Path.Combine(Application.dataPath, RelativeCachePath);
#endif
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
