using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace StarLib
{
    public class StarFile: IDisposable, IEnumerable<Star>
    {
        private const int StarSizeInBytes = sizeof(double)    // X
                                          + sizeof(double)    // Y
                                          + sizeof(double)    // Z
                                          + sizeof(float)     // AbsoluteMagnitude
                                          + sizeof(float);    // Temperature

        private readonly MemoryMappedFile file;
        private readonly MemoryMappedViewAccessor view;
        private readonly long capacity;


        private StarFile(MemoryMappedFile file, MemoryMappedViewAccessor view, long capacity)
        {
            this.file = file;
            this.view = view;
            this.capacity = capacity;
        }

        public static StarFile Create(string path, long capacity)
        {
            var file = MemoryMappedFile.CreateFromFile(path, FileMode.Create,
                Guid.NewGuid().ToString("N"), sizeof(long) + capacity * StarSizeInBytes);

            file.CreateViewAccessor(0, sizeof(long)).Write(0, capacity);

            var view = file.CreateViewAccessor(sizeof(long), capacity * StarSizeInBytes);

            return new StarFile(file, view, capacity);
        }

        public static StarFile OpenReadOnly(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    "The specified star data file was not found", path);
            }

            var file = MemoryMappedFile.CreateFromFile(path, FileMode.Open);

            var capacity = file.CreateViewAccessor(0, sizeof(long)).ReadInt64(0);

            var view = file.CreateViewAccessor(sizeof(long), capacity * StarSizeInBytes);

            return new StarFile(file, view, capacity);
        }

        public Star this[long i]
        {
            get
            {
                if (i > capacity)
                    throw new ArgumentOutOfRangeException();

                var pos = i * StarSizeInBytes;
                var star = new Star {X = view.ReadDouble(pos)};
                pos += sizeof(double);
                star.Y = view.ReadDouble(pos);
                pos += sizeof(double);
                star.Z = view.ReadDouble(pos);
                pos += sizeof(double);
                star.AbsoluteMagnitude = view.ReadSingle(pos);
                pos += sizeof(float);
                star.Temperature = view.ReadSingle(pos);
                return star;
            }
            set
            {
                if (i > capacity)
                    throw new ArgumentOutOfRangeException();

                var pos = i * StarSizeInBytes;
                view.Write(pos, value.X);
                pos += sizeof(double);
                view.Write(pos, value.Y);
                pos += sizeof(double);
                view.Write(pos, value.Z);
                pos += sizeof(double);
                view.Write(pos, value.AbsoluteMagnitude);
                pos += sizeof(float);
                view.Write(pos, value.Temperature);
            }
        }

        public void Flush()
        {
            view.Flush();
        }

        public void Dispose()
        {
            view.Dispose();
            file.Dispose();
        }

        public IEnumerator<Star> GetEnumerator()
        {
            for (var i = 0; i < capacity; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


//public void WriteExample(string dataPath, long numStars)
//{
//    var r = new Random();
//    const long n = 1_000_000_000L;

//    using (var mmfile = MemoryMappedFile.CreateFromFile(
//        @"D:\temp\mmtest.data", FileMode.Create, "mmtest.data", sizeof(double) * n))
//    {
//        using (var mm = mmfile.CreateViewAccessor())
//        {
//            for (var i = 0L; i < n * sizeof(double); i += sizeof(double))
//            {
//                var v = r.NextDouble();

//                mm.Write(i, v);

//                if (i / sizeof(double) % 1000000 == 0)
//                    Console.WriteLine($"Writing: {i / sizeof(double)}: {v}");

//            }
//            Console.Write("Flushing...");
//            mm.Flush();
//            Console.WriteLine("Done");
//        }
//    }

//}

//public void ReadArrayExample(string dataPath)
//{
//    const long n = 1_000_000_000L;

//    using (var mmfile = MemoryMappedFile.CreateFromFile(
//        @"C:\Users\todd.prater\Desktop\mmtest.data", FileMode.Open))
//    {
//        using (var mm = mmfile.CreateViewAccessor())
//        {
//            var bufferSize = 1_000_000;
//            var buffer = new double[bufferSize];

//            for (var i = 0L; i < n * sizeof(double); i += bufferSize * sizeof(double))
//            {
//                var count = mm.ReadArray(i, buffer, 0, bufferSize);
//                Console.WriteLine($"Reading block: {i / bufferSize / sizeof(double)}: {count}");
//            }

//            mm.Flush();
//        }
//    }

//}


