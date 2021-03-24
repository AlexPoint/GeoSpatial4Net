using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace GeoSpatial4Net.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = new List<DistanceComputationResult>();
            var stopWatch = new Stopwatch();

            var step = 5;

            var distCalc = new GeoDistanceCalculator();

            var outputFilePath = Path.Combine(GetApplicationRoot(), "Output\\results.csv");
            using (var writer = new StreamWriter(outputFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<DistanceComputationResult>();
                csv.NextRecord();

                for (var i = -180; i <= 180; i+=step)
                {
                    Console.WriteLine("Coordinate 1 ; longitude {0}", i);
                    for (var j = -90; j <= 90; j+=step)
                    {
                        for (var k = -180; k <= 180; k+=step)
                        {
                            for (var l = -90; l <= 90; l+=step)
                            {
                                // Haversine
                                stopWatch.Start();
                                var haversineDistance = distCalc.HaversineDistance(j, i, l, k);
                                stopWatch.Stop();

                                var haversineEllapsed = stopWatch.Elapsed.TotalMilliseconds;

                                stopWatch.Reset();

                                // SLC
                                stopWatch.Start();
                                var slcDistance = distCalc.SLCDistance(j, i, l, k);
                                stopWatch.Stop();

                                var slcEllapsed = stopWatch.Elapsed.TotalMilliseconds;

                                stopWatch.Reset();
                                
                                // Equirectangular
                                stopWatch.Start();
                                var equirectangularDistance = distCalc.EquirectangularProjectionDistance(j, i, l, k);
                                stopWatch.Stop();

                                var equirectangularEllapsed = stopWatch.Elapsed.TotalMilliseconds; 
                                stopWatch.Reset();

                                // Vincenti
                                stopWatch.Start();
                                double vincentiDistance = -1;
                                try
                                {
                                    vincentiDistance = distCalc.VincentiDistance(j, i, l, k);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Vincenti distance calculation failed between ({0}, {1}) and ({2}, {3})", j, i, l, k);
                                }
                                stopWatch.Stop();

                                var vincentiEllapsed = stopWatch.Elapsed.TotalMilliseconds;
                                stopWatch.Reset();

                                // Write CSV record
                                csv.WriteRecord(new DistanceComputationResult()
                                {
                                    Coordinate1Latitude = j,
                                    Coordinate1Longitude = i,
                                    Coordinate2Latitude = l,
                                    Coordinate2Longitude = k,
                                    HaversineDistance = haversineDistance,
                                    HaversineEllapsedMilliseconds = haversineEllapsed,
                                    SLCDistance = slcDistance,
                                    SLCEllapsedMilliseconds = slcEllapsed,
                                    EquirectangularDistance = equirectangularDistance,
                                    EquirectangularEllapsedMilliseconds = equirectangularEllapsed,
                                    VincentiDistance = vincentiDistance,
                                    VincentiEllapsedMilliseconds = vincentiEllapsed
                                });
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }


        }

        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                              .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}
