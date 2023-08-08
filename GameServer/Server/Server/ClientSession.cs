using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using ServerCore;

namespace Server
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
        public string name;

        public PlayerInfoReq()
        {
            this.packetID = (ushort)PacketID.PlayerInfoReq;
        }

        public override void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //ReaOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

            //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            count += sizeof(ushort);
            //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + count);
            count += sizeof(ushort);
            this.playerId = BitConverter.ToInt64(segment.Array, segment.Offset + count);
            //this.playerId = BtiConverter.ToInt64(s.Slice(count, s.Length - count));
            count += sizeof(long);

            //ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
            //count += sizeof(ushort);
            //this.name = Encoding.Unicode.GetString(s.Slice(count, nameLen));
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);

            //현재 버전에서 지원불가?, GetBytes의 최적화 버전
            //ushort count = 0;
            //bool success = true;

            //count += sizeof(ushort);
            //success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), packet.packetID);
            //count += sizeof(ushort);
            //success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count, s.Count - count), packet.size);
            //count += sizeof(long);

            //ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(this.name);
            //sucess &= BitConverter.TryWriteBytes(s.Slice(CountdownEvent, s.Length - count), nameLen);
            //count += sizeof(ushort);
            //Array.Copy(Encoding.Unicode.GetBytes(this.name), 0, segment.Array, CountdownEvent, nameLen);
            //count += nameLen;

            //success &= BitConverter.TryWriteBytes(s, count);

            //if(success == false)
            //    return null;

            //return SendBufferHelper.Close(count);
            //


            //패킷사이즈는 밑으로 

            byte[] packetId = BitConverter.GetBytes(this.packetID);
            byte[] playerId = BitConverter.GetBytes(this.playerId);

            ushort count = 0;



            Array.Copy(packetId, 0, segment.Array, segment.Offset + count, 2);
            count += 2;
            Array.Copy(playerId, 0, segment.Array, segment.Offset + count, 8);
            count += 8;

            count += 2;
            byte[] size = BitConverter.GetBytes(count);
            Array.Copy(size, 0, segment.Array, segment.Offset + count, 2);



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

    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //Packet packet = new Packet() { size = 100, packetID = 10 };
            //
            //ArraySegment<byte> openSegment = SendBufferHelper.Open(4096);
            //byte[] buffer = BitConverter.GetBytes(packet.size);
            //byte[] buffer2 = BitConverter.GetBytes(packet.packetID);
            //Array.Copy(buffer, 0, openSegment.Array, openSegment.Offset, buffer.Length);
            //Array.Copy(buffer2, 0, openSegment.Array, openSegment.Offset + buffer.Length, buffer2.Length);
            //ArraySegment<byte> sendBuff = SendBufferHelper.Close(buffer.Length + buffer2.Length);


            //Send(sendBuff);
            Thread.Sleep(5000);
            Disconnect();
        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            switch ((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    {
                        PlayerInfoReq p = new PlayerInfoReq();
                        p.Read(buffer);
                        Console.WriteLine($"PlayerInfoReq : {p.playerId} {p.name}");
                    }
                    break;
                case PacketID.PlayerInfoOK:
                    {

                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine($"RecvPacketID : {id}, Size : {size}");

        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
