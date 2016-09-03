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
        public override string XmlTag { get { return "water"; } }
        [SerializeField]
        private Water.Settings _settings;

        public override void Start()
        {
            Query = (geo) => geo["geometry"]["type"].str == "Polygon" || geo["geometry"]["type"].str == "MultiPolygon";
        }

        public override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
        {
            var go = new GameObject("water");
            var water = go.AddComponent<Water>();
            var mesh = water.GetComponent<MeshFilter>().mesh;
            var verts = new List<Vector3>();
            var indices = new List<int>();

            water.Id = geo["properties"]["id"].ToString();
            water.Name = "water";
            water.Type = geo["type"].str;
            water.Kind = geo["properties"]["kind"].str;
            water.SortKey = (int)geo["properties"]["sort_key"].f;

            water.Init();
            water.name = "water";

            foreach (var bb in geo["geometry"]["coordinates"].list)
            {
                var waterCorners = new List<Vector3>();
                var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;

                for (int i = 0; i < jo.list.Count - 1; i++)
                {
                    var c = jo.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc  - tileMercPos;
                    waterCorners.Add(localMercPos.ToVector3());
                }

                try
                {
                    var waterCenter = waterCorners.Aggregate((acc, cur) => acc + cur) / waterCorners.Count;

                    for (int i = 0; i < waterCorners.Count; i++)
                    {
                        waterCorners[i] = waterCorners[i] - waterCenter;
                    }

                    var tris = new Triangulator(waterCorners);
                    var c = verts.Count;
                    verts.AddRange(waterCorners.Select(x => x + waterCenter));
                    indices.AddRange(tris.Triangulate().Select(x => c + x));
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }

            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            yield return water;
        }

        public override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> geoList)
        {
            var items = geoList.Where(x => x["geometry"]["type"].str == "Polygon" || x["geometry"]["type"].str == "MultiPolygon");
            if (!items.Any())
                return null;

            var go = new GameObject("Water");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var verts = new List<Vector3>();
            var indices = new List<int>();

            CalculateVertices(tileMercPos, items, verts, indices);
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            go.GetComponent<MeshRenderer>().material = BaseMaterial;
            go.transform.position += Vector3.up * Order;
            return go;
        }

        private static void CalculateVertices(Vector2d tileMercPos, IEnumerable<JSONObject> items, List<Vector3> verts, List<int> indices)
        {
            foreach (var geo in items)
            {
                foreach (var bb in geo["geometry"]["coordinates"].list)
                {
                    var waterCorners = new List<Vector3>();
                    var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;
                    //var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                    for (int i = 0; i < jo.list.Count - 1; i++)
                    {
                        var c = jo.list[i];
                        var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                        var localMercPos = dotMerc - tileMercPos;
                        waterCorners.Add(localMercPos.ToVector3());
                    }

                    try
                    {
                        var waterCenter = waterCorners.Aggregate((acc, cur) => acc + cur) / waterCorners.Count;

                        for (int i = 0; i < waterCorners.Count; i++)
                        {
                            waterCorners[i] = waterCorners[i] - waterCenter;
                        }

                        var tris = new Triangulator(waterCorners);
                        var c = verts.Count;
                        verts.AddRange(waterCorners.Select(x => x + waterCenter));
                        indices.AddRange(tris.Triangulate().Select(x => c + x));
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }
                }
            }
        }

        private void CreateMesh(List<Vector3> ends, ref List<Vector3> verts, ref List<int> indices)
        {

        }

    }
}
