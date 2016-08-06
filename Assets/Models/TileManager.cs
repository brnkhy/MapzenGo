using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Helpers;
using Assets.Models;
using Assets.Models.Factories;
using UniRx;
using UnityEngine;

namespace Assets
{
    public class TileManager : MonoBehaviour
    {
        private readonly string _mapzenUrl = "https://vector.mapzen.com/osm/{0}/{1}/{2}/{3}.{4}?api_key={5}";
        private readonly string _key = "vector-tiles-5sBcqh6"; //try getting your own key if this doesn't work
        private readonly string _mapzenLayers = "buildings,roads";
        private readonly string _mapzenFormat = "json";

        protected BuildingFactory BuildingFactory;
        protected RoadFactory RoadFactory;
        protected Transform TileHost;

        protected bool LoadImages;
        protected int Zoom = 16; //detail level of TMS system
        protected int Range = 1;
        protected Dictionary<Vector2, Tile> Tiles; //will use this later on
        protected Vector2 CenterTms; //tms tile coordinate
        protected Vector2 CenterInMercator; //this is like distance (meters) in mercator 
        
        public virtual void Init(BuildingFactory buildingFactory, RoadFactory roadFactory, World.Settings settings)
        {
            var v2 = GM.LatLonToMeters(settings.Lat, settings.Long);
            var tile = GM.MetersToTile(v2, settings.DetailLevel);

            TileHost = new GameObject("Tiles").transform;
            TileHost.SetParent(transform, false);

            BuildingFactory = buildingFactory;
            RoadFactory = roadFactory;
            Tiles = new Dictionary<Vector2, Tile>();
            CenterTms = tile;
            CenterInMercator = GM.TileBounds(CenterTms, Zoom).center;
            Zoom = settings.DetailLevel;
            Range = settings.Range;
            LoadImages = settings.LoadImages;

            LoadTiles(CenterTms, CenterInMercator);
        }

        protected void LoadTiles(Vector2 tms, Vector2 center)
        {
            for (int i = -Range; i <= Range; i++)
            {
                for (int j = -Range; j <= Range; j++)
                {
                    var v = new Vector2(tms.x + i, tms.y + j);
                    if (Tiles.ContainsKey(v))
                        continue;
                    StartCoroutine(CreateTile(v, center));
                }
            }
        }

        protected virtual IEnumerator CreateTile(Vector2 tileTms, Vector2 centerInMercator)
        {
            var rect = GM.TileBounds(tileTms, Zoom);
            var tile = new GameObject("tile " + tileTms.x + "-" + tileTms.y)
                .AddComponent<Tile>()
                .Initialize(BuildingFactory,
                      RoadFactory,
                      new Tile.Settings()
                      {
                          Zoom = Zoom,
                          TileTms = tileTms,
                          TileCenter = rect.center,
                          LoadImages = LoadImages
                      });
            Tiles.Add(tileTms, tile);
            tile.transform.SetParent(TileHost, true);
            tile.transform.position = (rect.center - centerInMercator).ToVector3xz();
            LoadTile(tileTms, tile);

            yield return null;
        }

        private void LoadTile(Vector2 tileTms, Tile tile)
        {
            var url = string.Format(_mapzenUrl, _mapzenLayers, Zoom, tileTms.x, tileTms.y, _mapzenFormat, _key);
            ObservableWWW.Get(url)
                .Subscribe(
                    tile.ConstructTile, //success
                    exp => Debug.Log("Error fetching -> " + url)); //failure
        }
    }
}
