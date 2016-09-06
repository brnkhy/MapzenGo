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
        

        [SerializeField]
        public RectD Rect;

        private List<Factory> _factories;
        public Settings _settings;

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
                
                RunFactories(mapData);
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
                    if (b) //getting a weird error without this, no idea really
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
            public Vector2d TileTms { get; set; }
            public Vector3d TileCenter { get; set; }
            public bool LoadImages { get; set; }
            public bool UseLayers { get; set; }
            public Material Material { get; set; }
        }
    }
}
