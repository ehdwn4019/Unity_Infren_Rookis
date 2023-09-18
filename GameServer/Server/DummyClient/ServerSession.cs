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
        public string name;

        public struct SkillInfo
        {
            public int id;
            public short level;
            public float duration;

            //public bool Write(Span<byte> s, ref ushort count)
            //{
            //    bool success = true;
            //
            //    success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), id);
            //    count += sizeof(int);
            //    success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), level);
            //    count += sizeof(short);
            //    success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), duration);
            //    count += sizeof(float);
            //
            //    return true;
            //}

            //public void Read(ReadOnlySpan<byte> s, ref ushort count)
            //{
            //    id = BitConverter.ToInt32(s.Slice(count, s.Length - count));
            //    count += sizeof(int);
            //    level = BitConverter.ToInt16(s.Slice(count, s.Length - count));
            //    count += sizeof(short);
            //    duration = BitConverter.ToSingle(s.Slice(count, s.Length - count));
            //    count += sizeof(float);
            //}
        }   //

        public List<SkillInfo> skills = new List<SkillInfo>();

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

            skills.Clear();
            //ushort skillLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
            //count += sizeof(ushort);
            //for(int i=0; i<skillLen; i++)
            //{
            //    SkillInfo skill = new SkillInfo();
            //    skill.Read(s, ref count);
            //    skills.Add(skill);
            //}
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

            //
            //ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(this.name);
            //sucess &= BitConverter.TryWriteBytes(s.Slice(CountdownEvent, s.Length - count), nameLen);
            //count += sizeof(ushort);
            //Array.Copy(Encoding.Unicode.GetBytes(this.name), 0, segment.Array, CountdownEvent, nameLen);
            //count += nameLen;

            //위에거 개선하면 밑에거 

            //ushort nameLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            //sucess &= BitConverter.TryWriteBytes(s.Slice(CountdownEvent, s.Length - count), nameLen);
            //count += sizeof(ushort);
            //count += nameLen;
            //

            //sucess &= BitConverter.TryWriteBytes(s.Slice(CountdownEvent, s.Length - count), (ushort)skills.Count);
            //count += sizeof(ushort);
            //foreach(SkillInfo skill in skills)
            //{
            //    success = &= skill.Write(s, ref count);
            //}

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

            PlayerInfoReq packet = new PlayerInfoReq() { playerId = 1001, name = "ABCD" };
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 101, level = 1, duration = 3.0f});
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 201, level = 2, duration = 4.0f });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 301, level = 3, duration = 5.0f });
            packet.skills.Add(new PlayerInfoReq.SkillInfo() { id = 401, level = 4, duration = 6.0f });
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
