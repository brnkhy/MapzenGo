using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Models.Factories
{
    public class WaterFactory : Factory
    {
        [SerializeField] private Water.Settings _settings;
        
        public override IEnumerable<MonoBehaviour> Create(Vector2 tileMercPos, JSONObject geo)
        {
            foreach (var bb in geo["geometry"]["coordinates"].list)
            {
                Water water = null;
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
                    water = new GameObject().AddComponent<Water>();
                    var waterCenter = waterCorners.Aggregate((acc, cur) => acc + cur) / waterCorners.Count;
                    
                    for (int i = 0; i < waterCorners.Count; i++)
                    {
                        waterCorners[i] = waterCorners[i] - waterCenter;
                    }

                    water.Id = geo["properties"]["id"].ToString();
                    water.Name = "water";
                    water.Type = geo["type"].str;
                    water.Kind = geo["properties"]["kind"].str;

                    water.Init(waterCorners, _settings);
                    water.name = "water";
                    water.transform.localPosition = waterCenter - new Vector3(0, 0.1f, 0);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
                yield return water;
            }
        }

    }
}
