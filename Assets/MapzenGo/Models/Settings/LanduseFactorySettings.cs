using System;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using MapzenGo.Models.Settings.Base;
using UnityEngine;

namespace MapzenGo.Models.Settings
{
    public class LanduseFactorySettings: SettingsLayers
    {
        public LanduseSettings DefaultLanduse = new LanduseSettings();
        public List<LanduseSettings> SettingsLanduse;

        public LanduseFactorySettings()
        {
            DefaultLanduse = new LanduseSettings()
            {
                Material = null,
                Type = LanduseKind.Forest,
            };
            SettingsLanduse = new List<LanduseSettings>();
        }

        public override LanduseSettings GetSettingsFor<LanduseSettings>(Enum type)
        {
            var f = SettingsLanduse.FirstOrDefault(x => x.Type == (LanduseKind)type);
            return f as LanduseSettings ?? DefaultLanduse as LanduseSettings;
        }

        public override bool HasSettingsFor(Enum type)
        {
            return SettingsLanduse.Any(x => x.Type == (LanduseKind)type);
        }
    }

    [Serializable]
    public class LanduseSettings : BaseSetting
    {
        public LanduseKind Type;
        public Material Material;
    }
}