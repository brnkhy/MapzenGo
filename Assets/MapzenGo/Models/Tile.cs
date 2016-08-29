using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Helpers;
using Assets.Models;
using Assets.Models.Factories;
using UnityEngine;
using UniRx;

namespace Assets
{
    public class Tile : MonoBehaviour
    {
        public string MapImageUrlBase = "http://b.tile.openstreetmap.org/";

        [SerializeField]
        public Rect Rect;

        private List<Factory> _factories;
        private Settings _settings;

        public Tile Initialize(List<Factory> factory, Settings settings)
        {
            _factories = factory;
            _settings = settings;
            Rect = GM.TileBounds(_settings.TileTms, _settings.Zoom);
            return this;
        }

        public void ConstructTile(string text)
        {
            ConstructAsync(text);
        }

        private void ConstructAsync(string text)
        {
            string url;
            var heavyMethod = Observable.Start(() => new JSONObject(text));

            heavyMethod.ObserveOnMainThread().Subscribe(mapData =>
            {
                if (!this) // checks if tile still exists and haven't destroyed yet
                    return;

                var go = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
                go.name = "map";
                go.SetParent(transform, true);
                go.localScale = new Vector3(Rect.width, Rect.width, 1);
                go.rotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
                go.localPosition = Vector3.zero;
                go.localPosition -= new Vector3(0, 1, 0);
                var rend = go.GetComponent<Renderer>();
                rend.material = _settings.Material;
                
                if (_settings.LoadImages)
                {
                    rend.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT5, false);
                    rend.material.color = new Color(1f, 1f, 1f, 1f);
                    url = MapImageUrlBase + _settings.Zoom + "/" + _settings.TileTms.x + "/" + _settings.TileTms.y + ".png";
                    ObservableWWW.GetWWW(url).Subscribe(x =>
                    {
                        x.LoadImageIntoTexture((Texture2D)rend.material.mainTexture);
                    });
                }

                RunFactories(mapData);

                //StartCoroutine(CreateBuildings(mapData["buildings"], _settings.TileCenter));
                //StartCoroutine(CreateRoads(mapData["roads"], _settings.TileCenter));
                //StartCoroutine(CreateWater(mapData["water"], _settings.TileCenter));
            });
        }

        private void RunFactories(JSONObject mapData)
        {
            foreach (var factory in _factories)
            {
                if (_settings.UseLayers)
                {
                    if (!mapData.HasField(factory.XmlTag))
                        continue;

                    var b = factory.CreateLayer(_settings.TileCenter, mapData[factory.XmlTag]["features"].list);
                    if(b) //getting a weird error without this, no idea really
                        b.transform.SetParent(transform, false);
                }
                else
                {
                    var fac = factory;
                    foreach (var entity in mapData[factory.XmlTag]["features"].list.Where(x => fac.Query(x)).SelectMany(geo => fac.Create(_settings.TileCenter, geo)))
                    {
                        if (entity != null)
                        {
                            entity.transform.SetParent(transform, false);
                        }
                        //I'm not keeping these anywhere for now but you can always create a list or something here and save them
                    }
                }
            }
        }

        public class Settings
        {
            public int Zoom { get; set; }
            public Vector2 TileTms { get; set; }
            public Vector3 TileCenter { get; set; }
            public bool LoadImages { get; set; }
            public bool UseLayers { get; set; }
            public Material Material { get; set; }
        }
    }
}
