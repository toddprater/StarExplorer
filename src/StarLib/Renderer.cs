using System;

namespace StarLib
{
    public class Renderer
    {
        private readonly Camera camera;
        private readonly World world;

        public Renderer(Camera camera, World world)
        {
            this.camera = camera;
            this.world = world;
        }

        public void Render(ImageSensor imageSensor, Action<int, int, RgbColor> renderAction)
        {
            imageSensor.ClearFrame();

            var eye = new XyzPoint {X = camera.Eye[0], Y = camera.Eye[1], Z = camera.Eye[2]};

            foreach (var starSet in world.StarSets)
            {
                foreach (var star in starSet.AllStars())
                {
                    var vp = camera.Project(new XyzPoint {X = star.X, Y = star.Y, Z = star.Z});
                    imageSensor.Accumulate(vp[0], vp[1], star.ApparentMagnitude(eye), star.Temperature);
                }
            }

            for (var x = 0; x < imageSensor.Width; x++)
            {
                for (var y = 0; y < imageSensor.Height; y++)
                {
                    renderAction(x, y, imageSensor[x, y]);
                }
            }
        }
    }
}
