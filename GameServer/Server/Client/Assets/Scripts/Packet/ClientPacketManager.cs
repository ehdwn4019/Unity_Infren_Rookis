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

        Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
        Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

        public void Register()
        {
            _makeFunc.Add((ushort)PacketID.PlayerInfoReq, MakePacket<PlayerInfoReq>);
            _handler.Add((ushort)PacketID.PlayerInfoReq, PacketHandler.PlayerInfoReqHandler);
            _makeFunc.Add((ushort)PacketID.PlayerInfoOK, MakePacket<PlayerInfoOK>);
            _handler.Add((ushort)PacketID.PlayerInfoOK, PacketHandler.TestHandler);

        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
            if (_makeFunc.TryGetValue(id, out func))
            {
                IPacket packet = func.Invoke(session, buffer);
                if (onRecvCallback != null)
                    onRecvCallback.Invoke(session, packet);
                else
                    HandlePacket(session, packet);
            }
        }

        T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
        {
            T pkt = new T();
            pkt.Read(buffer);
            return pkt;
        }

        public void HandlePacket(PacketSession session, IPacket packet)
        {
            Action<PacketSession, IPacket> action = null;
            if (_handler.TryGetValue(packet.Protocol, out action))
                action.Invoke(session, packet);
        }
    }
}
