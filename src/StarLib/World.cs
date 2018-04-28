using System.Collections.Generic;

namespace StarLib
{
    public class World
    {
        public List<IStarSet> StarSets { get; }

        public World()
        {
            StarSets = new List<IStarSet>();
        }
    }
}
