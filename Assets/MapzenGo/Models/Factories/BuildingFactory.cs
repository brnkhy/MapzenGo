using System.Collections.Generic;
using System.Linq;
using MapzenGo.Helpers;
using MapzenGo.Models.Enums;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapzenGo.Models.Factories
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
            var kind = geo["properties"].HasField("landuse_kind")
                ? geo["properties"]["landuse_kind"].str.ConvertToEnum<LanduseKind>()
                : LanduseKind.Unknown;
            if (!_active.Contains(key))
            {
                var typeSettings = _settings.GetSettingsFor(kind);
                var buildingCorners = new List<Vector3>();
                //foreach (var bb in geo["geometry"]["coordinates"].list)
                //{
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                for (int i = 0; i < bb.list.Count - 1; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc - tileMercPos;
                    buildingCorners.Add(localMercPos.ToVector3());
                }

                var building = new GameObject("Building").AddComponent<Building>();
                var verts = new List<Vector3>();
                var indices = new List<int>();
                var mesh = building.GetComponent<MeshFilter>().mesh;

                var buildingCenter = ChangeToRelativePositions(buildingCorners);
                building.transform.localPosition = buildingCenter;

                SetProperties(geo, building, typeSettings);

                var height = 0f;
                if (typeSettings.IsVolumetric)
                {
                    height = geo["properties"].HasField("height") ? geo["properties"]["height"].f : Random.Range(typeSettings.MinimumBuildingHeight, typeSettings.MaximumBuildingHeight);
                }

                CreateMesh(buildingCorners, height, typeSettings, ref verts, ref indices);
                
                mesh.vertices = verts.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.RecalculateNormals();

                _active.Add(building.Id);
                building.OnDestroyAsObservable().Subscribe(x => { _active.Remove(building.Id); });

                yield return building;
                //}
            }
        }

        public override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> geoList)
        {
            var items = geoList.Where(x =>
            {
                var key = x["properties"]["id"].ToString();
                var ret = !_active.Contains(key) && Query(x);
                if(ret)
                    _active.Add(key);
                return ret;
            });
            if (!items.Any())
                return null;

            var go = new GameObject("Buildings");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var verts = new List<Vector3>();
            var indices = new List<int>();

            GetVertices(tileMercPos, _settings.Default, items, verts, indices);
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            go.GetComponent<MeshRenderer>().material = _settings.Default.Material;
            go.transform.position += Vector3.up * Order;
            return go;
        }

        private Vector3 ChangeToRelativePositions(List<Vector3> buildingCorners)
        {
            var buildingCenter = buildingCorners.Aggregate((acc, cur) => acc + cur) / buildingCorners.Count;
            for (int i = 0; i < buildingCorners.Count; i++)
            {
                //using corner position relative to building center
                buildingCorners[i] = buildingCorners[i] - buildingCenter;
            }
            return buildingCenter;
        }

        private static void SetProperties(JSONObject geo, Building building, Building.BuildingSettings typeSettings)
        {
            building.name = "building " + geo["properties"]["id"].ToString();
            if (geo["properties"].HasField("name"))
                building.Name = geo["properties"]["name"].str;

            building.Id = geo["properties"]["id"].ToString();
            building.Type = geo["type"].str;
            building.SortKey = (int)geo["properties"]["sort_key"].f;
            building.Kind = typeSettings.Type.ToString();
            building.LanduseKind = typeSettings.Type.ToString();
            building.GetComponent<MeshRenderer>().material = typeSettings.Material;
        }

        private void GetVertices(Vector2d tileMercPos, Building.BuildingSettings typeSettings, IEnumerable<JSONObject> items, List<Vector3> verts, List<int> indices)
        {
            foreach (var geo in items)
            {
                var buildingCorners = new List<Vector3>();
                //foreach (var bb in geo["geometry"]["coordinates"].list)
                //{
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                for (int i = 0; i < bb.list.Count - 1; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = new Vector2((float)(dotMerc.x - tileMercPos.x), (float)(dotMerc.y - tileMercPos.y));
                    buildingCorners.Add(localMercPos.ToVector3xz());
                }

                var height = 0f;
                if (typeSettings.IsVolumetric)
                {
                    height = geo["properties"].HasField("height") ? geo["properties"]["height"].f : Random.Range(typeSettings.MinimumBuildingHeight, typeSettings.MaximumBuildingHeight);
                }

                CreateMesh(buildingCorners, height, typeSettings, ref verts, ref indices);
                //}
            }
        }

        public void CreateMesh(List<Vector3> corners, float height, Building.BuildingSettings typeSettings, ref List<Vector3> verts, ref List<int> indices)
        {

            var tris = new Triangulator(corners);
            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, height, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));

            if (typeSettings.IsVolumetric)
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
