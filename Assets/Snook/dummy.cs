using MapzenGo.Helpers;
using MapzenGo.Models;
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
        TileManager tm = GetComponent<TileManager>();

        GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text = tm.Latitude.ToString();  //33.8301f.ToString();
        GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text = tm.Longitude.ToString(); //(-84.265f).ToString();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Run();
        if (this._destination != null)
        {
            Rigidbody rgd = GameObject.Find("ThirdPersonController").gameObject.GetComponent<Rigidbody>();
            var player = GameObject.Find("ThirdPersonController").gameObject.GetComponent<ThirdPersonCharacter>();
            ////rgd.MovePosition(dest);
            //player.Move(_destination, false, false);
            rgd.position = Vector3.MoveTowards(rgd.position, _destination, Time.deltaTime * 100);
        }
    }

    private void Update()
    {
        GpsService gps = GameObject.Find("Canvas").GetComponent<GpsService>();
        if (gps.isRunning)
        {
            GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text = gps.Lattitude.ToString();
            GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text = gps.Longitude.ToString();
        }
    }

    public void Run()
    {
        //Rigidbody rgd = GameObject.Find("ThirdPersonController").gameObject.GetComponent<Rigidbody>();
        //Rigidbody rgd = GameObject.Find("ThirdPersonController").gameObject.GetComponent<Rigidbody>();
        ////rgd.MovePosition(dest);
        //rgd.position = Vector3.MoveTowards(rgd.position, _destination, Time.deltaTime * 10);
    }

    public void Move()
    {
        var tm = GameObject.Find("World").GetComponent<TileManager>();

        int zoom = 18;
        float lat = float.Parse(GameObject.Find("inpLat").gameObject.GetComponent<InputField>().text);
        float lon = float.Parse(GameObject.Find("inpLon").gameObject.GetComponent<InputField>().text);

        Vector2d startMerc = GM.LatLonToMeters(tm.Latitude, tm.Longitude);

        Vector2d destMerc = GM.LatLonToMeters(lat, lon); //33.8301, -84.265
        Vector2d localMerc = destMerc - startMerc;
        _destination = localMerc.ToVector3();
        Debug.Log("_destination.x: " + _destination.x);
        Debug.Log("_destination.y: " + _destination.y);
        Debug.Log("_destination.z: " + _destination.z);

        //Vector2d destpixels = GM.MetersToPixels(startMerc - destMerc, zoom);

        //Vector2d tileMercPos = GM.MetersToTile(destMerc, zoom);
        //Vector2d temp;
        //try
        //{
        //    string fu = "tile " + (int)tileMercPos.x + "-" + (int)tileMercPos.y;
        //    GameObject go = GameObject.Find(fu);
        //    Vector3 t = go.gameObject.transform.position;
        //    temp.x = tileMercPos.x;
        //    temp.y = tileMercPos.y;

        //    Vector2d pixels = GM.MetersToPixels(destMerc - tileMercPos, zoom);
        //    _destination = pixels.ToVector3();
        //    Debug.Log("_destination.x: " + _destination.x);
        //    Debug.Log("_destination.y: " + _destination.y);
        //    Debug.Log("_destination.z: " + _destination.z);
        //}
        //catch (NullReferenceException e)
        //{
        //    Debug.Log(e.Message);
        //    ///Debug.Break();
        //}
        //var dotMerc = GM.LatLonToMeters(seg[1].f, seg[0].f);
        //var localMercPos = dotMerc - tileMercPos;
        //roadEnds.Add(localMercPos.ToVector3());

        //temp = GM.MetersToPixels(temp, 1);

        //_destination = new Vector3((float)temp.normalized.x, 0, (float)temp.normalized.y);

        //GameObject.Find("ThirdPersonController").gameObject.GetComponent<ThirdPersonCharacter>().Move(dest, false, false);
    }
}