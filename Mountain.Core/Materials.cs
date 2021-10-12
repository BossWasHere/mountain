using Mountain.Core.Block;
using Mountain.Core.Block.Property;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mountain.Core
{
    public static class Materials
    {

        //private static readonly Dictionary<int, BlockMaterial> BlockRegistry = new Dictionary<int, BlockMaterial>();

        public static readonly BlockMaterial Air = new BlockMaterial("air", BlockPhysics.Air);
        public static readonly BlockMaterial Stone = new BlockMaterial("stone");
        public static readonly BlockMaterial Granite = new BlockMaterial("granite");
        public static readonly BlockMaterial PolishedGranite = new BlockMaterial("polished_granite");
        public static readonly BlockMaterial Diorite = new BlockMaterial("diorite");
        public static readonly BlockMaterial PolishedDiorite = new BlockMaterial("polished_diorite");
        public static readonly BlockMaterial Andesite = new BlockMaterial("andesite");
        public static readonly BlockMaterial PolishedAndesite = new BlockMaterial("polished_andesite");
        public static readonly BlockMaterial GrassBlock = new BlockMaterial("grass_block", BlockPhysics.GrassBlock, typeof(Snowable));
        public static readonly BlockMaterial Dirt = new BlockMaterial("dirt", BlockPhysics.Dirt);
        public static readonly BlockMaterial CoarseDirt = new BlockMaterial("coarse_dirt", BlockPhysics.Dirt);
        public static readonly BlockMaterial Podzol = new BlockMaterial("podzol", BlockPhysics.Dirt, typeof(Snowable));
        public static readonly BlockMaterial Cobblestone = new BlockMaterial("cobblestone", BlockPhysics.Cobblestone);
        public static readonly BlockMaterial OakPlanks = new BlockMaterial("oak_planks", BlockPhysics.Plank);
        public static readonly BlockMaterial SprucePlanks = new BlockMaterial("spruce_planks", BlockPhysics.Plank);
        public static readonly BlockMaterial BirchPlanks = new BlockMaterial("birch_planks", BlockPhysics.Plank);
        public static readonly BlockMaterial JunglePlanks = new BlockMaterial("jungle_planks", BlockPhysics.Plank);
        public static readonly BlockMaterial AcaciaPlanks = new BlockMaterial("acacia_planks", BlockPhysics.Plank);
        public static readonly BlockMaterial DarkOakPlanks = new BlockMaterial("dark_oak_planks", BlockPhysics.Plank);
        public static readonly BlockMaterial OakSapling = new BlockMaterial("oak_sapling", BlockPhysics.NoCollision, typeof(Sapling));
        public static readonly BlockMaterial SpruceSapling = new BlockMaterial("spruce_sapling", BlockPhysics.NoCollision, typeof(Sapling));
        public static readonly BlockMaterial BirchSapling = new BlockMaterial("birch_sapling", BlockPhysics.NoCollision, typeof(Sapling));
        public static readonly BlockMaterial JungleSapling = new BlockMaterial("jungle_sapling", BlockPhysics.NoCollision, typeof(Sapling));
        public static readonly BlockMaterial AcaciaSapling = new BlockMaterial("acacia_sapling", BlockPhysics.NoCollision, typeof(Sapling));
        public static readonly BlockMaterial DarkOakSapling = new BlockMaterial("dark_oak_sapling", BlockPhysics.NoCollision, typeof(Sapling));
        public static readonly BlockMaterial Bedrock = new BlockMaterial("bedrock", BlockPhysics.Unbreakable);
        public static readonly BlockMaterial Water = new BlockMaterial("water", BlockPhysics.Water, typeof(Liquid));
        public static readonly BlockMaterial Lava = new BlockMaterial("lava", BlockPhysics.Lava, typeof(Liquid));
        public static readonly BlockMaterial Sand = new BlockMaterial("sand", BlockPhysics.Sand);
        public static readonly BlockMaterial RedSand = new BlockMaterial("red_sand", BlockPhysics.Sand);
        public static readonly BlockMaterial GoldOre = new BlockMaterial("gold_ore", BlockPhysics.IronLevelOre);
        public static readonly BlockMaterial DeepslateGoldOre = new BlockMaterial("deepslate_gold_ore", BlockPhysics.IronLevelDeepslateOre);
        public static readonly BlockMaterial IronOre = new BlockMaterial("iron_ore", BlockPhysics.StoneLevelOre);
        public static readonly BlockMaterial DeepslateIronOre = new BlockMaterial("deepslate_iron_ore", BlockPhysics.StoneLevelDeepslateOre);
        public static readonly BlockMaterial CoalOre = new BlockMaterial("coal_ore", BlockPhysics.WoodLevelOre);
        public static readonly BlockMaterial DeepslateCoalOre = new BlockMaterial("deepslate_coal_ore", BlockPhysics.WoodLevelDeepslateOre);
        public static readonly BlockMaterial NetherGoldOre = new BlockMaterial("nether_gold_ore", BlockPhysics.WoodLevelNetherOre);
        public static readonly BlockMaterial OakLog = new BlockMaterial("oak_log", BlockPhysics.Log, typeof(AxisAligned));
        public static readonly BlockMaterial SpruceLog = new BlockMaterial("oak_log", BlockPhysics.Log, typeof(AxisAligned));
        public static readonly BlockMaterial BirchLog = new BlockMaterial("oak_log", BlockPhysics.Log, typeof(AxisAligned));
        public static readonly BlockMaterial JungleLog = new BlockMaterial("oak_log", BlockPhysics.Log, typeof(AxisAligned));
        public static readonly BlockMaterial AcaciaLog = new BlockMaterial("oak_log", BlockPhysics.Log, typeof(AxisAligned));
        public static readonly BlockMaterial DarkOakLog = new BlockMaterial("oak_log", BlockPhysics.Log, typeof(AxisAligned));

        //static Materials()
        //{
        //    int blockId = 0;
        //    FieldInfo[] fields = typeof(Materials).GetFields();
        //    foreach (FieldInfo fi in fields)
        //    {
        //        if (fi.GetValue(null) is BlockMaterial bm)
        //        {
        //            for (int i = 0; i < (bm.GetBlockDataStates()?.Length ?? 1); i++)
        //            {
        //                BlockRegistry.Add(blockId++, bm);
        //            }
        //        }
        //    }
        //}
    }
}
