using Snook.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Snook.GIS
{
    public class GPSService : SingletonMonoBehaviour<GPSService>, IGPSService
    {
        /// <summary>
        /// Returns a location
        /// </summary>
        public GeoLocationCoordinate Location { get { return this.lastLocation; } }

        public bool ActiveAndConnected
        {
            get
            {
                return Input.location.status == LocationServiceStatus.Running;
            }
        }

        public event GPSEventDelegate Connected;

        public event GPSEventDelegate Changed;

        private GeoLocationCoordinate startLocation;
        private GeoLocationCoordinate lastLocation;

        // Stuff for the maths
        public float sphereRadius = 6371;

        private float distanceBetweenPoints;
        public float factorOfScale = 95565;

        // allow us to paste in a comma sep lat/lon from goog maps etc
        private string _message;

        // Maintain a timestamp log of the locations
        private Dictionary<DateTime, GeoLocationCoordinate> locations;

        public Dictionary<DateTime, GeoLocationCoordinate> LocationHistory { get { return locations; } }

        private IEnumerator Wait(int Seconds)
        {
            yield return new WaitForSeconds(Seconds);
        }

        private void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();

            guiStyle.fontSize = 100; //change the font size

            GUI.Label(new Rect(10, 10, 400, 50), "Message: " + _message, guiStyle);
            GUI.Label(new Rect(10, 130, 200, 50), "Longitude: " + Input.location.lastData.longitude, guiStyle);
            GUI.Label(new Rect(10, 250, 200, 50), "Latitude: " + Input.location.lastData.latitude, guiStyle);
        }

        public void Start()
        {
            this.locations = new Dictionary<DateTime, GeoLocationCoordinate>();

            GpsStart();
        }

        /// <summary>
        /// Gets repeated every couple of seconds
        /// </summary>
        private void CheckLocation()
        {
            if (this.ActiveAndConnected)
            {
                var newLocation = new GeoLocationCoordinate(Input.location.lastData);
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

        private void GpsStart()
        {
            if (!Input.location.isEnabledByUser)
            {
                _message = "Not Enabled Dammit.";
                return;
            }
            Input.location.Start(1, 1);

            int maxWait = 20;
            _message = "Starting Location Services...";

            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                StartCoroutine("Wait");
                maxWait--;

                if (Input.location.status == LocationServiceStatus.Failed)
                {
                    _message = "Status: failed to locate you";
                }
                else
                {
                    _message = "GPS Service is go";
                    var loc = new GeoLocationCoordinate(Input.location.lastData.latitude, Input.location.lastData.longitude);
                    this.startLocation = this.lastLocation = loc;
                    Connected.Invoke(new GPSEventArgs(loc));
                    InvokeRepeating("CheckLocation", 2, 2);
                }
            }
        }

        private void OnDestroy()
        {
            Input.location.Stop();
        }

        public GeoLocationCoordinate GetStartLocation()
        {
            return startLocation;
        }
    }

    public delegate void GPSEventDelegate(GPSEventArgs _args);

    public class GPSEventArgs : EventArgs
    {
        private GeoLocationCoordinate _location;

        public GPSEventArgs(GeoLocationCoordinate oCoordinate)
        {
            this._location = oCoordinate;
        }

        public GeoLocationCoordinate Coordinate { get { return this._location; } }
    } // eo class GPSEventArgs
}