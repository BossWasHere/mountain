using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core
{
    public class Namespace
    {
        public static readonly Namespace Minecraft = new Namespace("minecraft");

        private static readonly Dictionary<string, Namespace> NAMESPACES;

        static Namespace() {
            NAMESPACES = new Dictionary<string, Namespace>
            {
                { Minecraft.Id, Minecraft }
            };
        }
        
        public static Namespace GetDefaultNamespace()
        {
            return Minecraft;
        }

        public static Namespace GetFromId(string namespaceId)
        {
            return NAMESPACES[namespaceId];
        }

        public static Namespace RegisterNamespace(string namespaceId)
        {
            if (NAMESPACES.ContainsKey(namespaceId)) throw new InvalidOperationException("A namespace with the ID " + namespaceId + " already exists");
            var ns = new Namespace(namespaceId);
            NAMESPACES.Add(namespaceId, ns);
            return ns;
        }

        public readonly string Id;

        private Namespace(string id)
        {
            Id = id;
        }
    }
}
