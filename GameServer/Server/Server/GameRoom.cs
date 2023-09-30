using ServerCore;
using ServerCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
                s.Send(_pendingList);

            //Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }

        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);   
        }

        public void Enter(ClientSession session)
        {
            // 플레이어가 추가 되는 부분 
            _sessions.Add(session);
            session.Room = this;

            // 새로 입장한 플레이어에게 모든 플레이어 정보 전송 

            //테스트 용 임시 데이터 
            List<PlayerInfoReq.PlayerPosInfo> players = new List<PlayerInfoReq.PlayerPosInfo>();
            foreach(ClientSession s in _sessions)
            {
                players.Add(new PlayerInfoReq.PlayerPosInfo()
                {
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.PoX, 
                    posY = s.PoY,
                    posZ = s.PoZ,
                });
            }

            // 플레이어 정보들 Send, Test 데이터 이기 떄문에 사용불가해서 주석처리 
            //session.Send(players.Write());

            // 모든 플레이어에게 새로 입장한 플레이어 정보 전송 
            PlayerInfoReq.PlayerPosInfo enter = new PlayerInfoReq.PlayerPosInfo();
            enter.playerId = session.SessionId;
            enter.posX = 0;
            enter.posY = 0;
            enter.posZ = 0;

            // 플레이어 정보들 Send, Test 데이터 이기 떄문에 사용불가해서 주석처리 
            //Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            // 플레이어가 제거 되는 부분 
            _sessions.Remove(session);

            // 모두에게 알림
            PlayerInfoReq.PlayerPosInfo leave = new PlayerInfoReq.PlayerPosInfo();
            leave.playerId = session.SessionId;

            // 플레이어 정보들 Send, Test 데이터 이기 떄문에 사용불가해서 주석처리 
            //Broadcast(leave);
        }

        public void Move(ClientSession session, PlayerInfoReq.PlayerPosInfo packet)
        {
            // 좌표 변경 해줌
            session.PoX = packet.posX;
            session.PoY = packet.posY;
            session.PoZ = packet.posZ;

            // 모두에게 알림 
            PlayerInfoReq.PlayerPosInfo move = new PlayerInfoReq.PlayerPosInfo();
            move.playerId = session.SessionId;
            move.posX = session.PoX;
            move.posY = session.PoY;
            move.posZ = session.PoZ;
            //Broadcast(move.Write());
        }
    }
}
