using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapzenGo.Helpers;
using MapzenGo.Models.Enums;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Factories
{
    public class RoadFactory : Factory
    {
        public override string XmlTag { get { return "roads"; } }

        public override void Start()
        {
            base.Start();
            Query = (geo) => geo["geometry"]["type"].str == "LineString" || geo["geometry"]["type"].str == "MultiLineString";
        }

        protected override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToEnum<RoadType>();
            if (_settings.HasSettingsFor(kind))
            {
                var typeSettings = _settings.GetSettingsFor(kind);

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
                        var localMercPos = dotMerc - tileMercPos;
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
                    road.SortKey = (int)geo["properties"]["sort_key"].f;
                    if (geo["properties"].HasField("name"))
                        road.Name = geo["properties"]["name"].str;

                    road.transform.position += Vector3.up*road.SortKey/100;
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
                            var localMercPos = dotMerc - tileMercPos;
                            roadEnds.Add(localMercPos.ToVector3());
                        }

                        SetProperties(geo, road);

                        CreateMesh(roadEnds, typeSettings, md);
                        mesh.vertices = md.Vertices.ToArray();
                        mesh.triangles = md.Indices.ToArray();
                        mesh.SetUVs(0, md.UV);
                        mesh.RecalculateNormals();
                        
                        road.GetComponent<MeshRenderer>().material = typeSettings.Material;

                        

                        road.transform.position += Vector3.up*road.SortKey/100;
                        yield return road;
                    }
                }
            }
        }

        private static void SetProperties(JSONObject geo, Road road)
        {
            road.Id = geo["properties"]["id"].ToString();
            road.Type = geo["type"].str;
            road.Kind = geo["properties"]["kind"].str;
            road.SortKey = (int) geo["properties"]["sort_key"].f;
            if (geo["properties"].HasField("name"))
                road.Name = geo["properties"]["name"].str;
        }

        protected override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> geoList)
        {
            var go = new GameObject("Roads");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var md = new MeshData();

            GetVertices(tileMercPos, geoList, md);
            mesh.vertices = md.Vertices.ToArray();
            mesh.triangles = md.Indices.ToArray();
            mesh.SetUVs(0, md.UV);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            go.GetComponent<MeshRenderer>().material = _settings.DefaultRoad.Material;
            go.transform.position += Vector3.up * Order;
            return go;
        }

        private void GetVertices(Vector2d tileMercPos, List<JSONObject> geoList, MeshData md)
        {
            foreach (var geo in geoList.Where(x => Query(x)))
            {
                var kind = geo["properties"]["kind"].str.ConvertToEnum<RoadType>();
                if (!_settings.HasSettingsFor(kind))
                    continue;

                var settings = _settings.GetSettingsFor(kind);
                var roadEnds = new List<Vector3>();

                if (geo["geometry"]["type"].str == "LineString")
                {
                    for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                    {
                        var c = geo["geometry"]["coordinates"][i];
                        var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                        var localMercPos = dotMerc - tileMercPos;
                        roadEnds.Add(localMercPos.ToVector3());
                    }
                    CreateMesh(roadEnds, settings, md);
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
                            var localMercPos = dotMerc - tileMercPos;
                            roadEnds.Add(localMercPos.ToVector3());
                        }
                        CreateMesh(roadEnds, settings, md);
                    }
                }
            }
        }

        private void CreateMesh(List<Vector3> list, SettingsLayers.RoadSettings settings, MeshData md)
        {
            var vertsStartCount = md.Vertices.Count;
            Vector3 lastPos = Vector3.zero;
            var norm = Vector3.zero;
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
                }

                lastPos = Vector3.Lerp(p1, p2, 1f);
                norm = GetNormal(p1, lastPos, p3) * settings.Width;
                md.Vertices.Add(lastPos + norm);
                md.Vertices.Add(lastPos - norm);
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
