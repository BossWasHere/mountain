using Mountain.Core.Block.Property;
using Mountain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block
{
    public abstract class BlockStateBase
    {
        public BlockMaterial Material { get; }

        protected BlockStateBase(BlockMaterial material)
        {
            Material = material;
        }

        public static BlockStateBase FromInt(int id)
        {
            //TODO
            return null;
        }

        public abstract short GetId();
    }

    public class BlockState : BlockStateBase
    {
        public BlockState(BlockMaterial material) : base(material)
        { }

        public BlockState<T> SetBlockData<T>(T blockData) where T : IBlockProperty
        {
            return new BlockState<T>(Material, blockData);
        }

        public override short GetId()
        {
            // TODO lookup ids
            return 0;
        }
    }

    public class BlockState<T> : BlockStateBase where T : IBlockProperty
    {
        public T BlockData { get; private set; }

        public BlockState(BlockMaterial material) : this(material, default)
        { }

        public BlockState(BlockMaterial material, T blockData) : base(material)
        {
            SetBlockData(blockData);
        }

        public void SetBlockData(T blockData)
        {
            if (Material.BlockDataClass != typeof(T))
            {
                throw new BlockStateException("Block data class " + typeof(T).Name + " not supported by " + Material.Name);
            }
            BlockData = blockData;
        }

        public override short GetId()
        {
            // TODO lookup ids
            return (short)BlockData.GetStateNumber();
        }
    }
}
