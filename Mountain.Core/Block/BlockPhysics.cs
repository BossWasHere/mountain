using Mountain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block
{
    public class BlockPhysics
    {
        public static readonly BlockPhysics Air = new BlockPhysics(-1.0f, -1.0f, false, false, false, false, ToolType.None, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Stone = new BlockPhysics(1.5f, 6.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Wood);
        public static readonly BlockPhysics Cobblestone = new BlockPhysics(2.0f, 6.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Wood);
        public static readonly BlockPhysics Dirt = new BlockPhysics(0.5f, 0.5f, false, false, true, true, ToolType.Shovel, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics GrassBlock = new BlockPhysics(0.6f, 0.6f, false, false, true, true, ToolType.Shovel, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Sand = new BlockPhysics(0.5f, 0.5f, true, false, true, true, ToolType.Shovel, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Gravel = new BlockPhysics(0.6f, 0.6f, true, false, true, true, ToolType.Shovel, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Log = new BlockPhysics(2.0f, 2.0f, false, true, true, true, ToolType.Axe, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics NetherStem = new BlockPhysics(2.0f, 2.0f, false, false, true, true, ToolType.Axe, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Plank = new BlockPhysics(2.0f, 3.0f, false, true, true, true, ToolType.Axe, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics NetherPlank = new BlockPhysics(2.0f, 3.0f, false, false, true, true, ToolType.Axe, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics NoCollision = new BlockPhysics(0.0f, 0.0f, false, false, false, false, ToolType.None, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Unbreakable = new BlockPhysics(-1.0f, 3600000.0f, false, false, true, true, ToolType.None, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Water = new BlockPhysics(-1.0f, 100.0f, false, false, false, true, ToolType.None, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics Lava = new BlockPhysics(-1.0f, 100.0f, false, false, false, true, ToolType.None, ToolType.None, ToolTier.None);
        public static readonly BlockPhysics WoodLevelOre = new BlockPhysics(3.0f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Wood);
        public static readonly BlockPhysics WoodLevelDeepslateOre = new BlockPhysics(4.5f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Wood);
        public static readonly BlockPhysics WoodLevelNetherOre = new BlockPhysics(3.0f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Wood);
        public static readonly BlockPhysics StoneLevelOre = new BlockPhysics(3.0f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Stone);
        public static readonly BlockPhysics StoneLevelDeepslateOre = new BlockPhysics(4.5f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Stone);
        public static readonly BlockPhysics IronLevelOre = new BlockPhysics(3.0f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Iron);
        public static readonly BlockPhysics IronLevelDeepslateOre = new BlockPhysics(4.5f, 3.0f, false, false, true, true, ToolType.Pickaxe, ToolType.Pickaxe, ToolTier.Iron);


        public float Hardness { get; }
        public float BlastResistance { get; }
        public bool HasGravity { get; }
        public bool IsFlammable { get; }
        public bool IsSolid { get; }
        public bool HasCollision { get; }
        public ToolType UsefulToolType { get; }
        public ToolType RequiredToolType { get; }
        public ToolTier RequiredToolTier { get; }

        private BlockPhysics(float hardness, float blastResistance, bool hasGravity, bool isFlammable, bool isSolid, bool hasCollision, ToolType usefulToolType, ToolType requiredToolType, ToolTier requiredToolTier)
        {
            Hardness = hardness;
            BlastResistance = blastResistance;
            UsefulToolType = usefulToolType;
            HasGravity = hasGravity;
            IsFlammable = isFlammable;
            IsSolid = isSolid;
            HasCollision = hasCollision;
            RequiredToolType = requiredToolType;
            RequiredToolTier = requiredToolTier;
        }
    }
}
