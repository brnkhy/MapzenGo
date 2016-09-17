using Snook.Helpers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Snook.GIS
{
    public class GPSService : SingletonMonoBehaviour<GPSService>
    {
        //public float Lattitude { get { return lastLocation.latitude; } }
        //public float Longitude { get { return lastLocation.longitude; } }
        //public bool isRunning { get { return Input.location.status == LocationServiceStatus.Running; } }

        /// <summary>
        /// Returns a location
        /// </summary>
        public GeoLocationCoordinate Location
        {
            get
            {
                return new GeoLocationCoordinate(lastLocation.latitude, lastLocation.longitude);
            }
        }

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

        public void Start()
        {
            GpsStart();
        }

        private void GpsStart()
        {
            if (Input.location.isEnabledByUser)
            {
                if (Input.location.status == LocationServiceStatus.Initializing || Input.location.status == LocationServiceStatus.Running)
                {
                    Input.location.Start(100);

                    if (Input.location.status == LocationServiceStatus.Running)
                    {
                        var loc = new GeoLocationCoordinate(Input.location.lastData.latitude, Input.location.lastData.longitude);
                        this.startLocation = this.lastLocation = loc;

                        Connected.Invoke(this, new GPSEventArgs(loc));
                        Debug.Log("ALREADY RUNNING");
                    }
                    StartCoroutine("TrackDistance");
                }
                else
                {
                    Input.location.Start(100);
                    Debug.Log("NOTHING");
                }
            }
            else
            {
                Debug.Log("GPS is diabled by user.");
                GpsDisabled();
            }
        }

        private void OnDestroy()
        {
            Input.location.Stop();
        }

        private float ToRad(float input)
        {
            return input * (Mathf.PI / 180);
        }

        private IEnumerator noTrackDistance()
        {
            while (true)
            {
                if (Input.location.status == LocationServiceStatus.Failed)
                {
                    GpsFailed();
                }

                if (Input.location.status == LocationServiceStatus.Initializing)
                {
                    Debug.Log("Initializing");
                }

                if (Input.location.status == LocationServiceStatus.Stopped)
                {
                    Debug.Log("Stopped?");
                }

                if (Input.location.status == LocationServiceStatus.Running)
                {
                    Debug.Log("Running");
                    startLocation = (GeoLocationCoordinate)Input.location.lastData;

                    while (Input.location.status == LocationServiceStatus.Running)
                    {
                        if (UnityEngine.Input.location.lastData.longitude <= 180.0f
                             && UnityEngine.Input.location.lastData.longitude >= -180.0f
                             && UnityEngine.Input.location.lastData.latitude <= 90.0f
                             && UnityEngine.Input.location.lastData.latitude >= -90.0f)
                        {
                            //Debug.Log("INSIDE LOOP");
                            lastLocation = (GeoLocationCoordinate)Input.location.lastData;

                            string gpsString = "::" + lastLocation.latitude + "//" + lastLocation.longitude;
                            Debug.Log(gpsString);
                            yield return new WaitForSeconds(0.5f);

                            float dLat = ToRad(lastLocation.latitude - startLocation.latitude);
                            float dLon = ToRad(lastLocation.longitude - startLocation.longitude);

                            float a = Mathf.Pow(Mathf.Sin(dLat / 2), 2) +
                                Mathf.Cos(ToRad(startLocation.latitude)) * Mathf.Cos(ToRad(lastLocation.latitude)) *
                                    Mathf.Pow(Mathf.Sin(dLon / 2), 2);

                            float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

                            distanceBetweenPoints = Mathf.RoundToInt((sphereRadius * c) * 1000);
                            Debug.Log("distanceBetweenPoints: " + distanceBetweenPoints + "M");
                            GameObject.Find("outputLabel").gameObject.GetComponent<Text>().text = distanceBetweenPoints.ToString() + "M, ACCURACY IS:: " + Input.location.lastData.horizontalAccuracy;

                            Vector3 CoordStartingPosition = new Vector3(
                                GetStartLocation().latitude,
                                GetStartLocation().longitude,
                                0
                                );

                            Vector3 ScreenStartingPosition = Vector3.zero;

                            Vector3 CoordNewPosition = new Vector3(
                                GetLastLocation().latitude,
                                GetLastLocation().longitude,
                                0
                                );

                            Vector3 ScreenNewPosition = new Vector3(
                                ScreenStartingPosition.x + ((CoordStartingPosition.x - CoordNewPosition.x) * factorOfScale),
                                ScreenStartingPosition.y + ((CoordStartingPosition.y - CoordNewPosition.y) * factorOfScale),
                                0
                                );
                            Debug.Log("Starting GPS Pos: " + CoordStartingPosition.x + " / " + CoordStartingPosition.y);
                            Debug.Log("New GPS Pos: " + CoordNewPosition.x + " / " + CoordNewPosition.y);
                            Debug.Log("New Screen Affectant: " + ((CoordStartingPosition.x - CoordNewPosition.x) * factorOfScale) + " / " + ((CoordStartingPosition.y - CoordNewPosition.y) * factorOfScale));
                        }
                        else
                        {
                            Debug.Log("WELCOME TO MARS");
                        }
                    }
                }
                Debug.Log("Checking");
                yield return new WaitForSeconds(2);
            }
        }

        private void GpsDisabled()
        {
            //Debug.Log("GPS Disabled");
        }

        private void GpsFailed()
        {
            Debug.Log("GPS Failed to start");
        }

        public GeoLocationCoordinate GetStartLocation()
        {
            return startLocation;
        }

        public GeoLocationCoordinate GetLastLocation()
        {
            return lastLocation;
        }

        public float GetDistance()
        {
            return distanceBetweenPoints;
        }
    }

    public delegate void GPSEventDelegate(object sender, GPSEventArgs _args);

    public class GPSEventArgs : EventArgs
    {
        private GeoLocationCoordinate _location;

        public GPSEventArgs(GeoLocationCoordinate oCoordinate)
        {
            this._location = oCoordinate;
        }

        public GeoLocationCoordinate Coordinate { get { return this._location; } }
    } // eo class GPSEventArgs

    //Example
    public class MySource
    {
        public void SomeFunction(GeoLocationCoordinate _data)
        {
            // raise event
            if (OnMyEvent != null) // might not have handlers!
                OnMyEvent(this, new GPSEventArgs(_data));
        } // eo SomeFunction

        public event GPSEventDelegate OnMyEvent;
    } // eo class mySource

    //public abstract class GPSService : SingletonMonoBehaviour<GPSService>
    //{
    //    public abstract event GPSEventDelegate Connected;

    //    public abstract event GPSEventDelegate Changed;

    //    public abstract GeoLocationCoordinate Location { get; }
    //    public abstract bool Enabled { get; }
    //}

    ///// <summary>
    /////  fake little GPS service
    ///// </summary>
    //public class FakeGPSService : GPSService
    //{
    //    public override event GPSEventDelegate Connected;

    //    public override event GPSEventDelegate Changed;

    //    private InputField inplatlon;
    //    private float Latitude { get { return Array.ConvertAll(inplatlon.text.Split(','), s => float.Parse(s))[0]; } }
    //    private float Longitude { get { return Array.ConvertAll(inplatlon.text.Split(','), s => float.Parse(s))[1]; } }

    //    /// <summary>
    //    /// The last known location
    //    /// </summary>
    //    public override GeoLocationCoordinate Location
    //    {
    //        get
    //        {
    //            return new GeoLocationCoordinate(this.Latitude, this.Longitude);
    //        }
    //    }

    //    public override bool Enabled
    //    {
    //        get
    //        {
    //            return true;
    //        }
    //    }

    //    public void Start()
    //    {
    //        inplatlon = GameObject.Find("inpLatLon").gameObject.GetComponent<InputField>();
    //        if (inplatlon == null)
    //            Debug.LogError("FakeGPS has no latlon input field!");
    //        inplatlon.text = "33.8251876,-84.264752";

    //        if (Connected != null)
    //            Connected.Invoke(this, new GPSEventArgs(this.Location));
    //    }
    //}
}