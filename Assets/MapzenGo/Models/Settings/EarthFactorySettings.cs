using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

namespace MapzenGo.Models.Settings
{
    public class EarthFactorySettings: SettingsLayers
    {
        public EarthSettings DefaultEarth = new EarthSettings();
        public List<EarthSettings> SettingsEarth = new List<EarthSettings>();

        public EarthFactorySettings()
        {
            DefaultEarth = new EarthSettings()
            {
                Material = null,
                Type = EarthType.Earth
            };
            SettingsEarth = new List<EarthSettings>();
        }

        public override EarthSettings GetSettingsFor<EarthSettings>(Enum type)
        {
            return SettingsEarth.FirstOrDefault(x => x.Type == (EarthType)type)as EarthSettings ?? DefaultEarth as EarthSettings;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsEarth.Any(x => x.Type == (EarthType)type);
        }
    }
    [Serializable]
    public class EarthSettings : BaseSetting
    {
        public EarthType Type;
        public Material Material;

        public EarthSettings() 
        {
            Type = EarthType.Earth;
            Material = null;
        }
   
    }
}