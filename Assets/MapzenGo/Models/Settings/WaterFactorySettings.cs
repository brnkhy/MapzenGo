using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

namespace MapzenGo.Models.Settings
{
    public class WaterFactorySettings: SettingsLayers
    {
        public WaterSettings DefaultWater = new WaterSettings();
        public List<WaterSettings> SettingsWater = new List<WaterSettings>();

        public WaterFactorySettings()
        {
            DefaultWater = new WaterSettings()
            {
                Material = null,
                Type = WaterType.Water
            };
            SettingsWater = new List<WaterSettings>();
        }

        public override WaterSettings GetSettingsFor<WaterSettings>(Enum type)
        {
            return SettingsWater.FirstOrDefault(x => x.Type == (WaterType)type)as WaterSettings ?? DefaultWater as WaterSettings;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsWater.Any(x => x.Type == (WaterType)type);
        }
    }
    [Serializable]
    public class WaterSettings : BaseSetting
    {
        public WaterType Type;
        public Material Material;

        public WaterSettings() 
        {
            Type = WaterType.Water;
            Material = null;
        }
   
    }
}