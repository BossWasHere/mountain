using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World.Generator
{
    public interface IChunkGeneratorProvider
    {
        public string GeneratorName { get; }
        public IChunkGenerator NewInstance();
    }

    //public interface IChunkGeneratorProvider<T>
    //{
    //    public T NewInstance();
    //}
}
