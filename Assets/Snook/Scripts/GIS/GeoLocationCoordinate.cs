using System;
using UnityEngine;

namespace Snook.GIS
{
    /// <summary>
    /// Meant to resemble LocationInfo which is meant to resemble GeoCoordinate
    /// </summary>
    public class GeoLocationCoordinate //: LocationInfo
    {
        private const int EarthRadius = 6378137;
        private const double OriginShift = 2 * Math.PI * EarthRadius / 2;

        private float _latitude;
        private float _longitude;

        public float latitude
        {
            get { return this._latitude; }
            private set
            {
                if (Mathf.Abs(value) <= 180)
                    this._latitude = value;
            }
        }

        public float longitude
        {
            get { return this._longitude; }
            private set
            {
                if (Mathf.Abs(value) <= 180)
                    this._longitude = value;
            }
        }

        public GeoLocationCoordinate(string Coord)
        {
            float[] latlon = Array.ConvertAll(Coord.Split(','), s => float.Parse(s));

            this.latitude = latlon[0];
            this.longitude = latlon[1];
        }

        public GeoLocationCoordinate(float fLatitude, float fLongitude)
        {
            this.latitude = fLatitude;
            this.longitude = fLatitude;
        }

        public GeoLocationCoordinate(LocationInfo locationInfo)
        {
            this.latitude = locationInfo.latitude;
            this.longitude = locationInfo.latitude;
        }

        /// <summary>
        /// Override of Equals to compare lat and lon
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var item = obj as GeoLocationCoordinate;

            return (this.latitude == item.latitude && this.longitude == item.longitude);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static explicit operator GeoLocationCoordinate(LocationInfo b)  // explicit byte to digit conversion operator
        {
            return new GeoLocationCoordinate(b.latitude, b.longitude);  // explicit conversion
        }

        #region GeoConverter methods

        public Vector2d ToMeters()
        {
            var p = new Vector2d();
            p.x = (longitude * OriginShift / 180);
            p.y = (Math.Log(Math.Tan((90 + latitude) * Math.PI / 360)) / (Math.PI / 180));
            p.y = (p.y * OriginShift / 180);
            return new Vector2d(p.x, p.y);
        }

        #endregion GeoConverter methods
    }

    public static class GeoLocationCoordinateSettings
    {
        public static int Zoom;
    }
}