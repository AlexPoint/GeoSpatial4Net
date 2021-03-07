# GeoSpatial4Net

.NET core library with basic geospatial operations such as distance calculation or conversion.
Feel free to reach out if operations are missing or better, do a pull request!

## Install
This library is available on [nuget](https://www.nuget.org/packages/GeoSpatial4Net/).
To install it via the package manager:
`PM> Install-Package GeoSpatial4Net`

## Quick start

### Distance calculation

Compute the distance (in meters by default) between two coordinates using the Haversine formula:
```cs
var coord1 = new Coordinate(36.12, -86.67);
var coord2 = new Coordinate(33.94, -118.4);
var distCalc = new GeoDistanceCalculator();
var computedDistance = distCalc.HaversineDistance(coord1, coord2);
```

You can also call the method directly with the 2 points' latitude/longitude:
```cs
var distCalc = new GeoDistanceCalculator();
var computedDistance2 = distCalc.HaversineDistance(36.12, -86.67, 33.94, -118.4);
```

### Distance conversion

All distances are returned by default in meters, the base unit of length in the International System of Units.
You can, however, convert those distances in other units:
```cs
var distanceConverter = new DistanceConverter();
// converts 1500 meters in miles
var distanceM = distanceConverter.ConvertDistance(1500, DistanceUnit.Meter, DistanceUnit.Mile);
```

## SaaS solution

If you are looking a ready-to-use solution for geospatial operations, you can check out [proximity-map](http://www.proximity-map.com).
For any address, find easily the closest location in a set of pre-defined addresses (flying and driving distances).