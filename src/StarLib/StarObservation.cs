using System;

namespace StarLib
{
    public struct StarObservation
    {
        public double RightAscension;
        public double Declination;
        public double Parallax;
        public float ApparentMagnitude;
        public float Temperature;

        public CelestialPoint Position => new CelestialPoint
        {
            RightAscension = RightAscension,
            Declination = Declination,
            Distance = Distance
        };

        public double Distance
            => Math.Abs(CelestialPoint.ParallaxToParsecs(Parallax));

        public float AbsoluteMagnitude
            => (float)(ApparentMagnitude - 5.0 * Math.Log(Distance / 10.0));
    }
}