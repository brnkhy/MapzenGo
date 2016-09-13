using MapzenGo.Helpers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class GpsService : MonoBehaviour
{
    public float Lattitude { get { return lastLocation.latitude; } }
    public float Longitude { get { return lastLocation.longitude; } }
    public bool isRunning { get { return Input.location.status == LocationServiceStatus.Running; } }

    private LocationInfo startLocation;
    private LocationInfo lastLocation;

    // Stuff for the maths
    public float sphereRadius = 6371;

    private float distanceBetweenPoints;
    public float factorOfScale = 95565;

    public void OnClick()
    {
        GpsStop();
        GpsStart();
    }

    public void GpsStart()
    {
        if (Input.location.isEnabledByUser)
        {
            if (Input.location.status == LocationServiceStatus.Initializing || Input.location.status == LocationServiceStatus.Running)
            {
                Input.location.Start(100);

                if (Input.location.status == LocationServiceStatus.Running)
                {
                    StopCoroutine("TrackDistance");
                    Debug.Log("ALREADY RUNNING");
                }
                StartCoroutine("TrackDistance");
            }
            else
            {
                Input.location.Start(100);
                StartCoroutine("TrackDistance");
                Debug.Log("NOTHING");
            }
        }
        else
        {
            Debug.Log("I'M DISABLED");
            GpsDisabled();
        }
    }

    public void GpsStop()
    {
        StopCoroutine("TrackDistance");
        Input.location.Stop();
    }

    private float ToRad(float input)
    {
        return input * (Mathf.PI / 180);
    }

    private IEnumerator TrackDistance()
    {
        int loopcount = 0;
        int i = 1;
        while (i == 1)
        {
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                GpsFailed();
                Debug.Log("FAILED");
            }

            if (Input.location.status == LocationServiceStatus.Initializing)
            {
                Debug.Log("INITIALISING");
            }

            if (Input.location.status == LocationServiceStatus.Stopped)
            {
                Debug.Log("STOPPED?");
            }

            if (Input.location.status == LocationServiceStatus.Running)
            {
                Debug.Log("RUNNING SUCESS");
                startLocation = Input.location.lastData;
                while (Input.location.status == LocationServiceStatus.Running)
                {
                    if (UnityEngine.Input.location.lastData.longitude <= 180.0f
                         && UnityEngine.Input.location.lastData.longitude >= -180.0f
                         && UnityEngine.Input.location.lastData.latitude <= 90.0f
                         && UnityEngine.Input.location.lastData.latitude >= -90.0f)
                    {
                        Debug.Log("INSIDE LOOP");
                        lastLocation = Input.location.lastData;

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
                        Debug.Log("DISTANCE BETWEEN POINTS IS:: " + distanceBetweenPoints + "M");
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

    public virtual void GpsDisabled()
    {
        Debug.Log("GPS IS DISABLED");
    }

    public virtual void GpsFailed()
    {
        Debug.Log("GPS FAILED TO START");
    }

    public LocationInfo GetStartLocation()
    {
        return startLocation;
    }

    public LocationInfo GetLastLocation()
    {
        return lastLocation;
    }

    public float GetDistance()
    {
        return distanceBetweenPoints;
    }
}