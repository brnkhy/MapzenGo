using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Helpers;
using MapzenGo.Models.Factories;
using MapzenGo.Models.Plugins;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] public float Latitude = 39.921864f;
        [SerializeField] public float Longitude = 32.818442f;
        [SerializeField] public int Range = 3;
        [SerializeField] public int Zoom = 16;
        [SerializeField] public float TileSize = 100;

        protected readonly string _mapzenUrl = "https://vector.mapzen.com/osm/{0}/{1}/{2}/{3}.{4}?api_key={5}";
        [SerializeField] protected string _key = "vector-tiles-5sBcqh6"; //try getting your own key if this doesn't work
        protected string _mapzenLayers;
        [SerializeField] protected Material MapMaterial;
        protected readonly string _mapzenFormat = "json";
        protected Transform TileHost;

        private List<Plugin> _plugins;

        protected Dictionary<Vector2d, Tile> Tiles; //will use this later on
        protected Vector2d CenterTms; //tms tile coordinate
        protected Vector2d CenterInMercator; //this is like distance (meters) in mercator 

        public virtual void Start()
        {
            if (MapMaterial == null)
                MapMaterial = Resources.Load<Material>("Ground");

            InitFactories();
            InitLayers();

            var v2 = GM.LatLonToMeters(Latitude, Longitude);
            var tile = GM.MetersToTile(v2, Zoom);

            TileHost = new GameObject("Tiles").transform;
            TileHost.SetParent(transform, false);

            Tiles = new Dictionary<Vector2d, Tile>();
            CenterTms = tile;
            CenterInMercator = GM.TileBounds(CenterTms, Zoom).Center;

            LoadTiles(CenterTms, CenterInMercator);

            var rect = GM.TileBounds(CenterTms, Zoom);
            transform.localScale = Vector3.one * (float)(TileSize / rect.Width);
        }

        private void InitLayers()
        {
            var layers = new List<string>();
            foreach (var plugin in _plugins.OfType<Factory>())
            {
                if (layers.Contains(plugin.XmlTag)) continue;
                layers.Add(plugin.XmlTag);
            }
            _mapzenLayers = string.Join(",", layers.ToArray());
        }

        private void InitFactories()
        {
            _plugins = new List<Plugin>();
            foreach (var plugin in GetComponentsInChildren<Plugin>())
            {
                _plugins.Add(plugin);
            }
        }

        protected void LoadTiles(Vector2d tms, Vector2d center)
        {
            for (int i = -Range; i <= Range; i++)
            {
                for (int j = -Range; j <= Range; j++)
                {
                    var v = new Vector2d(tms.x + i, tms.y + j);
                    if (Tiles.ContainsKey(v))
                        continue;
                    StartCoroutine(CreateTile(v, center));
                }
            }
        }

        protected virtual IEnumerator CreateTile(Vector2d tileTms, Vector2d centerInMercator)
        {
            var rect = GM.TileBounds(tileTms, Zoom);
            var tile = new GameObject("tile " + tileTms.x + "-" + tileTms.y).AddComponent<Tile>();

            tile.Zoom = Zoom;
            tile.TileTms = tileTms;
            tile.TileCenter = rect.Center;
            tile.Material = MapMaterial;
            tile.Rect = GM.TileBounds(tileTms, Zoom);

            Tiles.Add(tileTms, tile);
            tile.transform.position = (rect.Center - centerInMercator).ToVector3();
            tile.transform.SetParent(TileHost, false);
            LoadTile(tileTms, tile);
            
            yield return null;
        }

        protected virtual void LoadTile(Vector2d tileTms, Tile tile)
        {
            var url = string.Format(_mapzenUrl, _mapzenLayers, Zoom, tileTms.x, tileTms.y, _mapzenFormat, _key);
            Debug.Log(url);
            ObservableWWW.Get(url)
                .Subscribe(
                    text => { ConstructTile(text, tile); }, //success
                    exp => Debug.Log("Error fetching -> " + url)); //failure
        }

        protected void ConstructTile(string text, Tile tile)
        {
            var heavyMethod = Observable.Start(() => new JSONObject(text));

            heavyMethod.ObserveOnMainThread().Subscribe(mapData =>
            {
                if (!tile) // checks if tile still exists and haven't destroyed yet
                    return;
                tile.Data = mapData;

                foreach (var factory in _plugins)
                {
                    factory.Create(tile);
                }
            });
        }
    }
}
