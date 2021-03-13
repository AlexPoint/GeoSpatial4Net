using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net
{
    public class GeoDistanceCalculator
    {
        // The mean earth radius (the earth begin a flattened sphere, the equatorial and polar radii are not equal) gives a better accuracy for the distance computation methods below
        private const int earthRadiusInMeters = 6371009;

        public DistanceUnit Unit { get; private set; }

        private double EarthRadius
        {
            get
            {
                return new DistanceConverter().ConvertDistance(earthRadiusInMeters, DistanceUnit.Meter, Unit);
            }
        }

        public GeoDistanceCalculator(DistanceUnit unit = DistanceUnit.Meter) {
            Unit = unit;
        }

        /// <summary>
        /// Computes the distance between two sets of latitude/longitude, using the Haversine approximation.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double HaversineDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var coord1 = new Coordinate(latitude1, longitude1);
            var coord2 = new Coordinate(latitude2, longitude2);

            return HaversineDistance(coord1, coord2);
        }

        /// <summary>
        /// Computes the distance between two coordinates, using the Haversine approximation.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double HaversineDistance(Coordinate position1, Coordinate position2)
        {
            var lat = ToRadians(position2.Latitude - position1.Latitude);
            var lng = ToRadians(position2.Longitude - position1.Longitude);
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(ToRadians(position1.Latitude)) * Math.Cos(ToRadians(position2.Latitude)) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return EarthRadius * h2;
        }


        /// <summary>
        /// Computes the distance between two coordinates, using the spherical law of consines approximation.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double SLCDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var coord1 = new Coordinate(latitude1, longitude1);
            var coord2 = new Coordinate(latitude2, longitude2);

            return SLCDistance(coord1, coord2);
        }

        /// <summary>
        /// Computes the distance between two coordinates, using the spherical law of consines approximation.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double SLCDistance(Coordinate position1, Coordinate position2)
        {
            var lng = ToRadians(position2.Longitude - position1.Longitude);
            var lat1 = ToRadians(position1.Latitude);
            var lat2 = ToRadians(position2.Latitude);

            var centralAngle = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) +
                        Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lng));

            // great-circle (orthodromic) distance on Earth between 2 points    
            return EarthRadius * centralAngle; 
        }

        /// <summary>
        /// Computes the distance between two coordinates, using the equirectangular approximation.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double EquirectangularProjectionDistance(Coordinate position1, Coordinate position2)
        {
            var p1 = ToRadians(position2.Longitude - position1.Longitude) * Math.Cos(0.5 * ToRadians(position2.Latitude + position1.Latitude)); //convert lat/lon to radians
            var p2 = ToRadians(position2.Latitude - position1.Latitude);
            return EarthRadius * Math.Sqrt(p1 * p1 + p2 * p2);
        }

        /// <summary>
        /// Computes the distance between two sets of latitude/longitude, using the equirectangular approximation.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double EquirectangularProjectionDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var coord1 = new Coordinate(latitude1, longitude1);
            var coord2 = new Coordinate(latitude2, longitude2);

            return EquirectangularProjectionDistance(coord1, coord2);
        }

        private static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
