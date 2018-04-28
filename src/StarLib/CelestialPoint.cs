using System;
using MathNet.Numerics;

namespace StarLib
{
    public struct CelestialPoint
    {
        public double RightAscension;
        public double Declination;
        public double Distance;

        public XyzPoint ToXyzPoint()
        {
            //var dec = DmsAngle.FromDecimal(Declination);

            //var b = Math.Sign(Declination) *
            //        (Math.Abs(dec.Degrees) + dec.Minutes / 60.0 + dec.Seconds / 3600.0);

            var ra = Trig.DegreeToRadian(RightAscension);
            var dec = Trig.DegreeToRadian(Declination);

            return new XyzPoint
            {
                X = Distance * Math.Cos(dec) * Math.Cos(ra),
                Y = Distance * Math.Cos(dec) * Math.Sin(ra),
                Z = Distance * Math.Sin(dec)
            };
        }

        public static double ParallaxToParsecs(double parallax)
        {
            return 1000.0 * (1.0 / parallax);
        }
    }
}