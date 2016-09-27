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
    public class LanduseFactory : Factory
    {
        public override string XmlTag { get { return "landuse"; } }
        [SerializeField]
        protected LanduseFactorySettings FactorySettings;
        public override void Start()
        {
            base.Start();
            Query = (geo) => geo["geometry"]["type"].str == "Polygon" || geo["geometry"]["type"].str == "MultiPolygon";
        }

        protected override IEnumerable<MonoBehaviour> Create(Tile tile, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToLanduseType();

            if (!FactorySettings.HasSettingsFor(kind) && !JustDrawEverythingFam)
                yield break;

            var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
            if (bb == null || bb.list == null)
                yield break;

            var count = bb.list.Count - 1;
            if (count < 3)
                yield break;

            var inp = new InputGeometry(count);
            for (var i = 0; i < count; i++)
            {
                var c = bb.list[i];
                var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                var localMercPos = dotMerc - tile.Rect.Center;
                inp.AddPoint(localMercPos.x, localMercPos.y);
                inp.AddSegment(i, (i + 1) % count);
            }

            var landuse = new GameObject("Landuse").AddComponent<Landuse>();
            var md = new MeshData();
            var mesh = landuse.GetComponent<MeshFilter>().mesh;

            SetProperties(geo, landuse, kind);
            CreateMesh(inp, md);

            //I want object center to be in the middle of object, not at the corner of the tile
            var landuseCenter = ChangeToRelativePositions(md.Vertices);
            landuse.transform.localPosition = landuseCenter;

            mesh.vertices = md.Vertices.ToArray();
            mesh.triangles = md.Indices.ToArray();
            mesh.SetUVs(0, md.UV);
            mesh.RecalculateNormals();

            yield return landuse;
        }

        protected override GameObject CreateLayer(Tile tile, List<JSONObject> items)
        {
            var main = new GameObject("Landuse Layer");

            var _meshes = new Dictionary<LanduseKind, MeshData>();
            foreach (var geo in items.Where(x => Query(x)))
            {
                var kind = geo["properties"]["kind"].str.ConvertToLanduseType();
                if (!FactorySettings.HasSettingsFor(kind) && !JustDrawEverythingFam)
                    continue;

                var typeSettings = FactorySettings.GetSettingsFor<LanduseSettings>(kind);
                if (!_meshes.ContainsKey(kind))
                    _meshes.Add(kind, new MeshData());

                //foreach (var bb in geo["geometry"]["coordinates"].list)
                //{
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                var count = bb.list.Count - 1;

                if (count < 3)
                    continue;

                var inp = new InputGeometry(count);

                for (int i = 0; i < count; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc - tile.Rect.Center;
                    inp.AddPoint(localMercPos.x, localMercPos.y);
                    inp.AddSegment(i, (i + 1) % count);
                }

                CreateMesh(inp, _meshes[kind]);

                if (_meshes[kind].Vertices.Count > 64000)
                {
                    CreateGameObject(kind, _meshes[kind], main.transform);
                    _meshes[kind] = new MeshData();
                }
                //}
            }

            foreach (var group in _meshes)
            {
                CreateGameObject(group.Key, group.Value, main.transform);
            }

            return main;
        }

        private Vector3 ChangeToRelativePositions(List<Vector3> landuseCorners)
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
            landuse.GetComponent<MeshRenderer>().material = FactorySettings.GetSettingsFor<LanduseSettings>(kind).Material;
        }

        private void CreateMesh(InputGeometry corners, MeshData meshdata)
        {
            var mesh = new TriangleNet.Mesh();
            mesh.Behavior.Algorithm = TriangulationAlgorithm.SweepLine;
            mesh.Behavior.Quality = true;
            mesh.Triangulate(corners);

            var vertsStartCount = meshdata.Vertices.Count;
            meshdata.Vertices.AddRange(corners.Points.Select(x => new Vector3((float)x.X, 0, (float)x.Y)).ToList());
            meshdata.UV.AddRange(corners.Points.Select(x => new Vector2((float)x.X, (float)x.Y)).ToList());
            foreach (var tri in mesh.Triangles)
            {
                meshdata.Indices.Add(vertsStartCount + tri.P1);
                meshdata.Indices.Add(vertsStartCount + tri.P0);
                meshdata.Indices.Add(vertsStartCount + tri.P2);
            }
        }

        private void CreateGameObject(LanduseKind kind, MeshData meshdata, Transform parent)
        {
            var go = new GameObject(kind + " Landuse");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            mesh.SetVertices(meshdata.Vertices);
            mesh.SetTriangles(meshdata.Indices, 0);
            mesh.SetUVs(0, meshdata.UV);
            mesh.RecalculateNormals();
            go.GetComponent<MeshRenderer>().material = FactorySettings.GetSettingsFor<LanduseSettings>(kind).Material;
            go.transform.position += Vector3.up * Order;
            go.transform.SetParent(parent, true);
        }
    }
}
