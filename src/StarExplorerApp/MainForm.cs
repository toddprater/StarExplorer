using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using StarLib;
using StarLib.Primitives;

namespace StarExplorerApp
{
    public partial class MainForm : Form
    {
        private Bitmap canvas;
        private LockBitmap canvasLock;
        private bool hasBeenInitialized;

        private ImageSensor imageSensor;
        private readonly Camera camera = new Camera();
        private readonly World world = new World();

        public int AreaHeight { get; set; }
        public int AreaWidth { get; set; }

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, true);

            SetStyle(ControlStyles.DoubleBuffer, false);
        }

        private static bool IsApplicationIdle()
        {
            return PeekMessage(out NativeMessage _, IntPtr.Zero, 0, 0, 0) == 0;
        }

        private void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                canvasLock.LockBits();
                canvasLock.Fill(0xff000000);

                Animate();
                Render();

                canvasLock.UnlockBits();
                Invalidate();
            }
        }

        private void Animate()
        {
            imageSensor.ClearFrame();
        }

        private void Render()
        {
            var r = new Renderer(camera, world);

            r.Render(imageSensor, (x, y, v) =>
                canvasLock.SetPixel(x, y, GetHexColor(v)));
        }

        private uint GetHexColor(RgbColor c)
        {
            if (c.R == 0f && c.G == 0f && c.B == 0f)
                return 0xff000000;

            return 0xffffffff;

            var r = c.R;
            var g = c.G;
            var b = c.B;

            if (r > 255.0) r = 255f;
            if (g > 255.0) g = 255f;
            if (b > 255.0) b = 255f;

            return 0xff000000 | (uint)b | (uint)g << 8 | (uint)r << 16;
        }

        private void Init()
        {
            canvasLock = new LockBitmap(canvas);
            Application.Idle += HandleApplicationIdle;

            world.StarSets.Add(new Cube(new XyzPoint { X=-100000.0, Y=-100000.0, Z=-100000.0}, new XyzPoint{X=100000.0, Y=100000.0, Z=100000.0}, -7.0f, 1500f, 60000));
            world.StarSets.Add(new Axes(1000.0, 100));
            world.StarSets.Add(new StarDb(@"C:\dev\temp\stars1k.dat"));
        }


        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(canvas, 0, 0);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!hasBeenInitialized)
            {
                AreaWidth = Width;
                AreaHeight = Height;

                imageSensor = new ImageSensor(Width, Height);

                hasBeenInitialized = true;

                canvas = new Bitmap(AreaWidth, AreaHeight);

                Init();
            }
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message,
            IntPtr window, uint filterMin, uint filterMax, uint remove);

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            var axis = camera.Target - camera.Eye;
            var unit = axis.Normalize(2);
            var scale = axis.L2Norm();
            var right = unit.CrossProduct(camera.Up).Normalize(2);
            var up = axis.CrossProduct(right);

            switch (e.KeyChar)
            {
                case 'h':
                    camera.RotateTrackball(right, -5.0);
                    break;
                case 'l':
                    camera.RotateTrackball(right, 5.0);
                    break;
                case 'j':
                    camera.RotateTrackball(up, -5.0);
                    break;
                case 'k':
                    camera.RotateTrackball(up, 5.0);
                    break;
                case 'a':
                    camera.MoveForward(scale * 0.2);
                    break;
                case 's':
                    camera.MoveForward(-scale * 0.2);
                    break;
            }

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            AreaWidth = Width;
            AreaHeight = Height;
            camera.Aspect = (double)Width / Height;
            imageSensor = new ImageSensor(Width, Height);
            canvas = new Bitmap(AreaWidth, AreaHeight);
            canvasLock = new LockBitmap(canvas);
        }

        private int lastMouseX = MousePosition.X;
        private int lastMouseY = MousePosition.Y;
        private bool mouseButtonDown = false;
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastMouseX = MousePosition.X;
            lastMouseY = MousePosition.Y;
            mouseButtonDown = true;
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseButtonDown = false;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseButtonDown)
                return;

            double dx = MousePosition.X - lastMouseX;
            double dy = MousePosition.Y - lastMouseY;
            
            lastMouseX = MousePosition.X;
            lastMouseY = MousePosition.Y;

            var axis = camera.Target - camera.Eye;
            var unit = axis.Normalize(2);
            //var scale = axis.L2Norm();
            var right = unit.CrossProduct(camera.Up).Normalize(2);
            var up = axis.CrossProduct(right);

            Console.WriteLine($"Move: <{dx}, {dy}> -- Right<{right}>, Up<{up}>");

            camera.RotateTrackball(right, -0.5 * dy);
            camera.RotateTrackball(up, 0.5 * dx);
        }
    }
}
