using MapzenGo.Helpers;
using MapzenGo.Models;
using System;
using System.IO;
using UnityEngine;

namespace Snook.GIS
{
    /// <summary>
    /// A CachedDynamicTileManager which repsonds to GPS events instaed of begining on start event
    /// </summary>
    public class GPS_CDTM : CachedDynamicTileManager
    {
        private bool _weGotAMap = false;

        //[Tooltip("Drag GpsService container (World) component here.")]
        public IGPSService GPS;

        /// <summary>
        /// Subscribe to gps connect event
        /// </summary>
        public void Awake()
        {
#if UNITY_EDITOR
            this.GPS = GetComponent<GPSServiceTest>();
#else
            this.GPS = GetComponent<GPSService>();
#endif

            GPS.Connected += onConnected;
        }

        public override void Start()
        {
            GeoLocationCoordinateSettings.Zoom = this.Zoom;
        }

        /// Application.persistentDataPath is for android.  We've got a gps signal so lets draw our map
        public void onConnected(GPSEventArgs e)
        {
            Latitude = e.Coordinate.latitude;
            Longitude = e.Coordinate.longitude;
            Destroy(GameObject.Find("Tiles"));

            base.Start();

            CacheFolderPath = Path.Combine(Application.persistentDataPath, RelativeCachePath);
            CacheFolderPath = CacheFolderPath.Format(Zoom);
            if (!Directory.Exists(CacheFolderPath))
                Directory.CreateDirectory(CacheFolderPath);

            _weGotAMap = true;
        }

        protected override void Update()
        {
            if (_weGotAMap)
                base.Update();
        }
    }
}