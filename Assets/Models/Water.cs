using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Water : MonoBehaviour
    {
        public string Id { get; set; }
        public string Type;
        public string Kind;
        public string Name;

        public void Init(List<Vector3> buildingCorners, Settings settings)
        {
            GetComponent<MeshFilter>().mesh = CreateMesh(buildingCorners, settings);
            GetComponent<MeshRenderer>().material = Resources.Load<Material>("Water");
        }
        
        private static Mesh CreateMesh(List<Vector3> verts, Settings settings)
        {
            var tris = new Triangulator(verts.Select(x => x.ToVector2xz()).ToArray());
            var mesh = new Mesh();

            var vertices = verts.Select(x => new Vector3(x.x, 0, x.z)).ToList();
            var indices = tris.Triangulate().ToList();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }


        [Serializable]
        public class Settings
        {

        }
    }
}
