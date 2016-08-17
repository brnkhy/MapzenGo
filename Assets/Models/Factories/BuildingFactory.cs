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
    public class BuildingFactory : Factory
    {
        private HashSet<string> _active = new HashSet<string>();

        [SerializeField]
        private Building.Settings _settings;

        public override IEnumerable<MonoBehaviour> Create(Vector2 tileMercPos, JSONObject geo)
        {
            var key = geo["properties"]["id"].ToString();
            if (!_active.Contains(key))
            {
                var buildingCorners = new List<Vector3>();
                //foreach (var bb in geo["geometry"]["coordinates"].list)
                //{
                Building building = null;
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
                    building = new GameObject().AddComponent<Building>();
                    var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
                    for (int i = 0; i < buildingCorners.Count; i++)
                    {
                        //using corner position relative to building center
                        buildingCorners[i] = buildingCorners[i] - buildingCenter;
                    }

                    building.name = "building " + geo["properties"]["id"].ToString();
                    var kind = "";
                    if (geo["properties"].HasField("landuse_kind"))
                        kind = geo["properties"]["landuse_kind"].str;
                    if (geo["properties"].HasField("name"))
                        building.Name = geo["properties"]["name"].str;

                    building.Id = geo["properties"]["id"].ToString();
                    building.Type = geo["type"].str;
                    building.SortKey = (int)geo["properties"]["sort_key"].f;
                    building.Kind = kind;
                    building.LanduseKind = kind;
                    building.Init(buildingCorners, _settings);
                    building.transform.localPosition = buildingCenter;

                    _active.Add(building.Id);
                    building.OnDestroyAsObservable().Subscribe(x => { _active.Remove(building.Id); });
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }

                yield return building;
                //}
            }
        }

        public override GameObject CreateLayer(Vector2 tileMercPos, List<JSONObject> geoList)
        {
            var go = new GameObject();
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var verts = new List<Vector3>();
            var indices = new List<int>();

            foreach (var geo in geoList)
            {
                var key = geo["properties"]["id"].ToString();
                if (!_active.Contains(key))
                {
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
                        //building = new GameObject().AddComponent<Building>();
                        var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
                        for (int i = 0; i < buildingCorners.Count; i++)
                        {
                            //using corner position relative to building center
                            buildingCorners[i] = buildingCorners[i] - buildingCenter;
                        }

                        var m = Building.CreateMesh(buildingCorners, _settings);
                        var c = verts.Count;
                        verts.AddRange(m.vertices.Select(x => x + buildingCenter));
                        indices.AddRange(m.triangles.Select(x => x + c));
                        _active.Add(key);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }

                    //}
                }
            }
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return go;
        }
    }
}
