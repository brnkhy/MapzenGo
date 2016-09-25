using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapzenGo.Helpers;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Models
{
    public class Place : MonoBehaviour
    {
        public string Id;
        public string Type;
        public string Kind;
        public string Name;
        public int SortKey;
    }
}
