﻿using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DummyClient.Packet
{
    class PacketHandler
    {
        public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;

            Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.name}");

            foreach (PlayerInfoReq.SkillInfo skill in p.skills)
            {
                Console.WriteLine($"Skill({skill.id})({skill.level})({skill.duration})");
            }
        }

        public static void TestHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;

            Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.name}");

            foreach (PlayerInfoReq.SkillInfo skill in p.skills)
            {
                Console.WriteLine($"Skill({skill.id})({skill.level})({skill.duration})");
            }
        }

        public static void ChatHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq chatPacket = packet as PlayerInfoReq;
            ServerSession serverSession = session as ServerSession;

            if(chatPacket.playerId == 1)
            {
                Debug.Log(chatPacket.chat);

                GameObject go = GameObject.Find("Player");
                if (go == null)
                    Debug.Log("Player not found");
                else
                    Debug.Log("Player found");
            }
                

            //if(chatPacket.playerId == 1)
            //Console.WriteLine(chatPacket.chat);
        }

        public static void BroadcastEnterGameHandler(PacketSession session, IPacket packet)
        {
            //PlayerInfoReq.PlayerPosInfo pkt = packet as PlayerInfoReq.PlayerPosInfo;
            ServerSession serverSession = session as ServerSession;

            //PlayerManager.Instance.EnterGame(pkt);
        }

        public static void BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
        {
            //PlayerInfoReq.PlayerPosInfo pkt = packet as PlayerInfoReq.PlayerPosInfo;
            ServerSession serverSession = session as ServerSession;

            //PlayerManager.Instance.LeaveGame(pkt);
        }

        public static void PlayerListHandler(PacketSession session, IPacket packet)
        {
            //PlayerInfoReq.PlayerPosInfo pkt = packet as PlayerInfoReq.PlayerPosInfo;
            ServerSession serverSession = session as ServerSession;

            //PlayerManager.Instance.Add(pkt);
        }

        public static void BroadcastMoveHandler(PacketSession session, IPacket packet)
        {
            //PlayerInfoReq.PlayerPosInfo pkt = packet as PlayerInfoReq.PlayerPosInfo;
            ServerSession serverSession = session as ServerSession;

            //PlayerManager.Instance.Move(pkt);
        }
    }
}
