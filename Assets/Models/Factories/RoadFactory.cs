using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models.Factories
{
    public class RoadFactory : Factory
    {
        [SerializeField] private Road.Settings _settings;

        public override IEnumerable<MonoBehaviour> Create(Vector2 tileMercPos, JSONObject geo)
        {
            var roadEnds = new List<Vector3>();
            if (geo["geometry"]["type"].str == "LineString")
            {
                for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var c = geo["geometry"]["coordinates"][i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                    roadEnds.Add(localMercPos.ToVector3xz());
                }
                yield return CreateRoadSegment(geo, roadEnds);
            }
            else if (geo["geometry"]["type"].str == "MultiLineString")
            {
                for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    roadEnds.Clear();
                    var c = geo["geometry"]["coordinates"][i];
                    for (var j = 0; j < c.list.Count; j++)
                    {
                        var seg = c[j];
                        var dotMerc = GM.LatLonToMeters(seg[1].f, seg[0].f);
                        var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                        roadEnds.Add(localMercPos.ToVector3xz());
                    }
                    yield return CreateRoadSegment(geo, roadEnds);
                }
            }
        }

        private MonoBehaviour CreateRoadSegment(JSONObject geo, List<Vector3> roadEnds)
        {
            if (roadEnds.Count < 2)
                return null;

            var road = new GameObject("road").AddComponent<Road>();
            road.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Road");
            try
            {
                road.Initialize(roadEnds, geo["properties"]["kind"].str, _settings);
                road.Id = geo["properties"]["id"].ToString();
                road.Type = geo["type"].str;
                road.Kind = geo["properties"]["kind"].str;
                if(geo["properties"].HasField("name"))
                    road.Name = geo["properties"]["name"].str;
                //road.Type = geo["properties"]["kind"].str;
                road.transform.localScale = Vector3.one;
                return road;
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return null;
            }
        }
    }
}
