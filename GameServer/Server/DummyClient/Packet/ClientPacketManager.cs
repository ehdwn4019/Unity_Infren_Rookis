using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient.Packet
{
    class ClientPacketManager
    {
        #region Singleton
        static ClientPacketManager _instance;
        public static ClientPacketManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientPacketManager();
                return _instance;
            }
        }
        #endregion

        Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
        Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

        public void Register()
        {
            _onRecv.Add((ushort)PacketID.PlayerInfoReq, MakePacket<PlayerInfoReq>);
            _handler.Add((ushort)PacketID.PlayerInfoReq, PacketHandler.PlayerInfoReqHandler);
            _onRecv.Add((ushort)PacketID.PlayerInfoOK, MakePacket<PlayerInfoOK>);
            _handler.Add((ushort)PacketID.PlayerInfoOK, PacketHandler.TestHandler);

        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            Action<PacketSession, ArraySegment<byte>> action = null;
            if (_onRecv.TryGetValue(id, out action))
                action.Invoke(session, buffer);

            switch ((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    {
                        PlayerInfoReq p = new PlayerInfoReq();
                        p.Read(buffer);
                    }
                    break;

                default:
                    break;
            }
        }

        void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
        {
            T pkt = new T();
            pkt.Read(buffer);

            Action<PacketSession, IPacket> action = null;
            if (_handler.TryGetValue(pkt.Protocol, out action))
                action.Invoke(session, pkt);
        }
    }
}
