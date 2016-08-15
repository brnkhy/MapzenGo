using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets
{
    public enum RoadType
    {
        Path,
        Rail,
        MinorRoad,
        MajorRoad,
        Highway,
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    internal class Road : MonoBehaviour
    {
        public string Id;
        public string Kind;
        public string Type;
        public string Name;

        public void Initialize(List<Vector3> list, string kindstring, Settings settings)
        {
            var verts = new List<Vector3>();
            var indices = new List<int>();
            var kind = kindstring.ToRoadType();
            //Just assign a tmp value to this
            Vector3 lastPos = Vector3.zero;
            var norm = Vector3.zero;
            for (int i = 1; i < list.Count; i++)
            {
                var p1 = list[i - 1];
                var p2 = list[i];
                var p3 = p2;
                if (i + 1 < list.Count)
                    p3 = list[i + 1];

                if (lastPos == Vector3.zero)
                {
                    lastPos = Vector3.Lerp(p1, p2, 0f);
                    norm = GetNormal(p1, lastPos, p2) * RoadWidth(kind);
                    verts.Add(lastPos + norm);
                    verts.Add(lastPos - norm);
                }

                lastPos = Vector3.Lerp(p1, p2, 1f);
                norm = GetNormal(p1, lastPos, p3) * RoadWidth(kind);
                verts.Add(lastPos + norm);
                verts.Add(lastPos - norm);
            }


            for (int j = 0; j <= verts.Count - 3; j += 2)
            {
                var clock = Vector3.Cross(verts[j + 1] - verts[j], verts[j + 2] - verts[j + 1]);
                if (clock.y < 0)
                {
                    indices.Add(j);
                    indices.Add(j + 2);
                    indices.Add(j + 1);

                    indices.Add(j + 1);
                    indices.Add(j + 2);
                    indices.Add(j + 3);
                }
                else
                {
                    indices.Add(j + 1);
                    indices.Add(j + 2);
                    indices.Add(j);

                    indices.Add(j + 3);
                    indices.Add(j + 2);
                    indices.Add(j + 1);
                }
            }

            var mesh = GetComponent<MeshFilter>().mesh;
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private float RoadWidth(RoadType kind)
        {
            return ((float)(int)kind + 1);
        }

        private Vector3 GetNormal(Vector3 p1, Vector3 newPos, Vector3 p2)
        {
            if (newPos == p1 || newPos == p2)
            {
                var n = (p2 - p1).normalized;
                return new Vector3(-n.z, 0, n.x);
            }

            var b = (p2 - newPos).normalized + newPos;
            var a = (p1 - newPos).normalized + newPos;
            var t = (b - a).normalized;

            if (t == Vector3.zero)
            {
                var n = (p2 - p1).normalized;
                return new Vector3(-n.z, 0, n.x);
            }

            return new Vector3(-t.z, 0, t.x);
        }

        int ClampListPos(int pos, int count)
        {
            if (pos < 0)
            {
                pos = 0;
            }

            if (pos >= count)
            {
                pos = count - 1;
            }

            return pos;
        }

        Vector3 GetCatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var a = 0.5f * (2f * p1);
            var b = 0.5f * (p2 - p0);
            var c = 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3);
            var d = 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3);
            var pos = a + (b * t) + (c * t * t) + (d * t * t * t);
            return pos;
        }

        [Serializable]
        public class Settings
        {

        }
    }
}
