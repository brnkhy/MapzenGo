using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MapzenGo.Helpers;
using MapzenGo.Models.Enums;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace MapzenGo.Models.Factories
{
    public class TileBuilding
    {
        public TileBuilding(LanduseKind kind)
        {
            Kind = kind;
            Vertices = new List<Vector3>();
            Indices = new List<int>();
        }

        public LanduseKind Kind { get; set; }
        public List<Vector3> Vertices { get; set; }
        public List<int> Indices { get; set; }
    }

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

        protected override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
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

                CreateMesh(buildingCorners, height, typeSettings, verts, indices);

                mesh.vertices = verts.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.RecalculateNormals();

                _active.Add(building.Id);
                building.OnDisableAsObservable().Subscribe(x =>
                {
                    _active.Remove(building.Id);
                });

                yield return building;
                //}
            }
        }

        protected override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> items)
        {
            
            var main = new GameObject("Buildings Layer");

            var finalList = new List<TileBuilding>();
            var openList = new Dictionary<LanduseKind, Tuple<List<Vector3>, List<int>>>();
            
            foreach (var geo in items.Where(x => Query(x)))
            {
                var key = geo["properties"]["id"].ToString();
                //if (_active.Contains(key))
                //    continue;

                //to prevent duplicate buildings
                //_active.Add(key);

                var kind = geo["properties"].HasField("landuse_kind")
                ? geo["properties"]["landuse_kind"].str.ConvertToEnum<LanduseKind>()
                : LanduseKind.Unknown;

                var typeSettings = _settings.GetSettingsFor(kind);

                //if we dont have a setting defined for that, it'Ll be merged to "unknown" 
                if (!_settings.HasSettingsFor(kind))
                    kind = LanduseKind.Unknown;

                if (!openList.ContainsKey(kind))
                    openList.Add(kind, new Tuple<List<Vector3>, List<int>>(new List<Vector3>(), new List<int>()));

                var buildingCorners = new List<Vector3>();
                //foreach (var bb in geo["geometry"]["coordinates"].list)z
                //{
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                for (int i = 0; i < bb.list.Count - 1; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = new Vector2((float)(dotMerc.x - tileMercPos.x), (float)(dotMerc.y - tileMercPos.y));
                    buildingCorners.Add(localMercPos.ToVector3xz());
                }

                //returns random height if real value not available
                var height = GetHeights(geo, typeSettings.MinimumBuildingHeight, typeSettings.MaximumBuildingHeight);

                //create mesh, actually just to get vertice&indices
                //filling last two parameters, horrible call yea
                CreateMesh(buildingCorners, height, typeSettings, openList[kind].Item1, openList[kind].Item2);

                //unity cant handle more than 65k on single mesh
                //so we'll finish current and start a new one
                if (openList[kind].Item1.Count > 64000 || openList[kind].Item2.Count > 64000)
                {
                    var tb = new TileBuilding(kind);
                    tb.Vertices.AddRange(openList[kind].Item1);
                    tb.Indices.AddRange(openList[kind].Item2);
                    finalList.Add(tb);
                    openList[kind].Item1.Clear();
                    openList[kind].Item2.Clear();
                }
                //}
            }

            foreach (var tuple in openList)
            {
                var tb = new TileBuilding(tuple.Key);
                tb.Vertices.AddRange(tuple.Value.Item1);
                tb.Indices.AddRange(tuple.Value.Item2);
                finalList.Add(tb);
            }

            foreach (var group in finalList)
            {
                CreateGameObject(group.Kind, group.Vertices, group.Indices, main);
            }
            return main;
        }

        private float GetHeights(JSONObject geo, float min, float max)
        {
            var height = 0f;
            if (_settings.Default.IsVolumetric)
            {
                height = geo["properties"].HasField("height")
                    ? geo["properties"]["height"].f
                    : UnityEngine.Random.Range(min, max);
            }
            return height;
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
        private void CreateMesh(List<Vector3> corners, float height, Building.BuildingSettings typeSettings, List<Vector3> verts, List<int> indices)
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
        private void CreateGameObject(LanduseKind kind, List<Vector3> vertices, List<int> indices, GameObject main)
        {
            var go = new GameObject(kind + " Buildings");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshRenderer>().material = _settings.GetSettingsFor(kind).Material;
            go.transform.position += Vector3.up * Order;
            go.transform.SetParent(main.transform, false);
        }
    }
}
