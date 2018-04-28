using System.Collections.Generic;

namespace StarLib
{
    public interface IStarSet
    {
        IEnumerable<IStarSet> Subsets { get; }
        IEnumerable<Star> Stars { get; }
        IEnumerable<Star> AllStars();
    }
}
