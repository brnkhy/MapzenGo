using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Models.Factories
{
    public class BuildingFactory : Factory
    {
        public override string XmlTag { get { return "buildings"; } }
        private HashSet<string> _active = new HashSet<string>();

        [SerializeField]
        private Building.Settings _settings;

        public override void Start()
        {
            Query = (geo) => geo["geometry"]["type"].str == "Polygon";
        }

        public override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
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
                    var localMercPos = dotMerc - tileMercPos;
                    buildingCorners.Add(localMercPos.ToVector3());
                }

                try
                {
                    building = new GameObject("buildings").AddComponent<Building>();
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
                    var height = 0f;
                    if (_settings.IsVolumetric)
                    {
                        height = geo["properties"].HasField("height") ? geo["properties"]["height"].f : Random.Range(_settings.MinimumBuildingHeight, _settings.MaximumBuildingHeight);
                    }

                    CreateMesh(buildingCorners, height, _settings, ref verts, ref indices);

                    mesh.vertices = verts.ToArray();
                    mesh.triangles = indices.ToArray();
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();

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

        private static void SetProperties(JSONObject geo, Building building)
        {
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
        }

        public override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> geoList)
        {
            var items = geoList.Where(x => x["geometry"]["type"].str == "Polygon");
            if (!items.Any())
                return null;

            var go = new GameObject("Buildings");
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
            go.transform.position += Vector3.up * Order;
            return go;
        }

        private void GetVertices(Vector2d tileMercPos, IEnumerable<JSONObject> items, List<Vector3> verts, List<int> indices)
        {
            foreach (var geo in items)
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
                        var localMercPos = new Vector2((float) (dotMerc.x - tileMercPos.x), (float) (dotMerc.y - tileMercPos.y));
                        buildingCorners.Add(localMercPos.ToVector3xz());
                    }

                    try
                    {
                        //var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
                        //for (int i = 0; i < buildingCorners.Count; i++)
                        //{
                        //    //using corner position relative to building center
                        //    buildingCorners[i] = buildingCorners[i] - buildingCenter;
                        //}
                        var height = 0f;
                        if (_settings.IsVolumetric)
                        {
                            height = geo["properties"].HasField("height") ? geo["properties"]["height"].f : Random.Range(_settings.MinimumBuildingHeight, _settings.MaximumBuildingHeight);
                        }

                        CreateMesh(buildingCorners, height, _settings, ref verts, ref indices);
                        _active.Add(key);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }

                    //}
                }
            }
        }

        public void CreateMesh(List<Vector3> corners, float height, Building.Settings settings, ref List<Vector3> verts, ref List<int> indices)
        {
            var tris = new Triangulator(corners);

            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, height, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));

            if (settings.IsVolumetric)
            {

                Vector3 v1;
                Vector3 v2;
                int ind = 0;
                for (int i = 1; i < corners.Count; i++)
                {
                    v1 = verts[vertsStartCount + i - 1];
                    v2 = verts[vertsStartCount + i];
                    ind = verts.Count;
                    verts.Add(v1);
                    verts.Add(v2);
                    verts.Add(new Vector3(v1.x, 0, v1.z));
                    verts.Add(new Vector3(v2.x, 0, v2.z));

                    indices.Add(ind);
                    indices.Add(ind + 2);
                    indices.Add(ind + 1);

                    indices.Add(ind + 1);
                    indices.Add(ind + 2);
                    indices.Add(ind + 3);
                }

                v1 = verts[vertsStartCount];
                v2 = verts[vertsStartCount + corners.Count - 1];
                ind = verts.Count;
                verts.Add(v1);
                verts.Add(v2);
                verts.Add(new Vector3(v1.x, 0, v1.z));
                verts.Add(new Vector3(v2.x, 0, v2.z));

                indices.Add(ind);
                indices.Add(ind + 1);
                indices.Add(ind + 2);

                indices.Add(ind + 1);
                indices.Add(ind + 3);
                indices.Add(ind + 2);
            }
        }
    }
}
