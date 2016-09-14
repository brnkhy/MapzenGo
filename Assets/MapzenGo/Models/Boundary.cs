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

        [Serializable]
        public class Settings
        {
            public BoundarySettings Default;
            public List<BoundarySettings> AllSettings;

            public BoundarySettings GetSettingsFor(BoundaryType type)
            {
                var f = AllSettings.FirstOrDefault(x => x.Type == type);
                return f ?? Default;
            }

            public bool HasSettingsFor(BoundaryType type)
            {
                return AllSettings.Any(x => x.Type == type);
            }
        }

        [Serializable]
        public class BoundarySettings
        {
            public BoundaryType Type;
            public Material Material;
            public float Width = 6;
        }
    }
}
