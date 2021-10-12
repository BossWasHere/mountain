using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.DataGenerator
{
    [Generator]
    public class BlockGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var sourceBuilder = new StringBuilder(@"
using System;
namespace MountainServer
{
    public static class BlockData
    {
        public const long BuildTime = ");

            sourceBuilder.Append(DateTimeOffset.Now.ToUnixTimeMilliseconds()).Append(';');

            sourceBuilder.Append(@"        
    }
}
");
            context.AddSource("blockGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            
        }
    }
}
