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
            if (geo["geometry"]["type"].str == "LineString")
            {
                var road = new GameObject("road").AddComponent<Road>();
                var mesh = road.GetComponent<MeshFilter>().mesh;
                var roadEnds = new List<Vector3>();
                var verts = new List<Vector3>();
                var indices = new List<int>();
                var kind = geo["properties"]["kind"].str.ToRoadType();
                for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var c = geo["geometry"]["coordinates"][i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                    roadEnds.Add(localMercPos.ToVector3xz());
                }
                CreateMesh(roadEnds, kind, ref verts, ref indices);
                mesh.vertices = verts.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                road.Initialize(geo, roadEnds, _settings);
                yield return road;
            }
            else if (geo["geometry"]["type"].str == "MultiLineString")
            {
                for (var i = 0; i < geo["geometry"]["coordinates"].list.Count; i++)
                {
                    var road = new GameObject("road").AddComponent<Road>();
                    var mesh = road.GetComponent<MeshFilter>().mesh;
                    var roadEnds = new List<Vector3>();
                    var verts = new List<Vector3>();
                    var indices = new List<int>();
                    var kind = geo["properties"]["kind"].str.ToRoadType();

                    roadEnds.Clear();
                    var c = geo["geometry"]["coordinates"][i];
                    for (var j = 0; j < c.list.Count; j++)
                    {
                        var seg = c[j];
                        var dotMerc = GM.LatLonToMeters(seg[1].f, seg[0].f);
                        var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                        roadEnds.Add(localMercPos.ToVector3xz());
                    }
                    CreateMesh(roadEnds, kind, ref verts, ref indices);
                    mesh.vertices = verts.ToArray();
                    mesh.triangles = indices.ToArray();
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();
                    road.Initialize(geo, roadEnds, _settings);
                    yield return road;
                }
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
                var kind = geo["properties"]["kind"].str.ToRoadType();
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
                    CreateMesh(roadEnds, kind, ref verts, ref indices);
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
                            var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                            roadEnds.Add(localMercPos.ToVector3xz());
                        }
                        CreateMesh(roadEnds, kind, ref verts, ref indices);
                        //yield return CreateRoadSegment(geo, roadEnds);
                    }
                }
            }
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return go;
        }

        private void CreateMesh(List<Vector3> list, RoadType kind, ref List<Vector3> verts, ref List<int> indices)
        {
            var vertsStartCount = verts.Count;
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
                    norm = GetNormal(p1, lastPos, p2) * RoadWidth(kind);
                    verts.Add(lastPos + norm);
                    verts.Add(lastPos - norm);
                }

                lastPos = Vector3.Lerp(p1, p2, 1f);
                norm = GetNormal(p1, lastPos, p3) * RoadWidth(kind);
                verts.Add(lastPos + norm);
                verts.Add(lastPos - norm);
            }


            for (int j = vertsStartCount; j <= verts.Count - 3; j += 2)
            {
                var clock = Vector3.Cross(verts[j + 1] - verts[j], verts[j + 2] - verts[j + 1]);
                if (clock.y < 0)
                {
                    indices.Add(j);
                    indices.Add(j + 2);
                    indices.Add(j + 1);
                    
                    indices.Add(j + 1);
                    indices.Add(j + 2);
                    indices.Add(j + 3);
                }               
                else            
                {               
                    indices.Add(j + 1);
                    indices.Add(j + 2);
                    indices.Add(j);
                               
                    indices.Add(j + 3);
                    indices.Add(j + 2);
                    indices.Add(j + 1);
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
