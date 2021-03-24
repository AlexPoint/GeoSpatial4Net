using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net.Performance
{
    class DistanceComputationResult
    {
        public double Coordinate1Latitude { get; set; }
        public double Coordinate1Longitude { get; set; }
        public double Coordinate2Latitude { get; set; }
        public double Coordinate2Longitude { get; set; }
        public double HaversineDistance { get; set; }
        public double HaversineEllapsedMilliseconds { get; set; }
        public double SLCDistance { get; set; }
        public double SLCEllapsedMilliseconds { get; set; }
        public double EquirectangularDistance { get; set; }
        public double EquirectangularEllapsedMilliseconds { get; set; }
        public double VincentiDistance { get; set; }
        public double VincentiEllapsedMilliseconds { get; set; }
    }
}
