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

namespace DummyClient
{
    public abstract class Packet
    {
        public ushort size;
        public ushort packetID;

        public abstract ArraySegment<byte> Write();
        public abstract void Read(ArraySegment<byte> s);
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;

        public PlayerInfoReq()
        {
            this.packetID = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> s)
        {
            ushort count = 0;

            //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += 2;
            //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
            count += 2;
            this.playerId = BitConverter.ToInt64(s.Array, s.Offset + count);
            count += 8;
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> s = SendBufferHelper.Open(4096);

            //현재 버전에서 지원불가?, GetBytes의 최적화 버전
            //ushort count = 0;
            //bool success = true;

            //count += 2;
            //success &= BitConverter.TryWriteBytes(new Span(s.Array, s.Offset + count, s.Count - count), packet.packetID);
            //count += 2;
            //success &= BitConverter.TryWriteBytes(new Span(s.Array, s.Offset + count, s.Count - count), packet.size);
            //count += 8;
            //success &= BitConverter.TryWriteBytes(new Span(s.Array, s.Offset, s.Count), packet.size);
            //count += 2;

            //if(success == false)
            //    return null;

            //return SendBufferHelper.Close(count);
            //


            //패킷사이즈는 밑으로 

            byte[] packetId = BitConverter.GetBytes(this.packetID);
            byte[] playerId = BitConverter.GetBytes(this.playerId);

            ushort count = 0;



            Array.Copy(packetId, 0, s.Array, s.Offset + count, 2);
            count += 2;
            Array.Copy(playerId, 0, s.Array, s.Offset + count, 8);
            count += 8;

            count += 2;
            byte[] size = BitConverter.GetBytes(count);
            Array.Copy(size, 0, s.Array, s.Offset + count, 2);

            

            return SendBufferHelper.Close(count);
        }
    }

    //class PlayerInfoOK : Packet
    //{
    //    public int hp;
    //    public int attack;
    //}

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOK = 2,
    }

    class ServerSession : Session
    {

        //이렇게도 GetBytes의 최적화 가능, 또는 비트연산 노가다 
        static unsafe void ToBytes(byte[] array, int offset, ulong value)
        {
            fixed (byte* ptr = &array[offset])
                *(ulong*)ptr = value;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001 };

            //보냄 
            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = packet.Write();

                if (s != null)
                    Send(s);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
