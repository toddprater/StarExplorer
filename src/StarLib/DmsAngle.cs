using System;

namespace StarLib
{
    public struct DmsAngle
    {
        public int Degrees;
        public int Minutes;
        public double Seconds;

        public double DecimalDegrees => Degrees + (Minutes / 60.0) + (Seconds / 3600.0);

        public static DmsAngle FromDecimal(double decimalDegrees)
        {
            var ddAbs = Math.Abs(decimalDegrees);
            var ddSign = Math.Sign(decimalDegrees);
            var degs = (int)ddAbs;
            var minsec = ddAbs - degs;
            var mins = (int)(minsec * 60.0);
            var secs = (minsec - mins / 60.0) * 3600.0;
            degs *= ddSign;

            return new DmsAngle
            {
                Degrees = degs,
                Minutes = mins,
                Seconds = secs
            };
        }
    }
}