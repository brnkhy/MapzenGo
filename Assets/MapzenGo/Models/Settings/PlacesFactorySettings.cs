using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;

namespace MapzenGo.Models.Settings
{
    public class PlacesFactorySettings : SettingsLayers
    {
        public PlaceSettings DefaultPlace = new PlaceSettings();
        public List<PlaceSettings> SettingsPlace = new List<PlaceSettings>();

        public PlacesFactorySettings()
        {
            DefaultPlace = new PlaceSettings()
            {
                Material = null,
                Type = PlaceType.Unknown
            };
            SettingsPlace = new List<PlaceSettings>();
        }

        public override PlaceSettings GetSettingsFor<PlaceSettings>(Enum type)
        {
            return SettingsPlace.FirstOrDefault(x => x.Type == (PlaceType)type)as PlaceSettings ?? DefaultPlace as PlaceSettings;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsPlace.Any(x => x.Type == (PlaceType)type);
        }
    }
    [Serializable]
    public class PlaceSettings : BaseSetting
    {
        public PlaceType Type;

        public PlaceSettings() 
        {
            Type = PlaceType.Unknown;
            Material = null;
        }
   
    }
}