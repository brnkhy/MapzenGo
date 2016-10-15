using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapzenGo.Helpers;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Factories
{
    public class RoadFactory : Factory
    {
        public override string XmlTag { get { return "roads"; } }
        [SerializeField] protected RoadFactorySettings FactorySettings;

        public override void Start()
        {
            base.Start();
            Query = (geo) => geo["geometry"]["type"].str == "LineString" || geo["geometry"]["type"].str == "MultiLineString";
        }

        protected override IEnumerable<MonoBehaviour> Create(Tile tile, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToRoadType();
            if (!FactorySettings.HasSettingsFor(kind) && !JustDrawEverythingFam)
                yield break;

            var typeSettings = FactorySettings.GetSettingsFor<RoadSettings>(kind);

            if (geo["geometry"]["type"].str == "LineString")
            {
                var road = new GameObject("road").AddComponent<Road>();
                var mesh = road.GetComponent<MeshFilter>().mesh;
                var roadEnds = new List<Vector3>();
                var md = new MeshData();

                for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var c = geo["geometry"]["coordinates"][i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc - tile.Rect.Center;
                    roadEnds.Add(localMercPos.ToVector3());
                }

                CreateMesh(roadEnds, typeSettings, md);

                mesh.vertices = md.Vertices.ToArray();
                mesh.triangles = md.Indices.ToArray();
                mesh.SetUVs(0, md.UV);
                mesh.RecalculateNormals();

                road.GetComponent<MeshRenderer>().material = typeSettings.Material;

                road.Id = geo["properties"]["id"].ToString();
                road.Type = geo["type"].str;
                road.Kind = geo["properties"]["kind"].str;
                road.SortKey = (int)geo["properties"]["sort_rank"].f;
                if (geo["properties"].HasField("name"))
                    road.Name = geo["properties"]["name"].str;

                road.transform.position += Vector3.up * road.SortKey / 100;
                yield return road;
            }
            else if (geo["geometry"]["type"].str == "MultiLineString")
            {
                for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var road = new GameObject("Roads").AddComponent<Road>();
                    var mesh = road.GetComponent<MeshFilter>().mesh;
                    var roadEnds = new List<Vector3>();
                    var md = new MeshData();

                    roadEnds.Clear();
                    var c = geo["geometry"]["coordinates"][i];
                    for (var j = 0; j < c.list.Count; j++)
                    {
                        var seg = c[j];
                        var dotMerc = GM.LatLonToMeters(seg[1].f, seg[0].f);
                        var localMercPos = dotMerc - tile.Rect.Center;
                        roadEnds.Add(localMercPos.ToVector3());
                    }

                    SetProperties(geo, road);

                    CreateMesh(roadEnds, typeSettings, md);
                    mesh.vertices = md.Vertices.ToArray();
                    mesh.triangles = md.Indices.ToArray();
                    mesh.SetUVs(0, md.UV);
                    mesh.RecalculateNormals();

                    road.GetComponent<MeshRenderer>().material = typeSettings.Material;



                    road.transform.position += Vector3.up * road.SortKey / 100;
                    yield return road;
                }
            }
        }

        private static void SetProperties(JSONObject geo, Road road)
        {
            road.Id = geo["properties"]["id"].ToString();
            road.Type = geo["type"].str;
            road.Kind = geo["properties"]["kind"].str;
            road.SortKey = (int)geo["properties"]["sort_rank"].f;
            if (geo["properties"].HasField("name"))
                road.Name = geo["properties"]["name"].str;
        }

        protected override GameObject CreateLayer(Tile tile, List<JSONObject> geoList)
        {
            var main = new GameObject("Roads");
            var _meshes = new Dictionary<RoadType, MeshData>();

            foreach (var geo in geoList.Where(x => Query(x)))
            {
                var kind = geo["properties"]["kind"].str.ConvertToRoadType();
                if (!FactorySettings.HasSettingsFor(kind) && !JustDrawEverythingFam)
                    continue;

                if (!_meshes.ContainsKey(kind))
                    _meshes.Add(kind, new MeshData());

                var settings = FactorySettings.GetSettingsFor<RoadSettings>(kind);
                var roadEnds = new List<Vector3>();

                if (geo["geometry"]["type"].str == "LineString")
                {
                    for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                    {
                        var c = geo["geometry"]["coordinates"][i];
                        var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                        var localMercPos = dotMerc - tile.Rect.Center;
                        roadEnds.Add(localMercPos.ToVector3());
                    }
                    CreateMesh(roadEnds, settings, _meshes[kind]);
                    //yield return CreateRoadSegment(geo, roadEnds);
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
                            var localMercPos = dotMerc - tile.Rect.Center;
                            roadEnds.Add(localMercPos.ToVector3());
                        }
                        CreateMesh(roadEnds, settings, _meshes[kind]);
                    }
                }

                if (_meshes[kind].Vertices.Count > 64000)
                {
                    CreateGameObject(kind, _meshes[kind], main.transform);
                    _meshes[kind] = new MeshData();
                }
            }

            foreach (var group in _meshes)
            {
                CreateGameObject(group.Key, group.Value, main.transform);
            }

            return main;
        }

        private void CreateGameObject(RoadType kind, MeshData meshdata, Transform parent)
        {
            var go = new GameObject(kind + " Road");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            mesh.SetVertices(meshdata.Vertices);
            mesh.SetTriangles(meshdata.Indices, 0);
            mesh.SetUVs(0, meshdata.UV);
            mesh.RecalculateNormals();
            go.GetComponent<MeshRenderer>().material = FactorySettings.GetSettingsFor<RoadSettings>(kind).Material;
            go.transform.position += Vector3.up * Order;
            go.transform.SetParent(parent, true);
        }

        private void CreateMesh(List<Vector3> list, RoadSettings settings, MeshData md)
        {
            var vertsStartCount = md.Vertices.Count;
            Vector3 lastPos = Vector3.zero;
            var norm = Vector3.zero;
            var lastUV = 0f;
            for (int i = 1; i < list.Count; i++)
            {
                var p1 = list[i - 1];
                var p2 = list[i];
                var p3 = p2;
                if (i + 1 < list.Count)
                    p3 = list[i + 1];

                if (lastPos == Vector3.zero)
                {
                    lastPos = Vector3.Lerp(p1, p2, 0f);
                    norm = GetNormal(p1, lastPos, p2) * settings.Width;
                    md.Vertices.Add(lastPos + norm);
                    md.Vertices.Add(lastPos - norm);
                    md.UV.Add(new Vector2(0, 0));
                    md.UV.Add(new Vector2(1, 0));
                }
                var dist = Vector3.Distance(lastPos, p2);
                lastUV += dist;
                lastPos = p2;
                //lastPos = Vector3.Lerp(p1, p2, 1f);
                norm = GetNormal(p1, lastPos, p3) * settings.Width;
                md.Vertices.Add(lastPos + norm);
                md.Vertices.Add(lastPos - norm);
                md.UV.Add(new Vector2(0, lastUV));
                md.UV.Add(new Vector2(1, lastUV));
            }


            for (int j = vertsStartCount; j <= md.Vertices.Count - 3; j += 2)
            {
                var clock = Vector3.Cross(md.Vertices[j + 1] - md.Vertices[j], md.Vertices[j + 2] - md.Vertices[j + 1]);
                if (clock.y < 0)
                {
                    md.Indices.Add(j);
                    md.Indices.Add(j + 2);
                    md.Indices.Add(j + 1);

                    md.Indices.Add(j + 1);
                    md.Indices.Add(j + 2);
                    md.Indices.Add(j + 3);
                }
                else
                {
                    md.Indices.Add(j + 1);
                    md.Indices.Add(j + 2);
                    md.Indices.Add(j);

                    md.Indices.Add(j + 3);
                    md.Indices.Add(j + 2);
                    md.Indices.Add(j + 1);
                }
            }
        }

        private float RoadWidth(RoadType kind)
        {
            return ((float)(int)kind + 1);
        }

        private Vector3 GetNormal(Vector3 p1, Vector3 newPos, Vector3 p2)
        {
            if (newPos == p1 || newPos == p2)
            {
                var n = (p2 - p1).normalized;
                return new Vector3(-n.z, 0, n.x);
            }

            var b = (p2 - newPos).normalized + newPos;
            var a = (p1 - newPos).normalized + newPos;
            var t = (b - a).normalized;

            if (t == Vector3.zero)
            {
                var n = (p2 - p1).normalized;
                return new Vector3(-n.z, 0, n.x);
            }

            return new Vector3(-t.z, 0, t.x);
        }

    }
}
