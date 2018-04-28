using System;
using System.Collections.Generic;
using System.Linq;

namespace StarLib.Primitives
{
    public class PointCloud: IStarSet
    {
        private readonly List<Star> stars = new List<Star>();

        public IEnumerable<IStarSet> Subsets { get; } = new List<IStarSet>();
        public IEnumerable<Star> Stars => stars;

        public PointCloud(double scale, float mag, float temp, int numPoints)
        {
            var random = new Random();

            for (var i = 0; i < numPoints; i++)
            {
                stars.Add(new Star
                {
                    X = scale * (random.NextDouble() - 0.5),
                    Y = scale * (random.NextDouble() - 0.5),
                    Z = scale * (random.NextDouble() - 0.5),
                    AbsoluteMagnitude = mag,
                    Temperature = temp
                });
            }
        }

        private IEnumerable<IEnumerable<Star>> AllSubsets()
        {
            return Subsets.Select(subset => subset.AllStars());
        }

        public IEnumerable<Star> AllStars()
        {
            foreach (var subset in AllSubsets())
                foreach (var star in subset)
                    yield return star;

            foreach (var star in Stars)
                yield return star;
        }
    }
}