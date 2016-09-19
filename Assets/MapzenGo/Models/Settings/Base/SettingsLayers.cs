using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.MapzenGo.Models.Enums;
using MapzenGo.Models.Enums;
using SerializableCollections;

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
        return Resources.Load<T>("Settings/" + (typeof (T).ToString()));
    }

}

public class SettingsLayersDictionary : SerializableDictionary<Enum, BaseSetting> { };



