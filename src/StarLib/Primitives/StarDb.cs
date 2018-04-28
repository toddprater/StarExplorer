using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace StarLib.Primitives
{
    public class StarDb: IStarSet, IDisposable
    {
        private const int StarSizeInBytes = sizeof(double)    // X
                                          + sizeof(double)    // Y
                                          + sizeof(double)    // Z
                                          + sizeof(float)     // AbsoluteMagnitude
                                          + sizeof(float);    // Temperature

        private readonly MemoryMappedFile file;
        private readonly MemoryMappedViewAccessor view;
        private readonly long capacity;

        public StarDb(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    "The specified star data file was not found", path);

            file = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
            capacity = file.CreateViewAccessor(0, sizeof(long)).ReadInt64(0);
            view = file.CreateViewAccessor(sizeof(long), capacity * StarSizeInBytes);
        }

        public void Dispose()
        {
            view.Dispose();
            file.Dispose();
        }

        public IEnumerable<IStarSet> Subsets { get; } = new List<IStarSet>();

        public IEnumerable<Star> Stars
        {
            get
            {
                for (long i = 0; i < capacity; i++)
                {
                    var pos = i * StarSizeInBytes;

                    var x = view.ReadDouble(pos);
                    pos += sizeof(double);
                    var y = view.ReadDouble(pos);
                    pos += sizeof(double);
                    var z = view.ReadDouble(pos);
                    pos += sizeof(double);
                    var mag = view.ReadSingle(pos);
                    pos += sizeof(float);
                    var temp = view.ReadSingle(pos);

                    yield return new Star { X = x, Y = y, Z = z, AbsoluteMagnitude = mag, Temperature = temp };
                }
            }
        }

        private IEnumerable<IEnumerable<Star>> AllSubsets()
        {
            return Subsets.Select(p => p.AllStars());
        }

        public IEnumerable<Star> AllStars()
        {
            foreach (var subsets in AllSubsets())
                foreach (var star in subsets)
                    yield return star;

            foreach (var star in Stars)
                yield return star;
        }
    }
}
