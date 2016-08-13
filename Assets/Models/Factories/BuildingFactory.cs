using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models.Factories
{
    public class BuildingFactory : Factory
    {
        [SerializeField] private Building.Settings _settings;
        //using building center for key is quite wrong actually, but works for now
        private Dictionary<string, Building> _buildingDictionary { get; set; }

        public void Start()
        {
            _buildingDictionary = new Dictionary<string, Building>();
        }

        public override void Create(Vector2 tileMercPos, JSONObject geo, Transform parent = null)
        {
            parent = parent ?? transform;
            var buildingCorners = new List<Vector3>();
            //foreach (var bb in geo["geometry"]["coordinates"].list)
            //{
            var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
            for (int i = 0; i < bb.list.Count - 1; i++)
            {
                var c = bb.list[i];
                var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                buildingCorners.Add(localMercPos.ToVector3xz());
            }
            
            try
            {
                var id = geo["properties"]["id"].ToString();
                var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
                if (!_buildingDictionary.ContainsKey(id))
                {
                    for (int i = 0; i < buildingCorners.Count; i++)
                    {
                        //using corner position relative to building center
                        buildingCorners[i] = buildingCorners[i] - buildingCenter;
                    }
                    var building = new GameObject().AddComponent<Building>();
                    building.name = "building " + geo["properties"]["id"].ToString();
                    var kind = "";
                    if (geo["properties"].HasField("landuse_kind"))
                        kind = geo["properties"]["landuse_kind"].str;
                    if (geo["properties"].HasField("name"))
                        building.Name = geo["properties"]["name"].str;

                    building.Id = geo["properties"]["id"].ToString();
                    building.Name = "building";
                    building.Type = geo["type"].str;
                    building.SortKey = (int) geo["properties"]["sort_key"].f;
                    building.Kind = geo["properties"]["kind"].str;
                    building.LanduseKind = kind;
                    building.Init(buildingCorners, _settings);

                    building.transform.SetParent(parent, false);
                    building.transform.localPosition = buildingCenter;
                    _buildingDictionary.Add(id, building);
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
