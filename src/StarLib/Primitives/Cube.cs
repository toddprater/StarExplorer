using System.Collections.Generic;
using System.Linq;

namespace StarLib.Primitives
{
    public class Cube: IStarSet
    {
        private readonly List<IStarSet> subsets = new List<IStarSet>();

        public IEnumerable<IStarSet> Subsets => subsets;
        public IEnumerable<Star> Stars { get; } = new List<Star>();

        public Cube(XyzPoint p1, XyzPoint p2, float mag, float temp, int n)
        {
            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p1.Y, Z = p1.Z},
                new XyzPoint {X = p2.X, Y = p1.Y, Z = p1.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p1.Y, Z = p1.Z},
                new XyzPoint {X = p1.X, Y = p2.Y, Z = p1.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p2.Y, Z = p1.Z},
                new XyzPoint {X = p2.X, Y = p2.Y, Z = p1.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p2.X, Y = p2.Y, Z = p1.Z},
                new XyzPoint {X = p2.X, Y = p1.Y, Z = p1.Z}, mag, temp, n));

            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p1.Y, Z = p2.Z},
                new XyzPoint {X = p2.X, Y = p1.Y, Z = p2.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p1.Y, Z = p2.Z},
                new XyzPoint {X = p1.X, Y = p2.Y, Z = p2.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p2.Y, Z = p2.Z},
                new XyzPoint {X = p2.X, Y = p2.Y, Z = p2.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p2.X, Y = p2.Y, Z = p2.Z},
                new XyzPoint {X = p2.X, Y = p1.Y, Z = p2.Z}, mag, temp, n));

            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p1.Y, Z = p1.Z},
                new XyzPoint {X = p1.X, Y = p1.Y, Z = p2.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p2.X, Y = p1.Y, Z = p1.Z},
                new XyzPoint {X = p2.X, Y = p1.Y, Z = p2.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p2.X, Y = p2.Y, Z = p1.Z},
                new XyzPoint {X = p2.X, Y = p2.Y, Z = p2.Z}, mag, temp, n));
            subsets.Add(new Line(new XyzPoint {X = p1.X, Y = p2.Y, Z = p1.Z},
                new XyzPoint {X = p1.X, Y = p2.Y, Z = p2.Z}, mag, temp, n));
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