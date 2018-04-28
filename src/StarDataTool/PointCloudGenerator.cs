using System;
using StarLib;

namespace StarDataTool
{
    public class PointCloudGenerator
    {
        public void Generate(string path, long count)
        {
            var random = new Random();

            using (var ss = StarFile.Create(path, count))
            {
                for (long i = 0; i < count; i++)
                {
                    var observation = new StarObservation
                    {
                        RightAscension = random.NextDouble() * 360.0,
                        Declination = 90.0 * (random.NextDouble() -0.5),
                        Parallax = 0.00001 + 0.0002 * random.NextDouble(),
                        ApparentMagnitude = (float)(20.0 * (random.NextDouble() - 0.05)),
                        Temperature = 4000f
                    };

                    if (i % 1_000_000 == 0)
                        Console.WriteLine($"Generated {i}");

                    ss[i] = Star.FromObservation(observation);
                }
            }

        }
    }
}