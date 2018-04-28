using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace StarLib
{
    public class ImageSensor
    {
        // Probably could have things like simulated ISO, and
        //    how to handle any effects like 'starburst', 'lens flare',
        //    light bleeding to adjacent pixels, etc

        // Not sure if lens effects are generated because of the bleeding
        //    to adjacent pixels, or because of mechanical details about
        //    how telescopes work optically. Research needed.

        public int Width { get; }
        public int Height { get; }

        private readonly Matrix<float> r;
        private readonly Matrix<float> g;
        private readonly Matrix<float> b;

        public RgbColor this[int x, int y] => new RgbColor {R = r[x, y], G = g[x, y], B = b[x, y]};

        public ImageSensor(int width, int height)
        {
            Width = width;
            Height = height;

            r = DenseMatrix.Create(width, height, 0.0f);
            g = DenseMatrix.Create(width, height, 0.0f);
            b = DenseMatrix.Create(width, height, 0.0f);
        }

        public void Accumulate(double px, double py, float mag, float temp)
        {
            var xx = (int)Math.Round((px + 1) / 2 * Width);
            var yy = (int)Math.Round((1 - py) / 2 * Height);

            if (xx >= Width || xx < 0 || yy >= Height || yy < 0)
                return;

            var color = RgbColor.FromTemperature(temp);

            var maxIntensity = 1000000000000.0;
            var maxMagnitude = -4.0;

            var intensity = (float)(maxIntensity / Math.Pow(2.512, mag - maxMagnitude));
            
            r[xx, yy] += color.R * intensity;
            g[xx, yy] += color.G * intensity;
            b[xx, yy] += color.B * intensity;
        }

        public void CaptureFrame(Matrix<float> rBuffer, Matrix<float> gBuffer, Matrix<float> bBuffer)
        {
            r.CopyTo(rBuffer);
            g.CopyTo(gBuffer);
            b.CopyTo(bBuffer);
        }

        public void ClearFrame()
        {
            r.Clear();
            g.Clear();
            b.Clear();
        }
    }
}
