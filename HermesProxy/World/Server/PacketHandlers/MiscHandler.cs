﻿using Framework.Constants;
using HermesProxy.Enums;
using HermesProxy.World;
using HermesProxy.World.Enums;
using HermesProxy.World.Objects;
using HermesProxy.World.Server.Packets;

namespace HermesProxy.World.Server
{
    public partial class WorldSocket
    {
        // Handlers for CMSG opcodes coming from the modern client
        [PacketHandler(Opcode.CMSG_TIME_SYNC_RESPONSE)]
        void HandleTimeSyncResponse(TimeSyncResponse response)
        {
            if (LegacyVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
            {
                WorldPacket packet = new WorldPacket(Opcode.CMSG_TIME_SYNC_RESPONSE);
                packet.WriteUInt32(response.SequenceIndex);
                packet.WriteUInt32(response.ClientTime);
                SendPacketToServer(packet);
            }
        }

        [PacketHandler(Opcode.CMSG_AREA_TRIGGER)]
        void HandleAreaTrigger(AreaTriggerPkt at)
        {
            if (at.Entered == false)
                return;

            GetSession().GameState.LastEnteredAreaTrigger = at.AreaTriggerID;
            WorldPacket packet = new WorldPacket(Opcode.CMSG_AREA_TRIGGER);
            packet.WriteUInt32(at.AreaTriggerID);
            SendPacketToServer(packet);
        }

        [PacketHandler(Opcode.CMSG_SET_SELECTION)]
        void HandleSetSelection(SetSelection selection)
        {
            WorldPacket packet = new WorldPacket(Opcode.CMSG_SET_SELECTION);
            packet.WriteGuid(selection.TargetGUID.To64());
            SendPacketToServer(packet);
        }
        [PacketHandler(Opcode.CMSG_REPOP_REQUEST)]
        void HandleRepopRequest(RepopRequest repop)
        {
            WorldPacket packet = new WorldPacket(Opcode.CMSG_REPOP_REQUEST);
            if (LegacyVersion.AddedInVersion(ClientVersionBuild.V2_0_1_6180))
                packet.WriteBool(repop.CheckInstance);
            SendPacketToServer(packet);
        }
        [PacketHandler(Opcode.CMSG_QUERY_CORPSE_LOCATION_FROM_CLIENT)]
        void HandleQueryCorpseLocationFromClient(QueryCorpseLocationFromClient query)
        {
            WorldPacket packet = new WorldPacket(Opcode.MSG_CORPSE_QUERY);
            SendPacketToServer(packet);
        }
        [PacketHandler(Opcode.CMSG_RECLAIM_CORPSE)]
        void HandleReclaimCorpse(ReclaimCorpse corpse)
        {
            WorldPacket packet = new WorldPacket(Opcode.CMSG_RECLAIM_CORPSE);
            packet.WriteGuid(corpse.CorpseGUID.To64());
            SendPacketToServer(packet);
        }
        [PacketHandler(Opcode.CMSG_STAND_STATE_CHANGE)]
        void HandleStandStateChange(StandStateChange state)
        {
            WorldPacket packet = new WorldPacket(Opcode.CMSG_STAND_STATE_CHANGE);
            packet.WriteUInt32(state.StandState);
            SendPacketToServer(packet);
        }
        [PacketHandler(Opcode.CMSG_OPENING_CINEMATIC)]
        [PacketHandler(Opcode.CMSG_NEXT_CINEMATIC_CAMERA)]
        [PacketHandler(Opcode.CMSG_COMPLETE_CINEMATIC)]
        void HandleCinematicPacket(ClientCinematicPkt cinematic)
        {
            WorldPacket packet = new WorldPacket(cinematic.GetUniversalOpcode());
            SendPacketToServer(packet);
        }

        [PacketHandler(Opcode.CMSG_FAR_SIGHT)]
        void HandleFarSight(FarSight sight)
        {
            WorldPacket packet = new WorldPacket(Opcode.CMSG_FAR_SIGHT);
            packet.WriteBool(sight.Enable);
            SendPacketToServer(packet);
        }

        [PacketHandler(Opcode.CMSG_MOUNT_SPECIAL_ANIM)]
        void HandleMountSpecialAnim(MountSpecial mount)
        {
            WorldPacket packet = new WorldPacket(Opcode.CMSG_MOUNT_SPECIAL_ANIM);
            SendPacketToServer(packet);
        }

        [PacketHandler(Opcode.CMSG_TUTORIAL_FLAG)]
        void HandleTutorialFlag(TutorialSetFlag tutorial)
        {
            switch (tutorial.Action)
            {
                case TutorialAction.Clear:
                {
                    WorldPacket packet = new WorldPacket(Opcode.CMSG_TUTORIAL_CLEAR);
                    SendPacketToServer(packet);
                    break;
                }
                case TutorialAction.Reset:
                {
                    WorldPacket packet = new WorldPacket(Opcode.CMSG_TUTORIAL_RESET);
                    SendPacketToServer(packet);
                    break;
                }
                case TutorialAction.Update:
                {
                    WorldPacket packet = new WorldPacket(Opcode.CMSG_TUTORIAL_FLAG);
                    packet.WriteUInt32(tutorial.TutorialBit);
                    SendPacketToServer(packet);
                    break;
                }
            }
        }
    }
}
