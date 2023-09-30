using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerCore;

namespace DummyClient
{
    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOK = 2,
    }

    public interface IPacket
    {
        ushort Protocol { get; }
        void Read(ArraySegment<Byte> segment);
        ArraySegment<byte> Write();
    }

    public class PlayerInfoReq : IPacket
    {
        public long playerId;
        public string name;
        public string chat;

        [Obsolete("Test 데이터 이기 떄문에 실사용불가")]
        public sealed class PlayerPosInfo
        {
            public bool isSelf;
            public int playerId;
            public float posX;
            public float posY;
            public float posZ;
        }

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

        public ushort Protocol { get { return (ushort)PacketID.PlayerInfoReq; } }

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            count += sizeof(ushort);
            count += sizeof(ushort);

            ushort chatLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);

            this.chat = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, chatLen);
            count += chatLen;



            //this.playerId = BitConverter.ToInt64(segment.Array, segment.Offset + count);

            //this.playerId = BtiConverter.ToInt64(s.Slice(count, s.Length - count));
            //count += sizeof(long);

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

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID.PlayerInfoReq), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, this.chat.Length, segment.Array, segment.Offset + count + sizeof(ushort));

            Array.Copy(BitConverter.GetBytes(chatLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += chatLen;

            Array.Copy(BitConverter.GetBytes(chatLen), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    class PlayerInfoOK : IPacket
    {
        public int hp;
        public int attack;

        public ushort Protocol => throw new NotImplementedException();

        public void Read(ArraySegment<byte> segment)
        {
            throw new NotImplementedException();
        }

        public ArraySegment<byte> Write()
        {
            throw new NotImplementedException();
        }
    }
}
