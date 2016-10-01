using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

public class BoundaryFactorySettings: SettingsLayers
{
    public BoundarySettings DefaultBoundary = new BoundarySettings();
    public List<BoundarySettings> SettingsBoundary;

    public BoundaryFactorySettings()
    {
        DefaultBoundary = new BoundarySettings()
        {
            Material = null,
            Type = BoundaryType.Unknown
        };
        SettingsBoundary = new List<BoundarySettings>();
    }
    public override BoundarySettings GetSettingsFor<BoundarySettings>(Enum type)
    {
        return SettingsBoundary.FirstOrDefault(x => x.Type == (BoundaryType)type) as BoundarySettings ?? DefaultBoundary as BoundarySettings;
    }

    public override bool HasSettingsFor(Enum type)
    {
        return SettingsBoundary.Any(x => x.Type == (BoundaryType)type);
    }
}
[Serializable]
public class BoundarySettings : BaseSetting
{
    public Material Material;
    public BoundaryType Type;
    public float Width = 2;
}