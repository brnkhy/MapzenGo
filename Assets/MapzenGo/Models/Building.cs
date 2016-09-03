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
                if (type == MapzenGo.Models.Enums.LanduseKind.Unknown)
                    return Default;
                return AllSettings.FirstOrDefault(x => x.Type == type) ?? Default;
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
