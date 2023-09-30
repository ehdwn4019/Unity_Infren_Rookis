using ServerCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.InteropServices;
using DummyClient.Packet;

namespace DummyClient
{
    class ServerSession : PacketSession
    {
        //이렇게도 GetBytes의 최적화 가능, 또는 비트연산 노가다 
        //static unsafe void ToBytes(byte[] array, int offset, ulong value)
        //{
        //    fixed (byte* ptr = &array[offset])
        //        *(ulong*)ptr = value;
        //}

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name = "ABCD" };
            //packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 101, level = 1, duration = 3.0f});
            //packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 201, level = 2, duration = 4.0f });
            //packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 301, level = 3, duration = 5.0f });
            //packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 401, level = 4, duration = 6.0f });
            ////보냄 
            ////for (int i = 0; i < 5; i++)
            //{
            //    ArraySegment<byte> s = packet.Write();
            //
            //    if (s != null)
            //        Send(s);
            //}
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ClientPacketManager.Instance.OnRecvPacket(this, buffer, (s, p)=> PacketQueue.Instance.Push(p));
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
