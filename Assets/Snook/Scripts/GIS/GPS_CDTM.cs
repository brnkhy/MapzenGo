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
        public GPSService GPS;

        /// <summary>
        /// Subscribe to gps connect event
        /// </summary>
        public override void Start()
        {
            this.GPS = GetComponent<GPSService>();

            GPS.Connected += onConnected;
            GeoLocationCoordinateSettings.Zoom = this.Zoom;
            GeoLocationCoordinateSettings.TileSize = (int)this.TileSize;
        }

        /// Application.persistentDataPath is for android.  We've got a gps signal so lets draw our map
        public void onConnected(object sender, EventArgs e)
        {
            Latitude = GPS.Location.latitude;
            Longitude = GPS.Location.longitude;

            base.Start();

            CacheFolderPath = Path.Combine(Application.persistentDataPath, RelativeCachePath);
            CacheFolderPath = CacheFolderPath.Format(Zoom);
            if (!Directory.Exists(CacheFolderPath))
                Directory.CreateDirectory(CacheFolderPath);
        }

        protected override void Update()
        {
            if (_weGotAMap)
                base.Update();
        }
    }
}