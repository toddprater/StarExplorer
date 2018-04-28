using System.Collections.Generic;
using System.Linq;

namespace StarLib.Primitives
{
    public class Line: IStarSet
    {
        private readonly List<Star> stars = new List<Star>();

        public IEnumerable<IStarSet> Subsets { get; } = new List<IStarSet>();
        public IEnumerable<Star> Stars => stars;

        public Line(XyzPoint p1, XyzPoint p2, float mag, float temp, int n)
        {
            var dx = (p2.X - p1.X) / n;
            var dy = (p2.Y - p1.Y) / n;
            var dz = (p2.Z - p1.Z) / n;

            for (var i = 0; i < n; i++)
                stars.Add(new Star
                {
                    X = p1.X + dx * i,
                    Y = p1.Y + dy * i,
                    Z = p1.Z + dz * i,
                    AbsoluteMagnitude = mag,
                    Temperature = temp
                });
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
