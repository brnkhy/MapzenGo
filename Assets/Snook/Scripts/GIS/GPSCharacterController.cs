using MapzenGo.Helpers;
using MapzenGo.Models;
using Snook.GIS;
using System;
using UnityEngine;

namespace Snook
{
    /// <summary>
    /// This script should be attached to gameobject that contains the rigidbody and the animator
    /// </summary>
    public class GPSCharacterController : MonoBehaviour
    {
        #region Public properties

        [Tooltip("Drag GpsService, (eg 'World') container component here.")]
        public IGPSService GPS;

        [Tooltip("Speed of object movement.")]
        public float Speed = 2;

        [Tooltip("Speed of object rotation.")]
        public float RotationSpeed = 10;

        #endregion Public properties

        #region private properties

        private GeoLocationCoordinate startLocation;
        private GeoLocationCoordinate lastLocation;

        private Rigidbody hardBody;
        private Animator animator;
        //private Quaternion _targetRotation;

        //Set to true on click. Reset to false on reaching destination
        private bool weMovin = false;

        //destination point
        private Vector3 destination;

        public Vector3 eulerAngleVelocity;

        #endregion private properties

        private void Awake()
        {
            if (GPS == null)
                try
                {
#if UNITY_EDITOR
                    this.GPS = GetComponent<GPSServiceTest>();
#else
                    this.GPS = GetComponent<GPSService>();
#endif
                }
                catch
                {
                    Debug.LogError("GPSCHaracterController require a GPS Service attached");
                }
            else
            {
                //We can't really start unitl we've got a signal so.
                GPS.Connected += onConnected;
                GPS.Changed += onGPSChanged;
            }
        }

        private void onConnected(GPSEventArgs e)
        {
            lastLocation = startLocation = GPS.Location; // new GeoLocationCoordinate(GPS.Location.latitude, GPS.Location.longitude);
            if (GetComponent<Rigidbody>())
                this.hardBody = GetComponent<Rigidbody>();
            else
                Debug.LogError("The character needs a rigidbody.");

            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            //GetInput();
        }

        public void onGPSChanged(GPSEventArgs e)
        {
            var meters = GM.LatLonToMeters(e.Coordinate.latitude, e.Coordinate.longitude);
            // get the tile manager for the correct zoom
            var tm = GameObject.Find("World").GetComponent<GPS_CDTM>();
            var tileinfo = GM.MetersToTile(meters, tm.Zoom);

            //does the tile we want to go to exist?
            var gTile = GameObject.Find("tile " + tileinfo.x + '-' + tileinfo.y);
            if (gTile)
            {
                //get this tiles Tile object move relative to it?
                Tile tile = gTile.GetComponent<Tile>();
                if (tile.Rect.Contains(meters))
                {
                    transform.position = (meters - tile.Rect.Center).ToVector3();
                }
            }
            else // coordinate outside of current area.  reset the map?
            {
                tm.onConnected(e);
            }
        }

        //public void onGPSChanged(GPSEventArgs e)
        //{
        //    var newLocation = GPS.Location;

        //    if (this.lastLocation != newLocation)
        //    {
        //        Vector2d localMerc = newLocation.ToMeters() - lastLocation.ToMeters();

        //        this.destination = localMerc.ToVector3();
        //        this.lastLocation = newLocation;
        //        Debug.Log(destination.x + ", " + destination.z);
        //    }
        //}

        public void FixedUpdate()
        {
            if (GPS != null && GPS.ActiveAndConnected)
                Run();
        }

        private Vector3 lastpos;

        private void LateUpdate()
        {
            //Vector3 newpos = transform.position;
            //newpos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            //transform.position = newpos;
        }

        private void move()
        {
            Quaternion deltaRotation = Quaternion.Euler(this.eulerAngleVelocity * Time.deltaTime);

            this.hardBody.MoveRotation(this.hardBody.rotation * deltaRotation);
            this.hardBody.MovePosition(this.destination + this.transform.forward * Time.deltaTime);
        }

        public void Run()
        {
            float velocity;

            if (this.transform.position != this.destination)
            {
                weMovin = true;
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * this.Speed);

                velocity = System.Math.Abs((transform.position.magnitude - lastpos.magnitude) / Time.deltaTime);

                lastpos = transform.position;

                if (velocity > 0)
                {
                    Turn();
                    //move
                    animator.SetFloat("velocity", velocity);
                }
                else
                {
                    animator.SetFloat("velocity", 0f);
                }
            }
            else
                weMovin = false;
        }

        private void Turn()
        {
            //find the vector pointing from our position to the target
            Vector3 _direction = (destination - transform.position).normalized;
            //create the rotation we need to be in to look at the target
            Quaternion _targetRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * this.RotationSpeed);
        }
    }
}