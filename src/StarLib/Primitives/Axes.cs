using System.Collections.Generic;
using System.Linq;

namespace StarLib.Primitives
{
    public class Axes: IStarSet
    {
        private readonly List<IStarSet> subsets = new List<IStarSet>();

        public IEnumerable<IStarSet> Subsets => subsets;
        public IEnumerable<Star> Stars { get; } = new List<Star>();

        public Axes(double scale, int n)
        {
            var origin = new XyzPoint {X = 0.0, Y = 0.0, Z = 0.0};
            var x = new XyzPoint {X = scale, Y = 0.0, Z = 0.0};
            var y = new XyzPoint {X = 0.0, Y = scale, Z = 0.0};
            var z = new XyzPoint {X = 0.0, Y = 0.0, Z = scale};

            subsets.Add(new Line(origin, x, 0.0f, 4000.0f, n));
            subsets.Add(new Line(origin, y, 0.0f, 4000.0f, n));
            subsets.Add(new Line(origin, z, 0.0f, 4000.0f, n));
        }

        private IEnumerable<IEnumerable<Star>> AllSubsets()
        {
            return Subsets.Select(p => p.AllStars());
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