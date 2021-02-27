using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net.Tests
{
    public class DistanceComputationTest
    {

        [Test]
        public void DistanceConversionSampleTest()
        {
            // Distance between Nashville International Airport (BNA) and Los Angeles International Airport (LAX)
            // See https://rosettacode.org/wiki/Haversine_formula#C.23
            var coord1 = new Coordinate(36.12, -86.67);
            var coord2 = new Coordinate(33.94, -118.4);
            var distCalc = new GeoDistanceCalculator();
            var expectedDistanceM = 2887259.95;
            var computedDistance = distCalc.HaversineDistance(coord1, coord2);

            Assert.AreEqual(Math.Floor(expectedDistanceM), Math.Floor(computedDistance));
        }
    }
}
