using Mountain.Core.Block.Property;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block
{
    public class BlockMaterial : IMaterial
    {
        public Namespace Namespace { get; }
        public string Key => Name;

        internal BlockMaterial(string name) : this(name, BlockPhysics.Stone)
        { }

        internal BlockMaterial(string name, BlockPhysics physics) : this(name, physics, null)
        { }

        internal BlockMaterial(string name, BlockPhysics physics, Type blockDataClass) : this(name, physics, true, blockDataClass)
        { }

        internal BlockMaterial(string name, BlockPhysics physics, bool itemObtainable, Type blockDataClass)
        {
            Namespace = Namespace.Minecraft;
            Name = name;
            PhysicalProperties = physics;
            IsItem = itemObtainable;
            BlockDataClass = blockDataClass;
        }

        //public IBlockProperty[] GetBlockDataStates()
        //{
        //    if (BlockDataClass is IBlockProperty)
        //    {
        //        var method = BlockDataClass.GetMethod("GetStates");
        //        return (IBlockProperty[])method.Invoke(null, null);
        //    }
        //    return null;
        //}

        public string Name { get; }

        public BlockPhysics PhysicalProperties { get; }

        public Type BlockDataClass { get; }

        public bool IsBlock => true;

        public bool IsItem { get; }

        public bool IsSolid => PhysicalProperties.IsSolid;

        public bool IsEdible => false;

        public bool IsRecord => false;

        public bool IsAir => PhysicalProperties.Equals(BlockPhysics.Air);
    }
}
