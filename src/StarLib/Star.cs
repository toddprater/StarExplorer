using System;

namespace StarLib
{
    public struct Star
    {
        public double X;
        public double Y;
        public double Z;
        public float AbsoluteMagnitude;
        public float Temperature;

        public static Star FromObservation(StarObservation observation)
        {
            var p = observation.Position.ToXyzPoint();
            var mag = observation.AbsoluteMagnitude;

            return new Star
            {
                X = p.X, Y = p.Y, Z = p.Z,
                AbsoluteMagnitude = mag,
                Temperature = observation.Temperature
            };
        }

        public float ApparentMagnitude(double distance)
        {
            return (float)(5.0 * Math.Log(Math.Abs(distance) / 10.0) + AbsoluteMagnitude);
        }

        public float ApparentMagnitude(XyzPoint p)
        {
            return ApparentMagnitude(Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z));
        }

        public RgbColor Color => RgbColor.FromTemperature(Temperature);
    }
}
