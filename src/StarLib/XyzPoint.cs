using System;

namespace StarLib
{
    public struct XyzPoint
    {
        public double X;
        public double Y;
        public double Z;

        public double DistanceTo(XyzPoint p)
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
    }
}