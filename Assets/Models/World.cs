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
    private BuildingFactory _buildingFactory;
    private RoadFactory _roadFactory;

    void Start ()
    {
        _buildingFactory = GetComponentInChildren<BuildingFactory>();
        _roadFactory = GetComponentInChildren<RoadFactory>();
	    
        _tileManager = GetComponent<TileManager>();
        _tileManager.Init(_buildingFactory, _roadFactory, _settings);
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
    }
}
