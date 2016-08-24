using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Models.Factories
{
    public class LanduseFactory : Factory
    {
        public override string XmlTag { get { return "landuse"; } }

        [SerializeField]
        private Building.Settings _settings;

        public override IEnumerable<MonoBehaviour> Create(Vector2 tileMercPos, JSONObject geo)
        {
            if (geo["properties"]["kind"].str == "park")
            {
                var buildingCorners = new List<Vector3>();
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
                    var verts = new List<Vector3>();
                    var indices = new List<int>();
                    var mesh = building.GetComponent<MeshFilter>().mesh;

                    var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
                    for (int i = 0; i < buildingCorners.Count; i++)
                    {
                        //using corner position relative to building center
                        buildingCorners[i] = buildingCorners[i] - buildingCenter;
                    }

                    building.transform.localPosition = buildingCenter;
                    SetProperties(geo, building);
                    CreateMesh(buildingCorners, _settings, ref verts, ref indices);

                    mesh.vertices = verts.ToArray();
                    mesh.triangles = indices.ToArray();
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }

                yield return building;
            }
        }

        private static void SetProperties(JSONObject geo, Building building)
        {
            building.name = "landuse " + geo["properties"]["id"].ToString();
            var kind = "";
            if (geo["properties"].HasField("kind"))
                kind = geo["properties"]["kind"].str;
            if (geo["properties"].HasField("name"))
                building.Name = geo["properties"]["name"].str;

            building.Id = geo["properties"]["id"].ToString();
            building.Type = geo["type"].str;
            building.SortKey = (int)geo["properties"]["sort_key"].f;
            building.Kind = kind;
            building.LanduseKind = kind;
        }

        public override GameObject CreateLayer(Vector2 tileMercPos, List<JSONObject> geoList)
        {
            var items = geoList.Where(x => x["geometry"]["type"].str == "Polygon" && x["properties"]["kind"].str == "park");
            if (!items.Any())
                return null;

            var go = new GameObject();
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var verts = new List<Vector3>();
            var indices = new List<int>();

            GetVertices(tileMercPos, items, verts, indices);
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            go.GetComponent<MeshRenderer>().material = BaseMaterial;
            go.transform.position += Vector3.up*Order;
            return go;
        }

        private void GetVertices(Vector2 tileMercPos, IEnumerable<JSONObject> items, List<Vector3> verts, List<int> indices)
        {
            foreach (var geo in items)
            {
                var buildingCorners = new List<Vector3>();
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                for (int i = 0; i < bb.list.Count - 1; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
                    buildingCorners.Add(localMercPos.ToVector3xz());
                }
                CreateMesh(buildingCorners, _settings, ref verts, ref indices);
            }
        }

        public void CreateMesh(List<Vector3> corners, Building.Settings settings, ref List<Vector3> verts, ref List<int> indices)
        {
            var tris = new Triangulator(corners);
            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, 0, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));
        }
    }
}
