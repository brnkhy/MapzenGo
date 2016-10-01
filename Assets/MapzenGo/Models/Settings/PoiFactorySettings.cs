using System;
using System.Collections.Generic;
using System.Linq;
using Assets.MapzenGo.Models.Enums;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

namespace MapzenGo.Models.Settings
{
    public class PoiFactorySettings : SettingsLayers
    {
        public PoiSettings DefaultPoi = new PoiSettings();
        public List<PoiSettings> SettingsPoi = new List<PoiSettings>();

        public PoiFactorySettings()
        {
            DefaultPoi = new PoiSettings()
            {
                Type = PoiType.Unknown
            };
            SettingsPoi = new List<PoiSettings>();
        }

        public override PoiSettings GetSettingsFor<PoiSettings>(Enum type)
        {
            return SettingsPoi.FirstOrDefault(x => x.Type == (PoiType)type)as PoiSettings ?? DefaultPoi as PoiSettings;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsPoi.Any(x => x.Type == (PoiType)type);
        }
    }
    [Serializable]
    public class PoiSettings : BaseSetting
    {
        public PoiType Type;
        public Sprite Sprite;

        public PoiSettings() 
        {
            Type = PoiType.Unknown;
        }
   
    }
}