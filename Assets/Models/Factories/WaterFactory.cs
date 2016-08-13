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
        [SerializeField]
        private Water.Settings _settings;
        //using water center for key is quite wrong actually, but works for now
        private Dictionary<string, Water> _waterDictionary { get; set; }

        public void Start()
        {
            _waterDictionary = new Dictionary<string, Water>();
        }

        public override void Create(Vector2 tileMercPos, JSONObject geo, Transform parent = null)
        {
            parent = parent ?? transform;
            foreach (var bb in geo["geometry"]["coordinates"].list)
            {
                var waterCorners = new List<Vector3>();
                var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;
                //var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                for (int i = 0; i < jo.list.Count - 1; i++)
                {
                    var c = jo.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                    waterCorners.Add(localMercPos.ToVector3xz());
                }
                
                try
                {
                    var id = geo["properties"]["id"].ToString();
                    var waterCenter = waterCorners.Aggregate((acc, cur) => acc + cur) / waterCorners.Count;
                    if (!_waterDictionary.ContainsKey(id))
                    {
                        for (int i = 0; i < waterCorners.Count; i++)
                        {
                            //using corner position relative to water center
                            waterCorners[i] = waterCorners[i] - waterCenter;
                        }
                        var water = new GameObject().AddComponent<Water>();

                        water.Id = geo["properties"]["id"].ToString();
                        water.Name = "water";
                        water.Type = geo["type"].str;
                        water.Kind = geo["properties"]["kind"].str;

                        water.Init(waterCorners, _settings);
                        water.name = "water";
                        water.transform.SetParent(parent, false);
                        water.transform.localPosition = waterCenter - new Vector3(0, 0.1f, 0);
                        _waterDictionary.Add(id, water);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

    }
}
