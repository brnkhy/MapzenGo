using System;
using System.Collections.Generic;
using Assets;
using Assets.Helpers;
using Assets.Models;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections;
using Assets.Models.Factories;

public class World : MonoBehaviour
{
    [SerializeField] private Settings _settings;
    private TileManager _tileManager;

    private Dictionary<Type, Factory> Factories;

    void Start ()
    {
        Factories = new Dictionary<Type, Factory>();
        var buildingFactory = GetComponentInChildren<BuildingFactory>();
        Factories.Add(typeof(BuildingFactory), buildingFactory);
        var roadFactory = GetComponentInChildren<RoadFactory>();
        Factories.Add(typeof(RoadFactory), roadFactory);
        var waterFactory = GetComponentInChildren<WaterFactory>();
        Factories.Add(typeof(WaterFactory), waterFactory);

        _tileManager = GetComponent<TileManager>();
        _tileManager.Init(Factories, _settings);
	}

    [Serializable]
    public class Settings
    {
        [SerializeField]
        public float Lat = 39.921864f;
        [SerializeField]
        public float Long = 32.818442f;
        [SerializeField]
        public int Range = 3;
        [SerializeField]
        public int DetailLevel = 16;
        [SerializeField]
        public bool LoadImages = false;
        [SerializeField]
        public float TileSize = 100;
        [SerializeField]
        public bool UseLayers;
    }
}
