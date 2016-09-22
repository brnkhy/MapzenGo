using Snook.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
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

        // Maintain a timestamp log of the locations
        private Dictionary<DateTime, GeoLocationCoordinate> locations;

        public Dictionary<DateTime, GeoLocationCoordinate> LocationHistory { get { return locations; } }

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

        public void Start()
        {
            this.locations = new Dictionary<DateTime, GeoLocationCoordinate>();

            if (Connected != null)
                Connected.Invoke(new GPSEventArgs(this.startLocation));
            if (Changed != null)
                Changed.Invoke(new GPSEventArgs(this.startLocation));

            this.locations.Add(DateTime.Now, this.startLocation);

            InvokeRepeating("CheckLocation", 2, 2);
        }

        /// <summary>
        /// Gets repeated every couple of seconds
        /// </summary>
        private void CheckLocation()
        {
            if (this.ActiveAndConnected)
            {
                var newLocation = new GeoLocationCoordinate(inplatlon.text);
                if (!newLocation.Equals(this.lastLocation))
                {
                    // let anybody who cares know
                    if (Changed != null)
                        Changed.Invoke(new GPSEventArgs(newLocation));
                    //save it to a log.
                    this.locations.Add(DateTime.Now, newLocation);
                    this.lastLocation = newLocation;
                }
            }
        }

        public GeoLocationCoordinate GetStartLocation()
        {
            return startLocation;
        }
    }
}