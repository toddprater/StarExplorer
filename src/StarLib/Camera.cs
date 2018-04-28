using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace StarLib
{
    public class Camera
    {
        private Matrix<double> viewMatrix = DenseMatrix.CreateIdentity(4);
        private Matrix<double> projectionMatrix = DenseMatrix.CreateIdentity(4);
        private Matrix<double> combinedMatrix;

        public Camera()
        {
            Eye = DenseVector.OfArray(new[] { 1000000.0, 1000000.0, 1000000.0 });
            Target = DenseVector.OfArray(new[] { 0.0, 0.0, 0.0 });
            Up = DenseVector.OfArray(new[] { 0.0, 1.0, 0.0 });
            Fov = Trig.DegreeToRadian(40.0);

            SetViewMatrix();
            SetProjectionMatrix();
        }

        public Vector<double> Eye { get; set; }
        public Vector<double> Target { get; set; }
        public Vector<double> Up { get; set; }
        public double Fov { get; set; }

        private double aspect = 1.0;
        public double Aspect
        {
            get => aspect;
            set
            {
                aspect = value;
                SetProjectionMatrix();
            }
        }

        public void MoveForward(double distance)
        {
            Eye += (Target - Eye).Normalize(2) * distance;
            SetViewMatrix();
        }


        public void RotateTrackball(Vector<double> axis, double degrees)
        {
            var a = Trig.DegreeToRadian(degrees);

            axis = axis.Normalize(2);

            var x = axis[0];
            var y = axis[1];
            var z = axis[2];

            var cosa = Trig.Cos(a);
            var mcosa = 1.0 - cosa;
            var sina = Trig.Sin(a);

            var m = Matrix.Build.DenseOfColumnMajor(3, 3, new[]
            {
                 cosa + x*x*mcosa,    x*y*mcosa - z*sina,   x*z*mcosa + y*sina,
                y*x*mcosa + z*sina,    cosa + y*y*mcosa,    y*z*mcosa - x*sina,
                z*x*mcosa - y*sina,   z*y*mcosa + x*sina,    cosa + z*z*mcosa
            });

            Eye = m.Transpose().Multiply(Eye);
            Up = m.Transpose().Multiply(Up);

            SetViewMatrix();
        }


        public void RotateZ(double degrees)
        {
            degrees = Trig.DegreeToRadian(degrees);

            var m = DenseMatrix.Build.DenseOfColumnMajor(3, 3, new[]
            {
                Trig.Cos(degrees), -Trig.Sin(degrees), 0,
                Trig.Sin(degrees), Trig.Cos(degrees), 0,
                0, 0, 1
            });

            Eye = m.Multiply(Eye);
            Up = m.Multiply(Up);

            SetViewMatrix();
        }

        public void RotateY(double degrees)
        {
            degrees = Trig.DegreeToRadian(degrees);

            var m = DenseMatrix.Build.DenseOfColumnMajor(3, 3, new[]
            {
                Trig.Cos(degrees), 0, Trig.Sin(degrees),
                0, 1, 0,
                -Trig.Sin(degrees), 0, Trig.Cos(degrees)
            });

            Eye = m.Multiply(Eye);
            Up = m.Multiply(Up);

            SetViewMatrix();
        }

        public void RotateX(double degrees)
        {
            degrees = Trig.DegreeToRadian(degrees);

            var m = DenseMatrix.Build.DenseOfColumnMajor(3, 3, new[]
            {
                1, 0, 0,
                0, Trig.Cos(degrees), -Trig.Sin(degrees),
                0, Trig.Sin(degrees), Trig.Cos(degrees),
            });

            Eye = m.Multiply(Eye);
            Up = m.Multiply(Up);

            SetViewMatrix();
        }

        public void SetCombinedMatrix()
        {
            combinedMatrix = projectionMatrix.Multiply(viewMatrix);
        }

        public void SetViewMatrix()
        {
            var zaxis = (Eye - Target).Normalize(2);
            var xaxis = Up.CrossProduct(zaxis).Normalize(2);
            var yaxis = zaxis.CrossProduct(xaxis);

            viewMatrix = Matrix.Build.DenseOfColumnMajor(4, 4, new[]
            {
                       xaxis[0],               yaxis[0],               zaxis[0],        0.0,
                       xaxis[1],               yaxis[1],               zaxis[1],        0.0,
                       xaxis[2],               yaxis[2],               zaxis[2],        0.0,
                -xaxis.DotProduct(Eye), -yaxis.DotProduct(Eye), -zaxis.DotProduct(Eye), 1.0
            });

            SetCombinedMatrix();
        }

        public void SetProjectionMatrix()
        {
            const double znear = 1.0;
            const double zfar = 100000.0;

            var yScale = 1.0 / Math.Tan(Fov * 0.5);
            var q = zfar / (znear - zfar);

            projectionMatrix = Matrix.Build.DenseOfColumnMajor(4, 4, new[]
            {
                yScale/Aspect,   0.0,       0.0,       0.0,
                     0.0,       yScale,     0.0,       0.0,
                     0.0,        0.0,        q,       -1.0,
                     0.0,        0.0,      q*znear,    0.0
            });

            SetCombinedMatrix();
        }

        public Vector<double> Project(XyzPoint p) // to viewport
        {
            var h = DenseVector.OfArray(new[] { p.X, p.Y, p.Z, 1.0 });
            var x = combinedMatrix.Multiply(h);
            return (x / x[3]).SubVector(0, 2);
        }
    }
}
