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

        private string _landuseKind;
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
        
        [Serializable]
        public class Settings
        {
            public BuildingSettings Default = new BuildingSettings();
            public List<BuildingSettings> AllSettings;

            public BuildingSettings GetSettingsFor(LanduseKind type)
            {
                if (type == Enums.LanduseKind.Unknown)
                    return Default;
                return AllSettings.FirstOrDefault(x => x.Type == type) ?? Default;
            }

            public bool HasSettingsFor(LanduseKind type)
            {
                return AllSettings.Any(x => x.Type == type);
            }
        }

        [Serializable]
        public class BuildingSettings
        {
            public LanduseKind Type;
            public Material Material;
            public int MinimumBuildingHeight = 2;
            public int MaximumBuildingHeight = 5;
            public bool IsVolumetric = true;
        }
    }

}
