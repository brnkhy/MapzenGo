using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Building : MonoBehaviour
    {
        public string Id;
        public string Type;
        public string Kind;
        public string Name;

        public string LanduseKind
        {
            get { return _landuseKind; }
            set
            {
                _landuseKind = value;
                GetComponent<MeshRenderer>().material = Resources.Load<Material>(LanduseKind);
            }
        }

        public int SortKey;
        private string _landuseKind;

        public void Init(List<Vector3> buildingCorners, Settings settings)
        {
            GetComponent<MeshFilter>().mesh = CreateMesh(buildingCorners, settings);
        }

        public static Mesh CreateMesh(List<Vector3> verts, Settings settings)
        {
            var height = UnityEngine.Random.Range(settings.MinimumBuildingHeight,
                        settings.MaximumBuildingHeight);
            var tris = new Triangulator(verts.Select(x => x.ToVector2xz()).ToArray());
            var mesh = new Mesh();

            var vertices = verts.Select(x => new Vector3(x.x, height, x.z)).ToList();
            var indices = tris.Triangulate().ToList();

            var n = verts.Count;

            Vector3 v1;
            Vector3 v2;
            for (int i = 1; i < verts.Count; i++)
            {
                v1 = vertices[i - 1];
                v2 = vertices[i];
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(new Vector3(v1.x, 0, v1.z));
                vertices.Add(new Vector3(v2.x, 0, v2.z));

                indices.Add(n);
                indices.Add(n + 2);
                indices.Add(n + 1);

                indices.Add(n + 1);
                indices.Add(n + 2);
                indices.Add(n + 3);

                n += 4;
            }

            v1 = vertices[0];
            v2 = vertices[verts.Count - 1];
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(new Vector3(v1.x, 0, v1.z));
            vertices.Add(new Vector3(v2.x, 0, v2.z));

            indices.Add(n);
            indices.Add(n + 1);
            indices.Add(n + 2);

            indices.Add(n + 1);
            indices.Add(n + 3);
            indices.Add(n + 2);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }


        [Serializable]
        public class Settings
        {
            public int MinimumBuildingHeight = 2;
            public int MaximumBuildingHeight = 5;
        }
    }

}
