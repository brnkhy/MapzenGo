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
        public override string XmlTag { get { return "buildings"; } }
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
            var items = geoList.Where(x => x["geometry"]["type"].str == "Polygon");
            if (!items.Any())
                return null;

            var go = new GameObject();
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var verts = new List<Vector3>();
            var indices = new List<int>();

            var heavyMethod = Observable.Start(() => GetVertices(tileMercPos, items, verts, indices));
            heavyMethod.ObserveOnMainThread().Subscribe(mapData =>
            {
                mesh.vertices = verts.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                go.GetComponent<MeshRenderer>().material = Resources.Load<Material>("residential");
            });

            return go;
        }

        private void GetVertices(Vector2 tileMercPos, IEnumerable<JSONObject> items, List<Vector3> verts, List<int> indices)
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
                        var localMercPos = new Vector2(dotMerc.x - tileMercPos.x, dotMerc.y - tileMercPos.y);
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

                        CreateMesh(buildingCorners, _settings, ref verts, ref indices);
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

        public void CreateMesh(List<Vector3> corners, Building.Settings settings, ref List<Vector3> verts, ref List<int> indices)
        {
            var rnd = new System.Random();
            var height = settings.IsVolumetric ? rnd.Next(settings.MinimumBuildingHeight, settings.MaximumBuildingHeight) : 0;
            var tris = new Triangulator(corners.Select(x => x.ToVector2xz()).ToArray());

            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, height, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));

            if (settings.IsVolumetric)
            {
                vertsStartCount = verts.Count;
                verts.AddRange(corners.Select(x => new Vector3(x.x, 0, x.z)));

                var n = 0;
                for (int i = 1; i < corners.Count; i++)
                {
                    indices.Add(vertsStartCount + n);
                    indices.Add(vertsStartCount + n + 2);
                    indices.Add(vertsStartCount + n + 1);
                                
                    indices.Add(vertsStartCount + n + 1);
                    indices.Add(vertsStartCount + n + 2);
                    indices.Add(vertsStartCount + n + 3);

                    n += 4;
                }

                indices.Add(vertsStartCount + n);
                indices.Add(vertsStartCount + n + 1);
                indices.Add(vertsStartCount + n + 2);
                            
                indices.Add(vertsStartCount + n + 1);
                indices.Add(vertsStartCount + n + 3);
                indices.Add(vertsStartCount + n + 2);
            }
        }
    }
}
