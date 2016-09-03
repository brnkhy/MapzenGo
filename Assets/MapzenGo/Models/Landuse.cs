using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using Assets.MapzenGo.Models.Enums;
using UnityEngine;

namespace Assets.Models
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Landuse : MonoBehaviour
    {
        public string Id;
        public string Type;
        public LanduseKind Kind;
        public string Name;
        public int SortKey;
        
        [Serializable]
        public class Settings
        {
            public List<LanduseSettings> AllSettings;

            public LanduseSettings GetSettingsFor(LanduseKind type)
            {
                return AllSettings.FirstOrDefault(x => x.Type == type);
            }
        }

        [Serializable]
        public class LanduseSettings
        {
            public LanduseKind Type;
            public Material Material;
        }

    }

}
