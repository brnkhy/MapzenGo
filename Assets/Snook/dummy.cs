using MapzenGo.Helpers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class dummy : MonoBehaviour
{
    private Vector3 _destination;

    // Use this for initialization
    private void Start()
    {
        GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text = 33.8301f.ToString();
        GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text = (-84.265f).ToString();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Run();
    }

    private void Update()
    {
        GpsService gps = GameObject.Find("Canvas").GetComponent<GpsService>();
        if (gps.isRunning)
        {
            GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text = gps.Lattitude.ToString();
            GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text = gps.Longitude.ToString();
        }

        if (this._destination != null)
        {
            //Rigidbody rgd = GameObject.Find("ThirdPersonController").gameObject.GetComponent<Rigidbody>();
            ////rgd.MovePosition(dest);
            //rgd.position = Vector3.MoveTowards(rgd.position, _destination, Time.deltaTime * 10);
        }
    }

    public void Run()
    {
        Rigidbody rgd = GameObject.Find("ThirdPersonController").gameObject.GetComponent<Rigidbody>();
        //rgd.MovePosition(dest);
        rgd.position = Vector3.MoveTowards(rgd.position, _destination, Time.deltaTime * 10);
    }

    public void Move()
    {
        float lat = float.Parse(GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text);
        float lon = float.Parse(GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text);
        Vector2d destPost = GM.LatLonToMeters(lat, lon); //33.8301, -84.265
        Vector2d tileMercPos = GM.MetersToTile(destPost, 19);
        Vector2d temp;
        try
        {
            Vector3 t = GameObject.Find("Tile " + (int)tileMercPos.x + "-" + (int)tileMercPos.y).gameObject.transform.position;
            temp.x = t.x;
            temp.y = t.z;

            Vector2d pixels = GM.MetersToPixels(destPost - temp, 19);
            _destination = pixels.ToVector3();
            Debug.Log("_destination.x: " + _destination.x);
            Debug.Log("_destination.y: " + _destination.y);
            Debug.Log("_destination.z: " + _destination.z);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
            ///Debug.Break();
        }
        //var dotMerc = GM.LatLonToMeters(seg[1].f, seg[0].f);
        //var localMercPos = dotMerc - tileMercPos;
        //roadEnds.Add(localMercPos.ToVector3());

        //temp = GM.MetersToPixels(temp, 1);

        //_destination = new Vector3((float)temp.normalized.x, 0, (float)temp.normalized.y);

        //GameObject.Find("ThirdPersonController").gameObject.GetComponent<ThirdPersonCharacter>().Move(dest, false, false);
    }
}