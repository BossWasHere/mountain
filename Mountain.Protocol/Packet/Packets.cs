using Mountain.Core;
using Mountain.Protocol.Packet.In;
using Mountain.Protocol.Packet.Out;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mountain.Protocol.Packet
{
    /// <summary>
    /// Holds every type of known clientbound and serverbound packet. Based on the fantastic work of wiki.vg contributors.<br/>
    /// Release Protocol: <a href="https://wiki.vg/Protocol">https://wiki.vg/Protocol</a><br/>
    /// Pre-release Protocol: <a href="https://wiki.vg/Pre-release_protocol">https://wiki.vg/Pre-release_protocol</a>
    /// </summary>
    public static class Packets
    {
        public const int ProtocolVersion = CompileData.ProtocolVersion;

        private static readonly IPacketType<IInboundPacket>[][] InboundTypes = new IPacketType<IInboundPacket>[4][];
        private static readonly IPacketType<IOutboundPacket>[][] OutboundTypes = new IPacketType<IOutboundPacket>[4][];

        static Packets()
        {
            InboundTypes[(int)ConnectionState.Handshaking] = new IPacketType<IInboundPacket>[] { In.Handshaking.SetProtocol };
            InboundTypes[(int)ConnectionState.Status] = new IPacketType<IInboundPacket>[] { In.Status.Request, In.Status.Ping };
            InboundTypes[(int)ConnectionState.Login] = new IPacketType<IInboundPacket>[] { In.Login.LoginStart, In.Login.EncryptionResponse, In.Login.LoginPluginResponse };
            InboundTypes[(int)ConnectionState.Play] = typeof(In.Play).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (IPacketType<IInboundPacket>)f.GetValue(null)).ToArray();

            OutboundTypes[(int)ConnectionState.Status] = new IPacketType<IOutboundPacket>[] { Out.Status.StatusResponse, Out.Status.Pong };
            OutboundTypes[(int)ConnectionState.Login] = new IPacketType<IOutboundPacket>[] { Out.Login.Disconnect, Out.Login.EncryptionRequest, Out.Login.LoginSuccess, Out.Login.SetCompression, Out.Login.LoginPluginRequest };
            OutboundTypes[(int)ConnectionState.Play] = typeof(Out.Play).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (IPacketType<IOutboundPacket>)f.GetValue(null)).ToArray();
        }

        public static IPacketType<IInboundPacket> GetInboundType(byte packetId, ConnectionState state = ConnectionState.Play)
        {
            if (packetId == 0xFE && (state == ConnectionState.Handshaking || state == ConnectionState.Status)) return In.Handshaking.LegacyPing;
            var subtypes = InboundTypes[(int)state];
            return subtypes.Length > packetId ? subtypes[packetId] : null;
        }

        public static IPacketType<IOutboundPacket> GetOutboundType(byte packetId, ConnectionState state = ConnectionState.Play)
        {
            if (packetId == 0xFF && (state == ConnectionState.Handshaking || state == ConnectionState.Status)) return Out.LegacyKick;
            var subtypes = OutboundTypes[(int)state];
            return subtypes.Length > packetId ? subtypes[packetId] : null;
        }

        public readonly struct In
        {
            public readonly struct Handshaking
            {
                public static readonly IInboundPacketType<PacketHandshakingInSetProtocol> SetProtocol = new InboundPacketType<PacketHandshakingInSetProtocol>(0x00);
                public static readonly IInboundPacketType<PacketLegacyPing> LegacyPing = new InboundPacketType<PacketLegacyPing>(0xFE);
            }
            public readonly struct Status
            {
                // Cheat to not store socket when not in login/play phase
                public static readonly IInboundPacketType<PacketHandshakingInSetProtocol> Request = Handshaking.SetProtocol;
                public static readonly IInboundPacketType<PacketStatusInPing> Ping = new InboundPacketType<PacketStatusInPing>(0x01);

            }
            public readonly struct Login
            {
                public static readonly IInboundPacketType<PacketLoginInLoginStart> LoginStart = new InboundPacketType<PacketLoginInLoginStart>(0x00);
                public static readonly IInboundPacketType<PacketLoginInEncryptionResponse> EncryptionResponse = new InboundPacketType<PacketLoginInEncryptionResponse>(0x01);
                public static readonly IInboundPacketType<PacketLoginInLoginPluginResponse> LoginPluginResponse = new InboundPacketType<PacketLoginInLoginPluginResponse>(0x02);

            }
            public readonly struct Play
            {
                public static readonly IInboundPacketType<PacketPlayInTeleportConfirm> TeleportConfirm = new InboundPacketType<PacketPlayInTeleportConfirm>(0x00);
                public static readonly IInboundPacketType<PacketPlayInQueryBlockNBT> QueryBlockNBT = new InboundPacketType<PacketPlayInQueryBlockNBT>(0x01);
                public static readonly IInboundPacketType<PacketPlayInSetDifficulty> SetDifficulty = new InboundPacketType<PacketPlayInSetDifficulty>(0x02);
                public static readonly IInboundPacketType<PacketPlayInChatMessage> ChatMessage = new InboundPacketType<PacketPlayInChatMessage>(0x03);
                public static readonly IInboundPacketType<PacketPlayInClientStatus> ClientStatus = new InboundPacketType<PacketPlayInClientStatus>(0x04);
                public static readonly IInboundPacketType<PacketPlayInClientSettings> ClientSettings = new InboundPacketType<PacketPlayInClientSettings>(0x05);
                public static readonly IInboundPacketType<PacketPlayInTabComplete> TabComplete = new InboundPacketType<PacketPlayInTabComplete>(0x06);
                // Removed 1.17
                //public static readonly IInboundPacketType<PacketPlayInWindowConfirmation> WindowConfirmation = new InboundPacketType<PacketPlayInWindowConfirmation>(0x07);
                public static readonly IInboundPacketType<PacketPlayInClickWindowButton> ClickWindowButton = new InboundPacketType<PacketPlayInClickWindowButton>(0x07);
                public static readonly IInboundPacketType<PacketPlayInClickWindow> ClickWindow = new InboundPacketType<PacketPlayInClickWindow>(0x08);
                public static readonly IInboundPacketType<PacketPlayInCloseWindow> CloseWindow = new InboundPacketType<PacketPlayInCloseWindow>(0x09);
                public static readonly IInboundPacketType<PacketPlayInPluginMessage> PluginMessage = new InboundPacketType<PacketPlayInPluginMessage>(0x0A);
                public static readonly IInboundPacketType<PacketPlayInEditBook> EditBook = new InboundPacketType<PacketPlayInEditBook>(0x0B);
                public static readonly IInboundPacketType<PacketPlayInQueryEntityNBT> QueryEntityNBT = new InboundPacketType<PacketPlayInQueryEntityNBT>(0x0C);
                public static readonly IInboundPacketType<PacketPlayInInteractEntity> InteractEntity = new InboundPacketType<PacketPlayInInteractEntity>(0x0D);
                public static readonly IInboundPacketType<PacketPlayInGenerateStructure> GenerateStructure = new InboundPacketType<PacketPlayInGenerateStructure>(0x0E);
                public static readonly IInboundPacketType<PacketPlayInKeepAlive> KeepAlive = new InboundPacketType<PacketPlayInKeepAlive>(0x0F);

                public static readonly IInboundPacketType<PacketPlayInLockDifficulty> LockDifficulty = new InboundPacketType<PacketPlayInLockDifficulty>(0x10);
                public static readonly IInboundPacketType<PacketPlayInPlayerPosition> PlayerPosition = new InboundPacketType<PacketPlayInPlayerPosition>(0x11);
                public static readonly IInboundPacketType<PacketPlayInPlayerPositionRotation> PlayerPositionRotation = new InboundPacketType<PacketPlayInPlayerPositionRotation>(0x12);
                public static readonly IInboundPacketType<PacketPlayInPlayerRotation> PlayerRotation = new InboundPacketType<PacketPlayInPlayerRotation>(0x13);
                public static readonly IInboundPacketType<PacketPlayInPlayerMovement> PlayerMovement = new InboundPacketType<PacketPlayInPlayerMovement>(0x14);
                public static readonly IInboundPacketType<PacketPlayInVehicleMove> VehicleMove = new InboundPacketType<PacketPlayInVehicleMove>(0x15);
                public static readonly IInboundPacketType<PacketPlayInSteerBoat> SteerBoat = new InboundPacketType<PacketPlayInSteerBoat>(0x16);
                public static readonly IInboundPacketType<PacketPlayInPickItem> PickItem = new InboundPacketType<PacketPlayInPickItem>(0x17);
                public static readonly IInboundPacketType<PacketPlayInCraftRecipeRequest> CraftRecipeRequest = new InboundPacketType<PacketPlayInCraftRecipeRequest>(0x18);
                public static readonly IInboundPacketType<PacketPlayInPlayerAbilities> PlayerAbilities = new InboundPacketType<PacketPlayInPlayerAbilities>(0x19);
                public static readonly IInboundPacketType<PacketPlayInPlayerDigging> PlayerDigging = new InboundPacketType<PacketPlayInPlayerDigging>(0x1A);
                public static readonly IInboundPacketType<PacketPlayInEntityAction> EntityAction = new InboundPacketType<PacketPlayInEntityAction>(0x1B);
                public static readonly IInboundPacketType<PacketPlayInSteerVehicle> SteerVehicle = new InboundPacketType<PacketPlayInSteerVehicle>(0x1C);
                // New 1.17
                public static readonly IInboundPacketType<PacketPlayInPong> Pong = new InboundPacketType<PacketPlayInPong>(0x1D);
                public static readonly IInboundPacketType<PacketPlayInSetDisplayedRecipe> SetDisplayedRecipe = new InboundPacketType<PacketPlayInSetDisplayedRecipe>(0x1E);
                public static readonly IInboundPacketType<PacketPlayInSetRecipeBookState> SetRecipeBookState = new InboundPacketType<PacketPlayInSetRecipeBookState>(0x1F);

                public static readonly IInboundPacketType<PacketPlayInNameItem> NameItem = new InboundPacketType<PacketPlayInNameItem>(0x20);
                public static readonly IInboundPacketType<PacketPlayInResourcePackStatus> ResourcePackStatus = new InboundPacketType<PacketPlayInResourcePackStatus>(0x21);
                public static readonly IInboundPacketType<PacketPlayInAdvancementTab> AdvancementTab = new InboundPacketType<PacketPlayInAdvancementTab>(0x22);
                public static readonly IInboundPacketType<PacketPlayInSelectTrade> SelectTrade = new InboundPacketType<PacketPlayInSelectTrade>(0x23);
                public static readonly IInboundPacketType<PacketPlayInSetBeaconEffect> SetBeaconEffect = new InboundPacketType<PacketPlayInSetBeaconEffect>(0x24);
                public static readonly IInboundPacketType<PacketPlayInHeldItemChange> HeldItemChange = new InboundPacketType<PacketPlayInHeldItemChange>(0x25);
                public static readonly IInboundPacketType<PacketPlayInUpdateCommandBlock> UpdateCommandBlock = new InboundPacketType<PacketPlayInUpdateCommandBlock>(0x26);
                public static readonly IInboundPacketType<PacketPlayInUpdateCommandBlockMinecart> UpdateCommandBlockMinecart = new InboundPacketType<PacketPlayInUpdateCommandBlockMinecart>(0x27);
                public static readonly IInboundPacketType<PacketPlayInCreativeInventoryAction> CreativeInventoryAction = new InboundPacketType<PacketPlayInCreativeInventoryAction>(0x28);
                public static readonly IInboundPacketType<PacketPlayInUpdateJigsawBlock> UpdateJigsawBlock = new InboundPacketType<PacketPlayInUpdateJigsawBlock>(0x29);
                public static readonly IInboundPacketType<PacketPlayInUpdateStructureBlock> UpdateStructureBlock = new InboundPacketType<PacketPlayInUpdateStructureBlock>(0x2A);
                public static readonly IInboundPacketType<PacketPlayInUpdateSign> UpdateSign = new InboundPacketType<PacketPlayInUpdateSign>(0x2B);
                public static readonly IInboundPacketType<PacketPlayInAnimation> Animation = new InboundPacketType<PacketPlayInAnimation>(0x2C);
                public static readonly IInboundPacketType<PacketPlayInSpectate> Spectate = new InboundPacketType<PacketPlayInSpectate>(0x2D);
                public static readonly IInboundPacketType<PacketPlayInPlayerBlockPlacement> PlayerBlockPlacement = new InboundPacketType<PacketPlayInPlayerBlockPlacement>(0x2E);
                public static readonly IInboundPacketType<PacketPlayInUseItem> UseItem = new InboundPacketType<PacketPlayInUseItem>(0x2F);

            }
        }

        public readonly struct Out
        {
            public static readonly IOutboundPacketType<PacketLegacyKick> LegacyKick = new OutboundPacketType<PacketLegacyKick>(0xFF);
            public readonly struct Status
            {
                public static readonly IOutboundPacketType<PacketStatusOutServerResponse> StatusResponse = new OutboundPacketType<PacketStatusOutServerResponse>(0x00);
                public static readonly IOutboundPacketType<PacketStatusOutPong> Pong = new OutboundPacketType<PacketStatusOutPong>(0x01);

            }
            public readonly struct Login
            {
                public static readonly IOutboundPacketType<PacketLoginOutDisconnect> Disconnect = new OutboundPacketType<PacketLoginOutDisconnect>(0x00);
                public static readonly IOutboundPacketType<PacketLoginOutEncryptionRequest> EncryptionRequest = new OutboundPacketType<PacketLoginOutEncryptionRequest>(0x01);
                public static readonly IOutboundPacketType<PacketLoginOutLoginSuccess> LoginSuccess = new OutboundPacketType<PacketLoginOutLoginSuccess>(0x02);
                public static readonly IOutboundPacketType<PacketLoginOutSetCompression> SetCompression = new OutboundPacketType<PacketLoginOutSetCompression>(0x03);
                public static readonly IOutboundPacketType<PacketLoginOutLoginPluginRequest> LoginPluginRequest = new OutboundPacketType<PacketLoginOutLoginPluginRequest>(0x04);

            }
            public readonly struct Play
            {
                public static readonly IOutboundPacketType<PacketPlayOutSpawnEntity> SpawnEntity = new OutboundPacketType<PacketPlayOutSpawnEntity>(0x00);
                public static readonly IOutboundPacketType<PacketPlayOutSpawnExperienceOrb> SpawnExperienceOrb = new OutboundPacketType<PacketPlayOutSpawnExperienceOrb>(0x01);
                public static readonly IOutboundPacketType<PacketPlayOutSpawnLivingEntity> SpawnLivingEntity = new OutboundPacketType<PacketPlayOutSpawnLivingEntity>(0x02);
                public static readonly IOutboundPacketType<PacketPlayOutSpawnPainting> SpawnPainting = new OutboundPacketType<PacketPlayOutSpawnPainting>(0x03);
                public static readonly IOutboundPacketType<PacketPlayOutSpawnPlayer> SpawnPlayer = new OutboundPacketType<PacketPlayOutSpawnPlayer>(0x04);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutSkulkVibrationSignal> SkulkVibrationSignal = new OutboundPacketType<PacketPlayOutSkulkVibrationSignal>(0x05);
                public static readonly IOutboundPacketType<PacketPlayOutEntityAnimation> EntityAnimation = new OutboundPacketType<PacketPlayOutEntityAnimation>(0x06);
                public static readonly IOutboundPacketType<PacketPlayOutStatistics> Statistics = new OutboundPacketType<PacketPlayOutStatistics>(0x07);
                public static readonly IOutboundPacketType<PacketPlayOutAcknowlegdePlayerDigging> AcknowledgePlayerDigging = new OutboundPacketType<PacketPlayOutAcknowlegdePlayerDigging>(0x08);
                public static readonly IOutboundPacketType<PacketPlayOutBlockBreakAnimation> BlockBreakAnimation = new OutboundPacketType<PacketPlayOutBlockBreakAnimation>(0x09);
                public static readonly IOutboundPacketType<PacketPlayOutBlockEntityData> BlockEntityData = new OutboundPacketType<PacketPlayOutBlockEntityData>(0x0A);
                public static readonly IOutboundPacketType<PacketPlayOutBlockAction> BlockAction = new OutboundPacketType<PacketPlayOutBlockAction>(0x0B);
                public static readonly IOutboundPacketType<PacketPlayOutBlockChange> BlockChange = new OutboundPacketType<PacketPlayOutBlockChange>(0x0C);
                public static readonly IOutboundPacketType<PacketPlayOutBossBar> BossBar = new OutboundPacketType<PacketPlayOutBossBar>(0x0D);
                public static readonly IOutboundPacketType<PacketPlayOutServerDifficulty> ServerDifficulty = new OutboundPacketType<PacketPlayOutServerDifficulty>(0x0E);
                public static readonly IOutboundPacketType<PacketPlayOutChatMessage> ChatMessage = new OutboundPacketType<PacketPlayOutChatMessage>(0x0F);

                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutClearTitles> ClearTitles = new OutboundPacketType<PacketPlayOutClearTitles>(0x10);
                public static readonly IOutboundPacketType<PacketPlayOutTabComplete> TabComplete = new OutboundPacketType<PacketPlayOutTabComplete>(0x11);
                public static readonly IOutboundPacketType<PacketPlayOutDeclareCommands> DeclareCommands = new OutboundPacketType<PacketPlayOutDeclareCommands>(0x12);
                // Removed 1.17
                //public static readonly IOutboundPacketType<PacketPlayOutWindowConfirmation> WindowConfirmation = new OutboundPacketType<PacketPlayOutWindowConfirmation>(0x11);
                public static readonly IOutboundPacketType<PacketPlayOutCloseWindow> CloseWindow = new OutboundPacketType<PacketPlayOutCloseWindow>(0x13);
                public static readonly IOutboundPacketType<PacketPlayOutWindowItems> WindowItems = new OutboundPacketType<PacketPlayOutWindowItems>(0x14);
                public static readonly IOutboundPacketType<PacketPlayOutWindowProperty> WindowProperty = new OutboundPacketType<PacketPlayOutWindowProperty>(0x15);
                public static readonly IOutboundPacketType<PacketPlayOutSetSlot> SetSlot = new OutboundPacketType<PacketPlayOutSetSlot>(0x16);
                public static readonly IOutboundPacketType<PacketPlayOutSetCooldown> SetCooldown = new OutboundPacketType<PacketPlayOutSetCooldown>(0x17);
                public static readonly IOutboundPacketType<PacketPlayOutPluginMessage> PluginMessage = new OutboundPacketType<PacketPlayOutPluginMessage>(0x18);
                public static readonly IOutboundPacketType<PacketPlayOutNamedSoundEffect> NamedSoundEffect = new OutboundPacketType<PacketPlayOutNamedSoundEffect>(0x19);
                public static readonly IOutboundPacketType<PacketPlayOutDisconnect> Disconnect = new OutboundPacketType<PacketPlayOutDisconnect>(0x1A);
                public static readonly IOutboundPacketType<PacketPlayOutEntityStatus> EntityStatus = new OutboundPacketType<PacketPlayOutEntityStatus>(0x1B);
                public static readonly IOutboundPacketType<PacketPlayOutExplosion> Explosion = new OutboundPacketType<PacketPlayOutExplosion>(0x1C);
                public static readonly IOutboundPacketType<PacketPlayOutUnloadChunk> UnloadChunk = new OutboundPacketType<PacketPlayOutUnloadChunk>(0x1D);
                public static readonly IOutboundPacketType<PacketPlayOutChangeGameState> ChangeGameState = new OutboundPacketType<PacketPlayOutChangeGameState>(0x1E);
                public static readonly IOutboundPacketType<PacketPlayOutOpenHorseWindow> OpenHorseWindow = new OutboundPacketType<PacketPlayOutOpenHorseWindow>(0x1F);

                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutInitializeWorldBorder> InitializeWorldBorder = new OutboundPacketType<PacketPlayOutInitializeWorldBorder>(0x20);
                public static readonly IOutboundPacketType<PacketPlayOutKeepAlive> KeepAlive = new OutboundPacketType<PacketPlayOutKeepAlive>(0x21);
                public static readonly IOutboundPacketType<PacketPlayOutChunkData> ChunkData = new OutboundPacketType<PacketPlayOutChunkData>(0x22);
                public static readonly IOutboundPacketType<PacketPlayOutEffect> Effect = new OutboundPacketType<PacketPlayOutEffect>(0x23);
                public static readonly IOutboundPacketType<PacketPlayOutParticle> Particle = new OutboundPacketType<PacketPlayOutParticle>(0x24);
                public static readonly IOutboundPacketType<PacketPlayOutUpdateLight> UpdateLight = new OutboundPacketType<PacketPlayOutUpdateLight>(0x25);
                public static readonly IOutboundPacketType<PacketPlayOutJoinGame> JoinGame = new OutboundPacketType<PacketPlayOutJoinGame>(0x26);
                public static readonly IOutboundPacketType<PacketPlayOutMapData> MapData = new OutboundPacketType<PacketPlayOutMapData>(0x27);
                public static readonly IOutboundPacketType<PacketPlayOutTradeList> TradeList = new OutboundPacketType<PacketPlayOutTradeList>(0x28);
                public static readonly IOutboundPacketType<PacketPlayOutEntityPosition> EntityPosition = new OutboundPacketType<PacketPlayOutEntityPosition>(0x29);
                public static readonly IOutboundPacketType<PacketPlayOutEntityPositionRotation> EntityPositionRotation = new OutboundPacketType<PacketPlayOutEntityPositionRotation>(0x2A);
                public static readonly IOutboundPacketType<PacketPlayOutEntityRotation> EntityRotation = new OutboundPacketType<PacketPlayOutEntityRotation>(0x2B);
                // Removed 1.17
                //public static readonly IOutboundPacketType<PacketPlayOutEntityMovement> EntityMovement = new OutboundPacketType<PacketPlayOutEntityMovement>(0x2A);
                public static readonly IOutboundPacketType<PacketPlayOutVehicleMove> VehicleMove = new OutboundPacketType<PacketPlayOutVehicleMove>(0x2C);
                public static readonly IOutboundPacketType<PacketPlayOutOpenBook> OpenBook = new OutboundPacketType<PacketPlayOutOpenBook>(0x2D);
                public static readonly IOutboundPacketType<PacketPlayOutOpenWindow> OpenWindow = new OutboundPacketType<PacketPlayOutOpenWindow>(0x2E);
                public static readonly IOutboundPacketType<PacketPlayOutOpenSignEditor> OpenSignEditor = new OutboundPacketType<PacketPlayOutOpenSignEditor>(0x2F);

                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutPing> Ping = new OutboundPacketType<PacketPlayOutPing>(0x30);
                public static readonly IOutboundPacketType<PacketPlayOutCraftRecipeResponse> CraftRecipeResponse = new OutboundPacketType<PacketPlayOutCraftRecipeResponse>(0x31);
                public static readonly IOutboundPacketType<PacketPlayOutPlayerAbilities> PlayerAbilities = new OutboundPacketType<PacketPlayOutPlayerAbilities>(0x32);
                // Removed 1.17
                //public static readonly IOutboundPacketType<PacketPlayOutCombatEvent> CombatEvent = new OutboundPacketType<PacketPlayOutCombatEvent>(0x31);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutEndCombatEvent> EndCombatEvent = new OutboundPacketType<PacketPlayOutEndCombatEvent>(0x33);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutEnterCombatEvent> EnterCombatEvent = new OutboundPacketType<PacketPlayOutEnterCombatEvent>(0x34);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutDeathCombatEvent> DeathCombatEvent = new OutboundPacketType<PacketPlayOutDeathCombatEvent>(0x35);
                public static readonly IOutboundPacketType<PacketPlayOutPlayerInfo> PlayerInfo = new OutboundPacketType<PacketPlayOutPlayerInfo>(0x36);
                public static readonly IOutboundPacketType<PacketPlayOutFacePlayer> FacePlayer = new OutboundPacketType<PacketPlayOutFacePlayer>(0x37);
                public static readonly IOutboundPacketType<PacketPlayOutPlayerPositionLook> PlayerPositionLook = new OutboundPacketType<PacketPlayOutPlayerPositionLook>(0x38);
                public static readonly IOutboundPacketType<PacketPlayOutUnlockRecipes> UnlockRecipes = new OutboundPacketType<PacketPlayOutUnlockRecipes>(0x39);
                // Removed 1.17
                //public static readonly IOutboundPacketType<PacketPlayOutDestroyEntities> DestroyEntities = new OutboundPacketType<PacketPlayOutDestroyEntities>(0x36);
                public static readonly IOutboundPacketType<PacketPlayOutDestroyEntity> DestroyEntity = new OutboundPacketType<PacketPlayOutDestroyEntity>(0x3A);
                public static readonly IOutboundPacketType<PacketPlayOutRemoveEntityEffect> RemoveEntityEffect = new OutboundPacketType<PacketPlayOutRemoveEntityEffect>(0x3B);
                public static readonly IOutboundPacketType<PacketPlayOutResourcePackSend> ResourcePackSend = new OutboundPacketType<PacketPlayOutResourcePackSend>(0x3C);
                public static readonly IOutboundPacketType<PacketPlayOutRespawn> Respawn = new OutboundPacketType<PacketPlayOutRespawn>(0x3D);
                public static readonly IOutboundPacketType<PacketPlayOutEntityHeadLook> EntityHeadLook = new OutboundPacketType<PacketPlayOutEntityHeadLook>(0x3E);
                public static readonly IOutboundPacketType<PacketPlayOutMultiBlockChange> MultiBlockChange = new OutboundPacketType<PacketPlayOutMultiBlockChange>(0x3F);

                public static readonly IOutboundPacketType<PacketPlayOutSelectAdvancementTab> SelectAdvancementTab = new OutboundPacketType<PacketPlayOutSelectAdvancementTab>(0x40);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutActionBar> ActionBar = new OutboundPacketType<PacketPlayOutActionBar>(0x41);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutWorldBorderCenter> WorldBorderCenter = new OutboundPacketType<PacketPlayOutWorldBorderCenter>(0x42);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutWorldBorderLerp> WorldBorderLerp = new OutboundPacketType<PacketPlayOutWorldBorderLerp>(0x43);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutWorldBorderSize> WorldBorderSize = new OutboundPacketType<PacketPlayOutWorldBorderSize>(0x44);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutWorldBorderWarnDelay> WorldBorderWarnDelay = new OutboundPacketType<PacketPlayOutWorldBorderWarnDelay>(0x45);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutWorldBorderWarnReach> WorldBorderWarnReach = new OutboundPacketType<PacketPlayOutWorldBorderWarnReach>(0x46);
                // Replaced above 1.17
                //public static readonly IOutboundPacketType<PacketPlayOutWorldBorder> WorldBorder = new OutboundPacketType<PacketPlayOutWorldBorder>(0x3D);
                public static readonly IOutboundPacketType<PacketPlayOutCamera> Camera = new OutboundPacketType<PacketPlayOutCamera>(0x47);
                public static readonly IOutboundPacketType<PacketPlayOutHeldItemChange> HeldItemChange = new OutboundPacketType<PacketPlayOutHeldItemChange>(0x48);
                public static readonly IOutboundPacketType<PacketPlayOutUpdateViewPosition> UpdateViewPosition = new OutboundPacketType<PacketPlayOutUpdateViewPosition>(0x49);
                public static readonly IOutboundPacketType<PacketPlayOutUpdateViewDistance> UpdateViewDistance = new OutboundPacketType<PacketPlayOutUpdateViewDistance>(0x4A);
                public static readonly IOutboundPacketType<PacketPlayOutSpawnPosition> SpawnPosition = new OutboundPacketType<PacketPlayOutSpawnPosition>(0x4B);
                public static readonly IOutboundPacketType<PacketPlayOutDisplayScoreboard> DisplayScoreboard = new OutboundPacketType<PacketPlayOutDisplayScoreboard>(0x4C);
                public static readonly IOutboundPacketType<PacketPlayOutEntityMetadata> EntityMetadata = new OutboundPacketType<PacketPlayOutEntityMetadata>(0x4D);
                public static readonly IOutboundPacketType<PacketPlayOutAttachEntity> AttachEntity = new OutboundPacketType<PacketPlayOutAttachEntity>(0x4E);
                public static readonly IOutboundPacketType<PacketPlayOutEntityVelocity> EntityVelocity = new OutboundPacketType<PacketPlayOutEntityVelocity>(0x4F);

                public static readonly IOutboundPacketType<PacketPlayOutEntityEquipment> EntityEquipment = new OutboundPacketType<PacketPlayOutEntityEquipment>(0x50);
                public static readonly IOutboundPacketType<PacketPlayOutSetExperience> SetExperience = new OutboundPacketType<PacketPlayOutSetExperience>(0x51);
                public static readonly IOutboundPacketType<PacketPlayOutUpdateHealth> UpdateHealth = new OutboundPacketType<PacketPlayOutUpdateHealth>(0x52);
                public static readonly IOutboundPacketType<PacketPlayOutScoreboardObjective> ScoreboardObjective = new OutboundPacketType<PacketPlayOutScoreboardObjective>(0x53);
                public static readonly IOutboundPacketType<PacketPlayOutSetPassengers> SetPassengers = new OutboundPacketType<PacketPlayOutSetPassengers>(0x54);
                public static readonly IOutboundPacketType<PacketPlayOutTeams> Teams = new OutboundPacketType<PacketPlayOutTeams>(0x55);
                public static readonly IOutboundPacketType<PacketPlayOutUpdateScore> UpdateScore = new OutboundPacketType<PacketPlayOutUpdateScore>(0x56);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutSetTitleSubtitle> SetTitleSubtitle = new OutboundPacketType<PacketPlayOutSetTitleSubtitle>(0x57);
                public static readonly IOutboundPacketType<PacketPlayOutTimeUpdate> TimeUpdate = new OutboundPacketType<PacketPlayOutTimeUpdate>(0x58);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutSetTitleText> SetTitleText = new OutboundPacketType<PacketPlayOutSetTitleText>(0x59);
                // New 1.17
                public static readonly IOutboundPacketType<PacketPlayOutSetTitleTime> SetTitleTime = new OutboundPacketType<PacketPlayOutSetTitleTime>(0x5A);
                //public static readonly IOutboundPacketType<PacketPlayOutTitle> Title = new OutboundPacketType<PacketPlayOutTitle>(0x4F);
                public static readonly IOutboundPacketType<PacketPlayOutEntitySoundEffect> EntitySoundEffect = new OutboundPacketType<PacketPlayOutEntitySoundEffect>(0x5B);
                public static readonly IOutboundPacketType<PacketPlayOutSoundEffect> SoundEffect = new OutboundPacketType<PacketPlayOutSoundEffect>(0x5C);
                public static readonly IOutboundPacketType<PacketPlayOutStopSound> StopSound = new OutboundPacketType<PacketPlayOutStopSound>(0x5D);
                public static readonly IOutboundPacketType<PacketPlayOutPlayerListHeaderFooter> PlayerListHeaderFooter = new OutboundPacketType<PacketPlayOutPlayerListHeaderFooter>(0x5E);
                public static readonly IOutboundPacketType<PacketPlayOutNBTQueryResponse> NBTQueryResponse = new OutboundPacketType<PacketPlayOutNBTQueryResponse>(0x5F);

                public static readonly IOutboundPacketType<PacketPlayOutCollectItem> CollectItem = new OutboundPacketType<PacketPlayOutCollectItem>(0x60);
                public static readonly IOutboundPacketType<PacketPlayOutEntityTeleport> EntityTeleport = new OutboundPacketType<PacketPlayOutEntityTeleport>(0x61);
                public static readonly IOutboundPacketType<PacketPlayOutAdvancements> Advancements = new OutboundPacketType<PacketPlayOutAdvancements>(0x62);
                public static readonly IOutboundPacketType<PacketPlayOutEntityProperties> EntityProperties = new OutboundPacketType<PacketPlayOutEntityProperties>(0x63);
                public static readonly IOutboundPacketType<PacketPlayOutEntityEffect> EntityEffect = new OutboundPacketType<PacketPlayOutEntityEffect>(0x64);
                public static readonly IOutboundPacketType<PacketPlayOutDeclareRecipes> DeclareRecipes = new OutboundPacketType<PacketPlayOutDeclareRecipes>(0x65);
                public static readonly IOutboundPacketType<PacketPlayOutTags> Tags = new OutboundPacketType<PacketPlayOutTags>(0x66);

            }
        }
    }

    public interface IPacketType<out T> where T : IPacket
    {
        public byte PacketId { get; }
        public bool IsInbound { get; }
        public T GetBase();
    }

    public interface IInboundPacketType<T> : IPacketType<T> where T : IInboundPacket
    {
        public T ReadFromStream(Stream stream, int lengthHint);
    }

    public interface IOutboundPacketType<T> : IPacketType<T> where T : IOutboundPacket
    { }

    public class InboundPacketType<T> : IInboundPacketType<T> where T : IInboundPacket, new()
    {
        public byte PacketId { get; }
        public bool IsInbound => true;

        public InboundPacketType(byte packetId)
        {
            PacketId = packetId;
        }

        public T GetBase() => new T();

        public T ReadFromStream(Stream stream, int lengthHint)
        {
            T packet = GetBase();
            packet.ReadFromStream(stream, lengthHint);
            return packet;
        }
    }

    public class OutboundPacketType<T> : IOutboundPacketType<T> where T : IOutboundPacket, new()
    {
        public byte PacketId { get; }
        public bool IsInbound => false;

        public OutboundPacketType(byte packetId)
        {
            PacketId = packetId;
        }

        public T GetBase() => new T();
    }
}
