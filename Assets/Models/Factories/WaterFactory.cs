using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models.Factories
{
    public class WaterFactory : Factory
    {
        [SerializeField] private Water.Settings _settings;
        //using water center for key is quite wrong actually, but works for now
        private Dictionary<Vector3, Water> _waterDictionary { get; set; }

        public void Start()
        {
            _waterDictionary = new Dictionary<Vector3, Water>();
        }

        public override void Create(Vector2 tileMercPos, JSONObject geo, Transform parent = null)
        {
            parent = parent ?? transform;
            var waterCorners = new List<Vector3>();
            //foreach (var bb in geo["geometry"]["coordinates"].list)
            //{
            var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
            for (int i = 0; i < bb.list.Count - 1; i++)
            {
                var c = bb.list[i];
                var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                waterCorners.Add(localMercPos.ToVector3xz());
            }

            //prevents duplicates coming from different tiles
            //it sucks yea but works for now, cant use propery/id in json for some reason
            var uniqPos = new Vector3(bb.list[0][1].f, 0, bb.list[0][0].f);
            if (_waterDictionary.ContainsKey(uniqPos))
            {
                return;
            }

            try
            {
                var waterCenter = waterCorners.Aggregate((acc, cur) => acc + cur) / waterCorners.Count;
                if (!_waterDictionary.ContainsKey(waterCenter))
                {
                    for (int i = 0; i < waterCorners.Count; i++)
                    {
                        //using corner position relative to water center
                        waterCorners[i] = waterCorners[i] - waterCenter;
                    }
                    var water = new GameObject().AddComponent<Water>();

                    water.Init(waterCorners, _settings);
                    water.name = "water";
                    water.transform.SetParent(parent, false);
                    water.transform.localPosition = waterCenter - new Vector3(0, 0.1f, 0);
                    _waterDictionary.Add(uniqPos, water);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            //}
        }

    }
}
