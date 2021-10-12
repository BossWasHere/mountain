using Mountain.World.Generator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World.Env
{
    public static class AssemblyData
    {
        public static IEnumerable<KeyValuePair<string, IChunkGeneratorProvider>> GetBuiltinGeneratorProviders()
        {
            var flatGenerator = new FlatGeneratorProvider();
            yield return new KeyValuePair<string, IChunkGeneratorProvider>(flatGenerator.GeneratorName, flatGenerator);
        }
    }
}
