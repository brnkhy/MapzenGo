using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Helpers;
using UniRx;
using UnityEngine;

namespace MapzenGo.Models.Factories
{
    public class LanduseFactory : Factory
    {
        public override string XmlTag { get { return "landuse"; } }

        [SerializeField] private Landuse.Settings _settings;

        public override void Start()
        {
            Query = (geo) => geo["geometry"]["type"].str == "Polygon" || geo["geometry"]["type"].str == "MultiPolygon";
        }

        protected override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToEnum<LanduseKind>();
            if (kind != LanduseKind.Unknown && _settings.HasSettingsFor(kind))
            {
                var landuseCorners = new List<Vector3>();
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                if (bb == null || bb.list == null)
                    yield break;

                for (var i = 0; i < bb.list.Count - 1; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc - tileMercPos;
                    landuseCorners.Add(localMercPos.ToVector3());
                }

                if (!landuseCorners.Any())
                    yield break;

                var landuse = new GameObject("Landuse").AddComponent<Landuse>();
                var verts = new List<Vector3>();
                var indices = new List<int>();
                var mesh = landuse.GetComponent<MeshFilter>().mesh;

                //I want object center to be in the middle of object, not at the corner of the tile
                var landuseCenter = ChangeToRelativePositions(landuseCorners);
                landuse.transform.localPosition = landuseCenter;

                SetProperties(geo, landuse, kind);
                CreateMesh(landuseCorners, verts, indices);
                
                mesh.vertices = verts.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.RecalculateNormals();

                yield return landuse;
            }
        }

        protected override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> items)
        {
            var main = new GameObject("Landuse Layer");
            
            var _meshes = new Dictionary<LanduseKind, Tuple<List<Vector3>, List<int>>>();
            foreach (var geo in items.Where(x => Query(x)))
            {
                var kind = geo["properties"]["kind"].str.ConvertToEnum<LanduseKind>();
                if (!_settings.HasSettingsFor(kind))
                    continue;

                var typeSettings = _settings.GetSettingsFor(kind);
                if (!_meshes.ContainsKey(kind))
                    _meshes.Add(kind, new Tuple<List<Vector3>, List<int>>(new List<Vector3>(), new List<int>()));

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

                CreateMesh(buildingCorners, _meshes[kind].Item1, _meshes[kind].Item2);

                if (_meshes[kind].Item1.Count > 64000 || _meshes[kind].Item2.Count > 64000)
                {
                    CreateGameObject(kind, _meshes[kind].Item1, _meshes[kind].Item2, main);
                    _meshes[kind].Item1.Clear();
                    _meshes[kind].Item2.Clear();
                }
                //}
            }

            foreach (var group in _meshes)
            {
                CreateGameObject(group.Key, group.Value.Item1, group.Value.Item2, main);
            }

            return main;
        }
        
        private static Vector3 ChangeToRelativePositions(List<Vector3> landuseCorners)
        {
            var landuseCenter = landuseCorners.Aggregate((acc, cur) => acc + cur) / landuseCorners.Count;
            for (int i = 0; i < landuseCorners.Count; i++)
            {
                //using corner position relative to landuse center
                landuseCorners[i] = landuseCorners[i] - landuseCenter;
            }
            return landuseCenter;
        }

        private void SetProperties(JSONObject geo, Landuse landuse, LanduseKind kind)
        {
            landuse.name = "landuse " + geo["properties"]["id"].ToString();
            if (geo["properties"].HasField("name"))
                landuse.Name = geo["properties"]["name"].str;
            landuse.Id = geo["properties"]["id"].ToString();
            landuse.Type = geo["type"].str;
            landuse.SortKey = (int)geo["properties"]["sort_key"].f;
            landuse.Kind = kind;
            landuse.GetComponent<MeshRenderer>().material = _settings.GetSettingsFor(kind).Material;
        }
        
        private void CreateMesh(List<Vector3> corners, List<Vector3> verts, List<int> indices)
        {
            var tris = new Triangulator(corners);
            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, 0, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));
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
            go.transform.SetParent(main.transform, true);
        }
    }
}
