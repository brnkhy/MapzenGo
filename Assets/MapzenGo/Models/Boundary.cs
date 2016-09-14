using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Boundary : MonoBehaviour
    {
        public string Id;
        public string Kind;
        public string Type;
        public string Name;
        public int SortKey;
    }
}
