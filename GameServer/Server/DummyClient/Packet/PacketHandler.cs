using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //if(chatPacket.playerId == 1)
            //Console.WriteLine(chatPacket.chat);
        }
    }
}
