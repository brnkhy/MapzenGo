using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Helpers;
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

        public override IEnumerable<MonoBehaviour> Create(Vector2d tileMercPos, JSONObject geo)
        {
            var kind = geo["properties"]["kind"].str.ConvertToEnum<LanduseKind>();
            if (kind != LanduseKind.Unknown && _settings.AllSettings.Any(x => x.Type == kind))
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
                CreateMesh(landuseCorners, ref verts, ref indices);
                
                mesh.vertices = verts.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.RecalculateNormals();

                yield return landuse;
            }
        }
        
        public override GameObject CreateLayer(Vector2d tileMercPos, List<JSONObject> geoList)
        {
            var items = geoList.Where(x =>
            {
                var kind = x["properties"]["kind"].str.ConvertToEnum<LanduseKind>();
                return kind != LanduseKind.Unknown && _settings.AllSettings.Any(a => a.Type == kind) && Query(x);
            });
            if (!items.Any())
                return null;

            var go = new GameObject("Parks");
            var mesh = go.AddComponent<MeshFilter>().mesh;
            go.AddComponent<MeshRenderer>();
            var verts = new List<Vector3>();
            var indices = new List<int>();

            GetVertices(tileMercPos, items, verts, indices);
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            go.GetComponent<MeshRenderer>().material = _settings.Default.Material;
            go.transform.position += Vector3.up * Order;
            return go;
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

        private void GetVertices(Vector2d tileMercPos, IEnumerable<JSONObject> items, List<Vector3> verts, List<int> indices)
        {
            foreach (var geo in items)
            {
                var landuseCorners = new List<Vector3>();
                var bb = geo["geometry"]["coordinates"].list[0]; //this is wrong but cant fix it now
                for (int i = 0; i < bb.list.Count - 1; i++)
                {
                    var c = bb.list[i];
                    var dotMerc = GM.LatLonToMeters(c[1].f, c[0].f);
                    var localMercPos = dotMerc - tileMercPos;
                    landuseCorners.Add(localMercPos.ToVector3());
                }
                CreateMesh(landuseCorners, ref verts, ref indices);
            }
        }

        public void CreateMesh(List<Vector3> corners, ref List<Vector3> verts, ref List<int> indices)
        {
            var tris = new Triangulator(corners);
            var vertsStartCount = verts.Count;
            verts.AddRange(corners.Select(x => new Vector3(x.x, 0, x.z)).ToList());
            indices.AddRange(tris.Triangulate().Select(x => vertsStartCount + x));
        }
    }
}
