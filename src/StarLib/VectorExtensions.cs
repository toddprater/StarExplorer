using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace StarLib
{
    public static class VectorExtensions
    {
        public static Vector<double> CrossProduct(this Vector<double> a, Vector<double> b)
        {
            return DenseVector.OfArray(new []
            {
                a[1] * b[2] - b[1] * a[2],    //   v1y*v2z - v2y*v1z
                b[0] * a[2] - a[0] * b[2],    //   v2x*v1z - v1x*v2z
                a[0] * b[1] - b[0] * a[1]     //   v1x*v2y - v2x*v1y
            });
        }
    }
}