using NUnit.Framework;

namespace GeoSpatial4Net.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DistanceConversion()
        {
            var distanceKm = 3;

            // Conversion of 3km to meters
            var distanceConverter = new DistanceConverter();
            var distanceM = distanceConverter.ConvertDistance(distanceKm, DistanceUnit.Kilometer, DistanceUnit.Meter);
            Assert.AreEqual(3 * 1000, distanceM);

            Assert.AreNotEqual(5 * 1000, distanceM);

            // Conversion from and to the same unit should return the same value
            var distance = 5.2;
            Assert.AreEqual(distance, distanceConverter.ConvertDistance(distance, DistanceUnit.Meter, DistanceUnit.Meter));
            Assert.AreEqual(distance, distanceConverter.ConvertDistance(distance, DistanceUnit.Kilometer, DistanceUnit.Kilometer));
            Assert.AreEqual(distance, distanceConverter.ConvertDistance(distance, DistanceUnit.Mile, DistanceUnit.Mile));
            Assert.AreEqual(distance, distanceConverter.ConvertDistance(distance, DistanceUnit.Feet, DistanceUnit.Feet));

            //Assert.Pass();
        }
    }
}