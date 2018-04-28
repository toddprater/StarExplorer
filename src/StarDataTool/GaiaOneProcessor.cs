using System;
using System.IO;
using CsvHelper;
using StarLib;

namespace StarDataTool
{
    public class GaiaOneProcessor
    {
        public void Process()
        {
            var files = new[]
            {
                @"Z:\Todd\stars\TgasSource_000-000-000.csv",
                @"Z:\Todd\stars\TgasSource_000-000-001.csv",
                @"Z:\Todd\stars\TgasSource_000-000-002.csv",
                @"Z:\Todd\stars\TgasSource_000-000-003.csv",
                @"Z:\Todd\stars\TgasSource_000-000-004.csv",
                @"Z:\Todd\stars\TgasSource_000-000-005.csv",
                @"Z:\Todd\stars\TgasSource_000-000-006.csv",
                @"Z:\Todd\stars\TgasSource_000-000-007.csv",
                @"Z:\Todd\stars\TgasSource_000-000-008.csv",
                @"Z:\Todd\stars\TgasSource_000-000-009.csv",
                @"Z:\Todd\stars\TgasSource_000-000-010.csv",
                @"Z:\Todd\stars\TgasSource_000-000-011.csv",
                @"Z:\Todd\stars\TgasSource_000-000-012.csv",
                @"Z:\Todd\stars\TgasSource_000-000-013.csv",
                @"Z:\Todd\stars\TgasSource_000-000-014.csv",
                @"Z:\Todd\stars\TgasSource_000-000-015.csv"
            };

            var i = 0;
            var count = 0;

            foreach (var file in files)
            {
                using (var stream = File.OpenText(file))
                {
                    using (var parser = new CsvParser(stream))
                    {
                        parser.Read();

                        while (true)
                        {
                            var fields = parser.Read();

                            if (fields == null)
                                break;

                            if (i++ % 2000 == 0)
                                count++;
                        }
                    }
                }

                Console.WriteLine($"Found {count} stars");
            }


            using (var ss = StarFile.Create(@"C:\dev\temp\stars1k.dat", count))
            {
                i = 0;
                count = 0;

                foreach (var file in files)
                {
                    using (var stream = File.OpenText(file))
                    {
                        using (var parser = new CsvParser(stream))
                        {
                            parser.Read();

                            while (true)
                            {
                                var fields = parser.Read();

                                if (fields == null)
                                    break;

                                var observation = new StarObservation
                                {
                                    RightAscension = double.Parse(fields[6]),
                                    Declination = double.Parse(fields[8]),
                                    Parallax = double.Parse(fields[10]),
                                    ApparentMagnitude = float.Parse(fields[53]),
                                    Temperature = 4000f

                                };

                                var raError = double.Parse(fields[7]);
                                var pxError = float.Parse(fields[11]);

                                if (observation.Parallax < 0.02 || pxError > 0.7 || raError > 0.7)
                                    observation.ApparentMagnitude = 0.0f;

                                if (i++ % 2000 == 0)
                                    ss[count++] = Star.FromObservation(observation);
                            }
                        }
                    }

                    Console.WriteLine($"Processed {file}");
                    ss.Flush();
                }
            }

        }
    }
}