using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Helpers;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Factories
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

        protected override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToEnum<WaterType>();
            var typeSettings = _settings.GetSettingsFor(kind);

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
            water.GetComponent<MeshRenderer>().material = typeSettings.Material;
            water.name = "water";

            foreach (var bb in geo["geometry"]["coordinates"].list)
            {
                var waterCorners = new List<Vector3>();
                var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;

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

            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();

            yield return water;
        }

        protected override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> items)
        {
            var main = new GameObject("Buildings Layer");
            var meshes = new Dictionary<WaterType, Tuple<List<Vector3>, List<int>>>();
            foreach (var geo in items.Where(x => Query(x)))
            {
                var kind = geo["properties"].HasField("kind")
                ? geo["properties"]["kind"].str.ConvertToEnum<WaterType>()
                : WaterType.Water;

                var typeSettings = _settings.GetSettingsFor(kind);

                //if we dont have a setting defined for that, it'Ll be merged to "unknown" 
                if (!_settings.HasSettingsFor(kind))
                    kind = WaterType.Water;

                if (!meshes.ContainsKey(kind))
                    meshes.Add(kind, new Tuple<List<Vector3>, List<int>>(new List<Vector3>(), new List<int>()));


                foreach (var bb in geo["geometry"]["coordinates"].list)
                {
                    var waterCorners = new List<Vector3>();
                    var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;

                    for (int i = 0; i < jo.list.Count - 1; i++)
                    {
                        var c = jo.list[i];
                        var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                        var localMercPos = dotMerc - tileMercPos;
                        waterCorners.Add(localMercPos.ToVector3());
                    }

                    //create mesh, actually just to get vertice&indices
                    //filling last two parameters, horrible call yea
                    CreateMesh(waterCorners, meshes[kind].Item1, meshes[kind].Item2);

                    //unity cant handle more than 65k on single mesh
                    //so we'll finish current and start a new one
                    if (meshes[kind].Item1.Count > 64000 || meshes[kind].Item2.Count > 64000)
                    {
                        CreateGameObject(kind, meshes[kind].Item1, meshes[kind].Item2, main);
                        meshes[kind].Item1.Clear();
                        meshes[kind].Item2.Clear();
                    }
                }
            }

            foreach (var group in meshes)
            {
                CreateGameObject(group.Key, group.Value.Item1, group.Value.Item2, main);
            }

            return main;
        }

        private void CreateMesh(List<Vector3> corners, List<Vector3> verts, List<int> indices)
        {
            var tris = new Triangulator(corners);
            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, 0, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));
        }

        private void CreateGameObject(WaterType kind, List<Vector3> vertices, List<int> indices, GameObject main)
        {
            var go = new GameObject(kind + " Waters");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshRenderer>().material = _settings.GetSettingsFor(kind).Material;
            go.transform.position += Vector3.up * Order;
            go.transform.SetParent(main.transform, true);
        }
    }
}
