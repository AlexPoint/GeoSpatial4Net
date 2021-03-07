using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSpatial4Net.Tests
{
    public class DistanceComputationTest
    {

        [Test]
        public void SampleTests()
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

        [Test]
        public void SameCoordinatesTest()
        {
            // Distance between Nashville International Airport (BNA) and Los Angeles International Airport (LAX)
            // See https://rosettacode.org/wiki/Haversine_formula#C.23
            var coord1 = new Coordinate(36.12, -86.67);
            var coord2 = new Coordinate(33.94, -118.4);
            var distCalc = new GeoDistanceCalculator();
            
            Assert.AreEqual(0, Math.Floor(distCalc.HaversineDistance(coord1, coord1)));
            Assert.AreEqual(0, Math.Floor(distCalc.HaversineDistance(coord2, coord2)));
            Assert.AreEqual(0, Math.Floor(distCalc.HaversineDistance(1.25489, 65.698712, 1.25489, 65.698712)));


            Assert.AreEqual(0, Math.Floor(distCalc.EquirectangularProjectionDistance(coord1, coord1)));
            Assert.AreEqual(0, Math.Floor(distCalc.EquirectangularProjectionDistance(coord2, coord2)));
        }

        [Test]
        public void WrongLatLong()
        {
            // longitude 273.33 is greater than 180; it should be -86.67 instead
            var coord1 = new Coordinate(36.12, 273.33);
            var coord2 = new Coordinate(33.94, -118.4);
            var distCalc = new GeoDistanceCalculator();
            var expectedDistanceM = 2887259.95;
            var computedDistance = distCalc.HaversineDistance(coord1, coord2);

            Assert.AreEqual(Math.Floor(expectedDistanceM), Math.Floor(computedDistance));


            // latitude 396.12 is greater than 90; it should be -36.12 instead 
            var computedDistance2 = distCalc.HaversineDistance(396.12, -86.67, 33.94, -118.4);
            Assert.AreEqual(Math.Floor(expectedDistanceM), Math.Floor(computedDistance2));
        }


        [Test]
        public void EquirectangularProjectionDistanceTest()
        {
            // The distance calculated with the equirectangular projection is slightly different from the Haversine distance
            var coord1 = new Coordinate(36.12, -86.67);
            var coord2 = new Coordinate(33.94, -118.4);
            var distCalc = new GeoDistanceCalculator();
            var expectedDistanceM = 2900055.0;
            var computedDistance = distCalc.EquirectangularProjectionDistance(coord1, coord2);

            Assert.AreEqual(Math.Floor(expectedDistanceM), Math.Floor(computedDistance));
            Assert.AreEqual(expectedDistanceM, Math.Floor(distCalc.EquirectangularProjectionDistance(36.12, -86.67, 33.94, -118.4)));
            
            Assert.AreEqual(0, Math.Floor(distCalc.EquirectangularProjectionDistance(1.25489, 65.698712, 1.25489, 65.698712)));
        }

        [Test]
        public void EquirectangularProjectionVsHaversineDistanceTest()
        {
            // The error between the two methods should be minimal for coordinate relatively close
            var coord1 = new Coordinate(36.12, -86.67);
            var coord2 = new Coordinate(36.94, -86.4);
            var distCalc = new GeoDistanceCalculator();
            var expectedDistanceM = distCalc.HaversineDistance(coord1, coord2);
            var computedDistance = distCalc.EquirectangularProjectionDistance(coord1, coord2);

            Assert.AreEqual(Math.Floor(expectedDistanceM), Math.Floor(computedDistance));
        }
    }
}
