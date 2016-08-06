using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models.Factories
{
    public class BuildingFactory : MonoBehaviour
    {
        [SerializeField] private Building.Settings _settings;
        //using building center for key is quite wrong actually, but works for now
        private Dictionary<Vector3, Building> _buildingDictionary { get; set; }

        public void Start()
        {
            _buildingDictionary = new Dictionary<Vector3, Building>();
        }

        public void CreateBuilding(Vector2 tileMercPos, JSONObject geo, Transform parent = null)
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

            //prevents duplicates coming from different tiles
            //it sucks yea but works for now, cant use propery/id in json for some reason
            var uniqPos = new Vector3(bb.list[0][1].f, 0, bb.list[0][0].f);
            if (_buildingDictionary.ContainsKey(uniqPos))
            {
                return;
            }

            try
            {
                var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
                if (!_buildingDictionary.ContainsKey(buildingCenter))
                {
                    for (int i = 0; i < buildingCorners.Count; i++)
                    {
                        //using corner position relative to building center
                        buildingCorners[i] = buildingCorners[i] - buildingCenter;
                    }
                    var building = new GameObject().AddComponent<Building>();
                    var kind = "";
                    if (geo["properties"].HasField("landuse_kind"))
                        kind = geo["properties"]["landuse_kind"].str;
                    if (geo["properties"].HasField("name"))
                        building.Name = geo["properties"]["name"].str;

                    building.Init(buildingCorners, kind, _settings);

                    building.name = "building";
                    building.transform.parent = parent;
                    building.transform.localPosition = buildingCenter;
                    _buildingDictionary.Add(uniqPos, building);
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
