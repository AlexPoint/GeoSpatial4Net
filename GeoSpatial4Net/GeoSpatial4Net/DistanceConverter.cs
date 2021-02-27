using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net
{
    public class DistanceConverter
    {
        private static Dictionary<DistanceUnit, double> ConversionFactorsToMeters = new Dictionary<DistanceUnit, double>()
        {
            { DistanceUnit.Meter, 1},
            { DistanceUnit.Kilometer, 1000 },
            { DistanceUnit.Mile, 0.0006213712 },
            { DistanceUnit.Feet, 3.28084 }
        };

        public DistanceConverter() { }

        /// <summary>
        /// Converts a distance from a given distance unit to another unit.
        /// </summary>
        public double ConvertDistance(double value, DistanceUnit srcUnit, DistanceUnit tgtUnit)
        {
            // We use the meter as a pivot unit
            var srcToMeter = ConversionFactorsToMeters[srcUnit];
            var tgtToMeter = ConversionFactorsToMeters[tgtUnit];

            return value * srcToMeter / tgtToMeter;
        }
    }

    public enum DistanceUnit { Meter, Kilometer, Mile, Feet }
}
