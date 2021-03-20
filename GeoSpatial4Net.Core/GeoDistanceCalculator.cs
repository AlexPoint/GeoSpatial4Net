using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net
{
    public class GeoDistanceCalculator
    {
        // The mean earth radius (the earth begin a flattened sphere, the equatorial and polar radii are not equal) gives a better accuracy for the distance computation methods below
        private const int earthRadiusInMeters = 6371009;

        public const double earthRadiusAtTheEquatorInMeters = 6_378_137D;
        public const double earthRadiusAtThePolesInMeters = 6_356_752.314245D; // radius at the poles, meters

        public DistanceUnit Unit { get; private set; }

        private double EarthRadius
        {
            get
            {
                return new DistanceConverter().ConvertDistance(earthRadiusInMeters, DistanceUnit.Meter, Unit);
            }
        }
        private double EarthRadiusAtTheEquator
        {
            get
            {
                return new DistanceConverter().ConvertDistance(earthRadiusAtTheEquatorInMeters, DistanceUnit.Meter, Unit);
            }
        }

        private double EarthRadiusAtThePoles
        {
            get
            {
                return new DistanceConverter().ConvertDistance(earthRadiusAtThePolesInMeters, DistanceUnit.Meter, Unit);
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

        /// <summary>
        /// Computes the distance between two sets of latitude/longitude, using the Vincenti formula.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double VincentiDistance(Coordinate position1, Coordinate position2, double precision = 1e-12, int maxIterations = 100)
        {
            var λ1 = ToRadians(position1.Longitude);
            var λ2 = ToRadians(position2.Longitude);

            var φ1 = ToRadians(position1.Latitude);
            var φ2 = ToRadians(position2.Latitude);

            var a = EarthRadiusAtTheEquator; // radius at equator
            var b = EarthRadiusAtThePoles; // Using b to keep close to academic formula.
            var f = 1 / 298.257223563D; // flattening of the ellipsoid

            var L = λ2 - λ1;
            var tanU1 = (1 - f) * Math.Tan(φ1);
            var cosU1 = 1 / Math.Sqrt((1 + tanU1 * tanU1));
            var sinU1 = tanU1 * cosU1;

            var tanU2 = (1 - f) * Math.Tan(φ2);
            var cosU2 = 1 / Math.Sqrt((1 + tanU2 * tanU2));
            var sinU2 = tanU2 * cosU2;

            double λ = L, λʹ, cosSqα, σ, cos2σM, cosσ, sinσ, sinλ, cosλ;
            do
            {
                sinλ = Math.Sin(λ);
                cosλ = Math.Cos(λ);
                var sinSqσ = (cosU2 * sinλ) * (cosU2 * sinλ) + (cosU1 * sinU2 - sinU1 * cosU2 * cosλ) * (cosU1 * sinU2 - sinU1 * cosU2 * cosλ);
                sinσ = Math.Sqrt(sinSqσ);
                //if (sinσ == 0) { return Vincenty.CO_INCIDENT_POINTS; } // co-incident points
                if (sinσ == 0) { return 0; } // co-incident points
                cosσ = sinU1 * sinU2 + cosU1 * cosU2 * cosλ;
                σ = Math.Atan2(sinσ, cosσ);
                var sinα = cosU1 * cosU2 * sinλ / sinσ;
                cosSqα = 1 - sinα * sinα;
                cos2σM = cosσ - 2 * sinU1 * sinU2 / cosSqα;

                if (double.IsNaN(cos2σM)) cos2σM = 0;  // equatorial line: cosSqα=0
                var C = f / 16 * cosSqα * (4 + f * (4 - 3 * cosSqα));
                λʹ = λ;
                λ = L + (1 - C) * f * sinα * (σ + C * sinσ * (cos2σM + C * cosσ * (-1 + 2 * cos2σM * cos2σM)));
            } while (Math.Abs(λ - λʹ) > precision && --maxIterations > 0);

            if (maxIterations == 0) { throw new InvalidOperationException("Vincenti formula failed to converge"); }

            var uSq = cosSqα * (a * a - b * b) / (b * b);
            var A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            var B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            var Δσ = B * sinσ * (cos2σM + B / 4 * (cosσ * (-1 + 2 * cos2σM * cos2σM) -
                    B / 6 * cos2σM * (-3 + 4 * sinσ * sinσ) * (-3 + 4 * cos2σM * cos2σM)));

            var distance = b * A * (σ - Δσ);

            return distance;

            /*var initialBearing = atan2(cosU2 * sinλ, cosU1 * sinU2 - sinU1 * cosU2 * cosλ);
            initialBearing = (initialBearing + 2 * PI) % (2 * PI); //turning value to trigonometric direction

            var finalBearing = atan2(cosU1 * sinλ, -sinU1 * cosU2 + cosU1 * sinU2 * cosλ);
            finalBearing = (finalBearing + 2 * PI) % (2 * PI);  //turning value to trigonometric direction

            return new Vincenty(distance, toDegrees(initialBearing), toDegrees(finalBearing));*/
        }


        /// <summary>
        /// Computes the distance between two sets of latitude/longitude, using the Vincenti formula.
        /// </summary>
        /// <returns>The estimated distance, in the unit specified in the GeoDistanceCalculator constructor</returns>
        public double VincentiDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var coord1 = new Coordinate(latitude1, longitude1);
            var coord2 = new Coordinate(latitude2, longitude2);

            return VincentiDistance(coord1, coord2);
        }

        private static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
