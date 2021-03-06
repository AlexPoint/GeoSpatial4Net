using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net
{
    public class GeoDistanceCalculator
    {
        private const int earthRadiusInMeters = 6372800;

        public GeoDistanceCalculator() { }


        public double HaversineDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var coord1 = new Coordinate(latitude1, longitude1);
            var coord2 = new Coordinate(latitude2, longitude2);

            return HaversineDistance(coord1, coord2);
        }

        /// <summary>
        /// Computes the distance, in meters, between two coordinates using the Haversine method.
        /// </summary>
        public double HaversineDistance(Coordinate position1, Coordinate position2)
        {
            var lat = ToRadians(position2.Latitude - position1.Latitude);
            var lng = ToRadians(position2.Longitude - position1.Longitude);
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(ToRadians(position1.Latitude)) * Math.Cos(ToRadians(position2.Latitude)) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return earthRadiusInMeters * h2;
        }

        private static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
