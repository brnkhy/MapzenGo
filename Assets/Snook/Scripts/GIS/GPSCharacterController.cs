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

        private GameObject world;

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
            this.world = GameObject.Find("World");

            if (this.GPS == null)
                try
                {
#if UNITY_EDITOR
                    this.GPS = world.GetComponent<GPSServiceTest>();
#else
                    this.GPS = GameObject.Find("World").GetComponent<GPSService>();
#endif
                    GPS.Connected += onConnected;
                    GPS.Changed += onGPSChanged;
                }
                catch
                {
                    Debug.LogError("GPSCHaracterController requires a GPS Service attached to World");
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
            transform.SetParent(world.transform);
            var tm = world.GetComponent<GPS_CDTM>();
            var meters = GM.LatLonToMeters(e.Coordinate.latitude, e.Coordinate.longitude);
            // get the tile manager for the correct zoom
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
                    //this.destination = (meters - tile.Rect.Center).ToVector3();
                    transform.localScale = Vector3.one * 152.8741f;
                    transform.SetParent(tile.transform, false); //or true, not sure about that part
                }
            }
            else // coordinate outside of current area.  reset the map?
            {
                tm.onConnected(e);
            }
        }

        public void FixedUpdate()
        {
            //if (GPS != null && GPS.ActiveAndConnected)
            //    move();
            //Run();
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