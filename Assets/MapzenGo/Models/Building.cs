using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Building : MonoBehaviour
    {
        public string Id;
        public string Type;
        public string Kind;
        public string Name;
        public string LanduseKind;
        public int SortKey;
    }

}
