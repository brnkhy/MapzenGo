using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapzenGo.Models.Enums;
using SerializableCollections;
using UnityEngine;

namespace MapzenGo.Models.Settings.Base
{
    [System.Serializable]
    public abstract class SettingsLayers : ScriptableObject
    {
        public virtual T GetSettingsFor<T>(Enum type) where T : BaseSetting
        {
            return null;
        }

        public virtual bool HasSettingsFor(Enum type)
        {
            return false;
        }

        public static T GetScriptableObject<T>() where T : SettingsLayers
        {
            return Resources.Load<T>("Settings/" + (typeof(T).ToString()));
        }

    }

    public class SettingsLayersDictionary : SerializableDictionary<Enum, BaseSetting> { };
}