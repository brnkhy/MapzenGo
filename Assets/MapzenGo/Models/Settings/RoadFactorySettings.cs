using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

namespace MapzenGo.Models.Settings
{
    public class RoadFactorySettings: SettingsLayers
    {
        public RoadSettings DefaultRoad;
        public List<RoadSettings> SettingsRoad;

        public RoadFactorySettings()
        {
            DefaultRoad = new RoadSettings()
            {
                Material = null,
                Type = RoadType.Path,
                Width = 3
            };
            SettingsRoad = new List<RoadSettings>();
        }

        public override RoadSettings GetSettingsFor<RoadSettings>(Enum type)
        {
            var f = SettingsRoad.FirstOrDefault(x => x.Type == (RoadType)type);
            return f as RoadSettings?? DefaultRoad as RoadSettings;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsRoad.Any(x => x.Type== (RoadType)type);
        }
    }

    [Serializable]
    public class RoadSettings : BaseSetting
    {
        public Material Material;
        public RoadType Type;
        public RailwayType TypeRail;
        public float Width = 6;
    }
}