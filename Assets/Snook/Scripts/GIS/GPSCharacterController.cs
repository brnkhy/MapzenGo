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

        [Tooltip("Drag GpsService container component here.")]
        public GPSService GPS;

        [Tooltip("Speed of object movement.")]
        public float Speed = 2;

        [Tooltip("Speed of object rotation.")]
        public float RotationSpeed = 10;

        #endregion Public properties

        #region private properties

        private GeoLocationCoordinate startLocation;
        private GeoLocationCoordinate lastLocation;

        private Rigidbody _hardBody;
        private Animator _animator;

        //flag to check if the user has tapped / clicked.
        //Set to true on click. Reset to false on reaching destination
        private bool _weMovin = false;

        //destination point
        private Vector3 _destination;

        private Vector3 _lastpos;
        private Vector3 _direction;

        #endregion private properties

        private void Start()
        {
            if (GPS == null)
                Debug.LogError("GPSCHaracterController require a GPS Service attached");

            //We can't really start unitl we've got a signal so.
            GPS.Connected += OnConnected;
            GPS.Changed += onGPSChanged;
        }

        private void OnConnected(object sender, GPSEventArgs e)
        {
            lastLocation = startLocation = GPS.Location; // new GeoLocationCoordinate(GPS.Location.latitude, GPS.Location.longitude);
            if (GetComponent<Rigidbody>())
                _hardBody = GetComponent<Rigidbody>();
            else
                Debug.LogError("The character needs a rigidbody.");

            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            //GetInput();
        }

        public void onGPSChanged(object sender, GPSEventArgs e)
        {
            var newLocation = GPS.Location;

            if (lastLocation != newLocation)
            {
                Vector2d localMerc = newLocation.ToMeters() - lastLocation.ToMeters();
                _destination = localMerc.ToVector3();

                /*
                var tm = GameObject.Find("World").GetComponent<TileManager>();

                //var tileMgr = new GeoLocationCoordinate(tm.Latitude, tm.Longitude);

                Vector2d startMerc = GM.LatLonToMeters(tm.Latitude, tm.Longitude);
                Vector2d destMerc = GM.LatLonToMeters(lat, lon); //33.8301, -84.265
                Vector2d localMerc = destMerc - startMerc;
                _destination = localMerc.ToVector3();
                Debug.Log("_destination: {" + _destination.x + ", " + _destination.y + ", " + _destination.z + "}");
                */
            }
        }

        public void FixedUpdate()
        {
            if (GPS.ActiveAndConnected)
                Run();
        }

        public void Run()
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * Speed);
            float velocity = System.Math.Abs((transform.position.magnitude - _lastpos.magnitude) / Time.deltaTime);

            _lastpos = transform.position;
            //Debug.Log("velocity = " + velocity);

            if (velocity > 0)
            {
                Turn();
                _animator.SetFloat("velocity", velocity);
            }
            else
            {
                _animator.SetFloat("velocity", 0f);
            }
        }

        private void Turn()
        {
            Quaternion _targetRotation;

            //find the vector pointing from our position to the target
            _direction = (_destination - transform.position).normalized;
            //create the rotation we need to be in to look at the target
            _targetRotation = Quaternion.LookRotation(_direction);

            if (_hardBody.transform.rotation != _targetRotation)
                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * RotationSpeed);
        }
    }
}