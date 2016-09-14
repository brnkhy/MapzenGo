using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.MapzenGo.Models.Enums;
using MapzenGo.Models.Enums;

[System.Serializable]
//[CreateAssetMenu(fileName = "BuildingSetting", menuName = "MapzenGO/Setting/BuildingSetting")]
public class SettingsLayers : ScriptableObject
{
    public BuildingSettings DefaultBuilding = new BuildingSettings();
    public List<BuildingSettings> SettingsBuildings;

    public RoadSettings DefaultRoad = new RoadSettings();
    public List<RoadSettings> SettingsRoad;

    public LanduseSettings DefaultLanduse = new LanduseSettings();
    public List<LanduseSettings> SettingsLanduse;

    public WaterSettings DefaultWater = new WaterSettings();
    public List<WaterSettings> SettingsWater;

    public BoundarySettings DefaultBoundary = new BoundarySettings();
    public List<BoundarySettings> SettingsBoundary;

    #region TYPE CLASS SETTING

    [System.Serializable]
    public class BuildingSettings:Settings
    {
        public BuildingType Type;
        public int MinimumBuildingHeight = 2;
        public int MaximumBuildingHeight = 5;
        public bool IsVolumetric = true;
    }

    [Serializable]
    public class RoadSettings : Settings
    {
        public RoadType Type;
        public RailwayType TypeRail;
        public float Width = 6;
    }
    [Serializable]
    public class LanduseSettings : Settings
    {
        public LanduseKind Type;
    }

    [Serializable]
    public class WaterSettings:Settings
    {
        public WaterType Type;
    }

    [Serializable]
    public class BoundarySettings : Settings
    {
        public BoundaryType Type;
        public float Width = 6;
    }

    public class Settings
    {
        public Material Material;
        [HideInInspector]
        public bool showContent;
    }

    #endregion

    #region BUILDING GET TYPE

    public BuildingSettings GetSettingsFor(BuildingType type)
    {
        if (type == BuildingType.Unknown)
            return DefaultBuilding;
        return SettingsBuildings.FirstOrDefault(x => x.Type == type) ?? DefaultBuilding;
    }

    public bool HasSettingsFor(BuildingType type)
    {
        return SettingsBuildings.Any(x => x.Type == type);
    }
    #endregion

    #region ROAD GET TYPE
    public RoadSettings GetSettingsFor(RoadType type)
    {
        var f = SettingsRoad.FirstOrDefault(x => x.Type == type);
        return f ?? DefaultRoad;
    }

    public bool HasSettingsFor(RoadType type)
    {
        return SettingsRoad.Any(x => x.Type == type);
    }
    #endregion

    #region  LANDUSE GET TYPE
    public LanduseSettings GetSettingsFor(LanduseKind type)
    {
        var f = SettingsLanduse.FirstOrDefault(x => x.Type == type);
        return f ?? DefaultLanduse;
    }

    public bool HasSettingsFor(LanduseKind type)
    {
        return SettingsLanduse.Any(x => x.Type == type);
    }
    #endregion

    #region WATER
    public WaterSettings GetSettingsFor(WaterType type)
    {
        return SettingsWater.FirstOrDefault(x => x.Type == type) ?? DefaultWater;
    }

    public bool HasSettingsFor(WaterType type)
    {
        return SettingsWater.Any(x => x.Type == type);
    }
    #endregion

    #region Boundary
    public BoundarySettings GetSettingsFor(BoundaryType type)
    {
        return SettingsBoundary.FirstOrDefault(x => x.Type == type) ?? DefaultBoundary;
    }

    public bool HasSettingsFor(BoundaryType type)
    {
        return SettingsBoundary.Any(x => x.Type == type);
    }
    #endregion

}


