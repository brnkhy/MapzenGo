using Snook.Helpers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Snook.GIS
{
    /// <summary>
    ///  fake little GPS service
    /// </summary>
    public class GPSServiceTest : SingletonMonoBehaviour<GPSServiceTest>, IGPSService
    {
        // events
        public event GPSEventDelegate Connected;

        public event GPSEventDelegate Changed;

        /// <summary>
        /// The last known location
        /// </summary>
        public GeoLocationCoordinate Location { get { return this.lastLocation; } }

        public bool ActiveAndConnected { get { return true; } }

        private InputField inplatlon;
        private GeoLocationCoordinate startLocation;
        private GeoLocationCoordinate lastLocation;

        public void Awake()
        {
            string sCoords = "33.830132, -84.264458";
            inplatlon = GameObject.Find("inpLatLon").gameObject.GetComponent<InputField>();
            if (inplatlon == null)
                Debug.LogError("FakeGPS has no latlon input field!");
            inplatlon.text = sCoords;

            startLocation = lastLocation = new GeoLocationCoordinate(sCoords);
        }

        public void ValueChangeCheck()
        {
            var newLocation = new GeoLocationCoordinate(inplatlon.text);

            if (newLocation != lastLocation)
            {
                // let anybody listening know whats up
                if (Changed != null)
                    Changed.Invoke(new GPSEventArgs(newLocation));
                lastLocation = newLocation;
            }
        }

        public void Start()
        {
            Connected.Invoke(new GPSEventArgs(this.startLocation));
            Changed.Invoke(new GPSEventArgs(this.startLocation));
        }

        public GeoLocationCoordinate GetStartLocation()
        {
            return startLocation;
        }
    }
}