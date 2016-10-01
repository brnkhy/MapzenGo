using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

namespace MapzenGo.Models.Settings
{
    public class BuildingFactorySettings: SettingsLayers
    {
        public BuildingSettings DefaultBuilding = new BuildingSettings();
        public List<BuildingSettings> SettingsBuildings;


        public override BuildingSettingsField GetSettingsFor<BuildingSettingsField>(Enum type)
        {
            if ((BuildingType)type == BuildingType.Unknown)
                return DefaultBuilding as BuildingSettingsField;
            return SettingsBuildings.FirstOrDefault(x => x.Type == (BuildingType)type) as BuildingSettingsField ?? DefaultBuilding as BuildingSettingsField;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsBuildings.Any(x => x.Type== (BuildingType)type);
        }
    }
    [System.Serializable]
    public class BuildingSettings:BaseSetting
    {
        public BuildingType Type;
        public Material Material;
        public int MinimumBuildingHeight = 2;
        public int MaximumBuildingHeight = 5;
        public bool IsVolumetric = true;
    }
}