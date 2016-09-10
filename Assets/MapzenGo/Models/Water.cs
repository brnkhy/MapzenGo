using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapzenGo.Helpers;
using MapzenGo.Models.Enums;
using UnityEngine;

namespace MapzenGo.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Water : MonoBehaviour
    {
        public string Id;
        public string Type;
        public string Kind;
        public string Name;
        public int SortKey;
        
        [Serializable]
        public class Settings
        {
            public WaterSettings Default = new WaterSettings();
            public List<WaterSettings> AllSettings;

            public WaterSettings GetSettingsFor(WaterType type)
            {
                return AllSettings.FirstOrDefault(x => x.Type == type) ?? Default;
            }

            public bool HasSettingsFor(WaterType type)
            {
                return AllSettings.Any(x => x.Type == type);
            }
        }

        [Serializable]
        public class WaterSettings
        {
            public WaterType Type;
            public Material Material;
        }
    }
}
