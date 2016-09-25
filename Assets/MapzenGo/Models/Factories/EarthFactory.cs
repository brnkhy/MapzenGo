using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Helpers;
using MapzenGo.Models.Settings;
using TriangleNet;
using TriangleNet.Geometry;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Factories
{
    public class EarthFactory : Factory
    {
        public override string XmlTag { get { return "earth"; } }
        [SerializeField]
        protected EarthFactorySettings FactorySettings;
        public override void Start()
        {
            base.Start();
            Query = (geo) => geo["geometry"]["type"].str == "Polygon" || geo["geometry"]["type"].str == "MultiPolygon";
        }

        protected override IEnumerable<MonoBehaviour> Create(Tile tile, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToEarthType();
            var typeSettings = FactorySettings.GetSettingsFor<EarthSettings>(kind);

            var go = new GameObject("earth");
            var Earth = go.AddComponent<Earth>();
            var mesh = Earth.GetComponent<MeshFilter>().mesh;
            var md = new MeshData();

            SetProperties(geo, Earth, typeSettings);

            foreach (var bb in geo["geometry"]["coordinates"].list)
            {
                var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;
                var count = jo.list.Count - 1;
                if (count < 3)
                    continue;

                var inp = new InputGeometry(count);

                for (int i = 0; i < count; i++)
                {
                    var c = jo.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc - tile.Rect.Center;
                    inp.AddPoint(localMercPos.x, localMercPos.y);
                    inp.AddSegment(i, (i + 1) % count);
                }
                
                CreateMesh(inp, md);
            }

            mesh.vertices = md.Vertices.ToArray();
            mesh.triangles = md.Indices.ToArray();
            mesh.SetUVs(0, md.UV);
            mesh.RecalculateNormals();

            yield return Earth;
        }

        private static void SetProperties(JSONObject geo, Earth earth, EarthSettings typeSettings)
        {
            earth.Id = geo["properties"]["id"].ToString();
            if (geo["properties"].HasField("name"))
                earth.Name = geo["properties"]["name"].str;
            earth.Type = geo["type"].str;
            earth.Kind = geo["properties"]["kind"].str;
            earth.SortKey = (int) geo["properties"]["sort_key"].f;
            earth.GetComponent<MeshRenderer>().material = typeSettings.Material;
            earth.name = "earth";
        }

        protected override GameObject CreateLayer(Tile tile, List<JSONObject> items)
        {
            var main = new GameObject("Earth Layer");
            var meshes = new Dictionary<EarthType, MeshData>();
            foreach (var geo in items.Where(x => Query(x)))
            {
                var kind = geo["properties"].HasField("kind")
                ? geo["properties"]["kind"].str.ConvertToEarthType()
                : EarthType.Earth;

                var typeSettings = FactorySettings.GetSettingsFor<EarthSettings>(kind);

                //if we dont have a setting defined for that, it'Ll be merged to "unknown" 
                if (!FactorySettings.HasSettingsFor(kind))
                    kind = EarthType.Earth;

                if (!meshes.ContainsKey(kind))
                    meshes.Add(kind, new MeshData());

                foreach (var bb in geo["geometry"]["coordinates"].list)
                {
                    var jo = (bb.list[0].list[0].IsArray) ? bb.list[0] : bb;
                    var count = jo.list.Count - 1;
                    if (count < 3)
                        continue;

                    var inp = new InputGeometry(count);

                    for (int i = 0; i < count; i++)
                    {
                        var c = jo.list[i];
                        var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                        var localMercPos = dotMerc - tile.Rect.Center;
                        inp.AddPoint(localMercPos.x, localMercPos.y);
                        inp.AddSegment(i, (i + 1) % count);
                    }
                    
                    //create mesh, actually just to get vertice&indices
                    //filling last two parameters, horrible call yea
                    CreateMesh(inp, meshes[kind]);

                    //unity cant handle more than 65k on single mesh
                    //so we'll finish current and start a new one
                    if (meshes[kind].Vertices.Count > 64000)
                    {
                        CreateGameObject(kind, meshes[kind], main.transform);
                        meshes[kind] = new MeshData();
                    }
                }
            }

            foreach (var group in meshes)
            {
                CreateGameObject(group.Key, group.Value, main.transform);
            }

            return main;
        }
        
        private void CreateMesh(InputGeometry corners, MeshData meshdata)
        {
            var mesh = new TriangleNet.Mesh();
            mesh.Behavior.Algorithm = TriangulationAlgorithm.SweepLine;
            mesh.Behavior.Quality = true;
            mesh.Triangulate(corners);

            var vertsStartCount = meshdata.Vertices.Count;
            meshdata.Vertices.AddRange(corners.Points.Select(x => new Vector3((float)x.X, 0, (float)x.Y)).ToList());

            foreach (var tri in mesh.Triangles)
            {
                meshdata.Indices.Add(vertsStartCount + tri.P1);
                meshdata.Indices.Add(vertsStartCount + tri.P0);
                meshdata.Indices.Add(vertsStartCount + tri.P2);
            }
        }

        private void CreateGameObject(EarthType kind, MeshData meshdata, Transform parent)
        {
            var go = new GameObject(kind + " Earths");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            mesh.vertices = meshdata.Vertices.ToArray();
            mesh.triangles = meshdata.Indices.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshRenderer>().material = FactorySettings.GetSettingsFor<EarthSettings>(kind).Material;
            go.transform.position += Vector3.up * Order;
            go.transform.SetParent(parent, true);
        }
    }
}
