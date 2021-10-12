using Mountain.World.Biome.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mountain.World.Biome
{
    public static class Biomes
    {
        private static readonly IBiomeType<IBiome>[] BiomeTypes;

        static Biomes()
        {
            BiomeTypes = typeof(Biomes).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (IBiomeType<IBiome>)f.GetValue(null)).ToArray();
        }

        public static readonly IBiomeType<BiomeOcean> Ocean = new BiomeType<BiomeOcean>(0);
        public static readonly IBiomeType<BiomeDeepOcean> DeepOcean = new BiomeType<BiomeDeepOcean>(24);
        public static readonly IBiomeType<BiomeFrozenOcean> FrozenOcean = new BiomeType<BiomeFrozenOcean>(10);
        public static readonly IBiomeType<BiomeDeepFrozenOcean> DeepFrozenOcean = new BiomeType<BiomeDeepFrozenOcean>(50);
        public static readonly IBiomeType<BiomeColdOcean> ColdOcean = new BiomeType<BiomeColdOcean>(46);
        public static readonly IBiomeType<BiomeDeepColdOcean> DeepColdOcean = new BiomeType<BiomeDeepColdOcean>(49);
        public static readonly IBiomeType<BiomeLukewarmOcean> LukewarmOcean = new BiomeType<BiomeLukewarmOcean>(45);
        public static readonly IBiomeType<BiomeDeepLukewarmOcean> DeepLukewarmOcean = new BiomeType<BiomeDeepLukewarmOcean>(48);
        public static readonly IBiomeType<BiomeWarmOcean> WarmOcean = new BiomeType<BiomeWarmOcean>(44);
        public static readonly IBiomeType<BiomeDeepWarmOcean> DeepWarmOcean = new BiomeType<BiomeDeepWarmOcean>(47);

        public static readonly IBiomeType<BiomeOcean> River = new BiomeType<BiomeOcean>(7);
        public static readonly IBiomeType<BiomeOcean> FrozenRiver = new BiomeType<BiomeOcean>(11);
        public static readonly IBiomeType<BiomeOcean> Beach = new BiomeType<BiomeOcean>(16);
        public static readonly IBiomeType<BiomeOcean> StonyShore = new BiomeType<BiomeOcean>(25);
        public static readonly IBiomeType<BiomeOcean> SnowyBeach = new BiomeType<BiomeOcean>(26);

        public static readonly IBiomeType<BiomeOcean> Forest = new BiomeType<BiomeOcean>(4);
        public static readonly IBiomeType<BiomeOcean> WoodenHills = new BiomeType<BiomeOcean>(18);
        public static readonly IBiomeType<BiomeOcean> FlowerForest = new BiomeType<BiomeOcean>(132);
        public static readonly IBiomeType<BiomeOcean> BirchForest = new BiomeType<BiomeOcean>(27);
        public static readonly IBiomeType<BiomeOcean> BirchForestHills = new BiomeType<BiomeOcean>(28);
        public static readonly IBiomeType<BiomeOcean> TellBirchForest = new BiomeType<BiomeOcean>(155);
        public static readonly IBiomeType<BiomeOcean> TallBirchHills = new BiomeType<BiomeOcean>(156);
        public static readonly IBiomeType<BiomeOcean> DarkForest = new BiomeType<BiomeOcean>(29);
        public static readonly IBiomeType<BiomeOcean> DarkForestHills = new BiomeType<BiomeOcean>(157);

        public static readonly IBiomeType<BiomeOcean> Jungle = new BiomeType<BiomeOcean>(21);
        public static readonly IBiomeType<BiomeOcean> JungleHills = new BiomeType<BiomeOcean>(22);
        public static readonly IBiomeType<BiomeOcean> ModifiedJungle = new BiomeType<BiomeOcean>(149);
        public static readonly IBiomeType<BiomeOcean> JungleEdge = new BiomeType<BiomeOcean>(23);
        public static readonly IBiomeType<BiomeOcean> ModifiedJungleEdge = new BiomeType<BiomeOcean>(151);
        public static readonly IBiomeType<BiomeOcean> BambooJungle = new BiomeType<BiomeOcean>(168);
        public static readonly IBiomeType<BiomeOcean> BambooJungleHills = new BiomeType<BiomeOcean>(169);

        public static readonly IBiomeType<BiomeOcean> Taiga = new BiomeType<BiomeOcean>(5);
        public static readonly IBiomeType<BiomeOcean> TaigaHills = new BiomeType<BiomeOcean>(19);
        public static readonly IBiomeType<BiomeOcean> TaigaMountains = new BiomeType<BiomeOcean>(133);
        public static readonly IBiomeType<BiomeOcean> SnowyTaiga = new BiomeType<BiomeOcean>(30);
        public static readonly IBiomeType<BiomeOcean> SnowyTaigaHills = new BiomeType<BiomeOcean>(31);
        public static readonly IBiomeType<BiomeOcean> SnowyTaigaMountains = new BiomeType<BiomeOcean>(158);
        public static readonly IBiomeType<BiomeOcean> GiantTreeTaiga = new BiomeType<BiomeOcean>(32);
        public static readonly IBiomeType<BiomeOcean> GiantTreeTaigaHills = new BiomeType<BiomeOcean>(33);
        public static readonly IBiomeType<BiomeOcean> GiantSpruceTaiga = new BiomeType<BiomeOcean>(160);
        public static readonly IBiomeType<BiomeOcean> GiantSpruceTaigaHills = new BiomeType<BiomeOcean>(161);

        public static readonly IBiomeType<BiomeOcean> MushroomFields = new BiomeType<BiomeOcean>(14);
        public static readonly IBiomeType<BiomeOcean> MushroomFieldShore = new BiomeType<BiomeOcean>(15);
        public static readonly IBiomeType<BiomeOcean> Swamp = new BiomeType<BiomeOcean>(6);
        public static readonly IBiomeType<BiomeOcean> SwampHills = new BiomeType<BiomeOcean>(134);

        public static readonly IBiomeType<BiomeOcean> Savanna = new BiomeType<BiomeOcean>(35);
        public static readonly IBiomeType<BiomeOcean> SavannaPlateau = new BiomeType<BiomeOcean>(36);
        public static readonly IBiomeType<BiomeOcean> ShatteredSavanna = new BiomeType<BiomeOcean>(163);
        public static readonly IBiomeType<BiomeOcean> ShatteredSavannaPlateau = new BiomeType<BiomeOcean>(164);

        public static readonly IBiomeType<BiomePlains> Plains = new BiomeType<BiomePlains>(1);
        public static readonly IBiomeType<BiomeOcean> SunflowerPlains = new BiomeType<BiomeOcean>(129);

        public static readonly IBiomeType<BiomeOcean> Desert = new BiomeType<BiomeOcean>(2);
        public static readonly IBiomeType<BiomeOcean> DesertHills = new BiomeType<BiomeOcean>(17);
        public static readonly IBiomeType<BiomeOcean> DesertLakes = new BiomeType<BiomeOcean>(130);

        public static readonly IBiomeType<BiomeOcean> SnowyTundra = new BiomeType<BiomeOcean>(12);
        public static readonly IBiomeType<BiomeOcean> SnowyMountains = new BiomeType<BiomeOcean>(13);
        public static readonly IBiomeType<BiomeOcean> IceSpikes = new BiomeType<BiomeOcean>(140);

        public static readonly IBiomeType<BiomeOcean> Mountains = new BiomeType<BiomeOcean>(3);
        public static readonly IBiomeType<BiomeOcean> WoodedMountains = new BiomeType<BiomeOcean>(34);
        public static readonly IBiomeType<BiomeOcean> GravellyMountains = new BiomeType<BiomeOcean>(131);
        public static readonly IBiomeType<BiomeOcean> ModifiedGravellyMountains = new BiomeType<BiomeOcean>(162);
        public static readonly IBiomeType<BiomeOcean> MountainEdge = new BiomeType<BiomeOcean>(20);

        public static readonly IBiomeType<BiomeOcean> Badlands = new BiomeType<BiomeOcean>(37);
        public static readonly IBiomeType<BiomeOcean> BadlandsPlateau = new BiomeType<BiomeOcean>(39);
        public static readonly IBiomeType<BiomeOcean> ModifiedBadlandsPlateau = new BiomeType<BiomeOcean>(167);
        public static readonly IBiomeType<BiomeOcean> WoodedBadlandsPlateau = new BiomeType<BiomeOcean>(38);
        public static readonly IBiomeType<BiomeOcean> ModifiedWoodedBadlandsPlateau = new BiomeType<BiomeOcean>(166);
        public static readonly IBiomeType<BiomeOcean> ErodedBadlands = new BiomeType<BiomeOcean>(165);

        public static readonly IBiomeType<BiomeOcean> DripstoneCaves = new BiomeType<BiomeOcean>(174);
        public static readonly IBiomeType<BiomeOcean> LushCaves = new BiomeType<BiomeOcean>(175);

        public static readonly IBiomeType<BiomeOcean> NetherWastes = new BiomeType<BiomeOcean>(8);
        public static readonly IBiomeType<BiomeOcean> CrimsonForest = new BiomeType<BiomeOcean>(171);
        public static readonly IBiomeType<BiomeOcean> WarpedForest = new BiomeType<BiomeOcean>(172);
        public static readonly IBiomeType<BiomeOcean> SoulSandValley = new BiomeType<BiomeOcean>(170);
        public static readonly IBiomeType<BiomeOcean> BasaltDeltas = new BiomeType<BiomeOcean>(173);

        public static readonly IBiomeType<BiomeOcean> TheEnd = new BiomeType<BiomeOcean>(9);
        public static readonly IBiomeType<BiomeOcean> SmallEndIslands = new BiomeType<BiomeOcean>(40);
        public static readonly IBiomeType<BiomeOcean> EndMidlands = new BiomeType<BiomeOcean>(41);
        public static readonly IBiomeType<BiomeOcean> EndHighlands = new BiomeType<BiomeOcean>(42);
        public static readonly IBiomeType<BiomeOcean> EndBarrens = new BiomeType<BiomeOcean>(43);

        public static readonly IBiomeType<BiomeVoid> Void = new BiomeType<BiomeVoid>(127);

    }

    public interface IBiomeType<out T> where T : IBiome
    {
        public T GetBase();
    }

    public class BiomeType<T> : IBiomeType<T> where T : IBiome, new()
    {
        public int BiomeId { get; }

        public BiomeType(int biomeId)
        {
            BiomeId = biomeId;
        }

        public T GetBase() => new T();
    }
}
