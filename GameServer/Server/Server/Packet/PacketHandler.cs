using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Packet
{
    class PacketHandler
    {
        public static void PlayerInfoReqHandler(PacketSession session, IPacket packet)
        {
            PlayerInfoReq p = packet as PlayerInfoReq;

            Console.WriteLine($"PlayerInfoReq: {p.playerId} {p.name}");

            foreach(PlayerInfoReq.SkillInfo skill in p.skills)
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

        //public static void ChatHandler(PacketSession session, IPacket packet)
        //{
        //    PlayerInfoReq chatPacket = packet as PlayerInfoReq;
        //    ClientSession clientSession = session as ClientSession;
        //
        //    if (clientSession.Room == null)
        //        return;
        //
        //    GameRoom room = clientSession.Room;
        //
        //    room.Push(() => room.Broadcast(clientSession, chatPacket.chat));
        //}

        public static void LeaveGameHandler(PacketSession session, IPacket packet)
        {
            //PlayerInfoReq.PlayerPosInfo leavePacket = packet as PlayerInfoReq.PlayerPosInfo;
            PlayerInfoReq.PlayerPosInfo leavePacket;
            ClientSession clientSession = session as ClientSession;

            if (clientSession.Room == null)
                return;

            GameRoom room = clientSession.Room;

            room.Push(() => room.Leave(clientSession));
        }

        public static void MoveHandler(PacketSession session, IPacket packet)
        {
            //PlayerInfoReq.PlayerPosInfo movePacket = packet as PlayerInfoReq.PlayerPosInfo;
            PlayerInfoReq.PlayerPosInfo movePacket = new PlayerInfoReq.PlayerPosInfo();
            ClientSession clientSession = session as ClientSession;

            if (clientSession.Room == null)
                return;

            //Console.WriteLine($"{movePacket.posX}, {movePacket.posY}, {movePacket.posZ}");

            GameRoom room = clientSession.Room;

            //room.Push(() => room.Move(clientSession, movePacket));
        }
    }
}
