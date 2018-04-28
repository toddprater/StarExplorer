namespace StarLib
{
    public struct RgbColor
    {
        public float R;
        public float G;
        public float B;

        public static RgbColor FromTemperature(double temperature)
        {
            var x = (float)(temperature / 1000.0);
            var x2 = x * x;
            var x3 = x2 * x;
            var x4 = x3 * x;
            var x5 = x4 * x;

            var r = (temperature <= 6600)
                ? 1f
                : 0.0002889f * x5 - 0.01258f * x4 + 0.2148f * x3 - 1.776f * x2 + 6.907f * x - 8.723f;

            var g = (temperature <= 6600)
                ? -4.593e-05f * x5 + 0.001424f * x4 - 0.01489f * x3 + 0.0498f * x2 + 0.1669f * x - 0.1653f
                : -1.308e-07f * x5 + 1.745e-05f * x4 - 0.0009116f * x3 + 0.02348f * x2 - 0.3048f * x + 2.159f;

            var b = (temperature <= 2000f)
                ? 0f
                : (temperature < 6600f)
                    ? 1.764e-05f * x5 + 0.0003575f * x4 - 0.01554f * x3 + 0.1549f * x2 - 0.3682f * x + 0.2386f
                    : 1f;

            return new RgbColor { R = r, G = g, B = b };
        }
    }
}