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
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
            Send(sendBuff);

            Thread.Sleep(1000);
            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Client] {recvData}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }

    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            #region 캐시 테스트
            int[,] arr = new int[10000, 10000];
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 순서 걸린 시간 {end-now}");
                //같은 공간을 참조하므로 시간이 적게 걸린다. 
                //ex y,x이므로 0,1 0,2 0,3 .... 이므로 이미 0이 참조된 상태로 같은 공간에 있으므로 다음 인덱스를 참조할것이다라고 예상하여 미리 캐싱해놓는다?
            }

            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[x, y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 순서 걸린 시간 {end-now}");
                //다른 공간을 참조하므로 시간이 더걸린다. 
                //ex x,y이므로 1,0 2,0 3,0 .... 이므로 같은 공간일줄알고 다음 인덱스들을 캐싱해놨는데 공간을 벗어나므로 시간이 좀더 걸린다? 
            }
            #endregion

            // DNS >> Domain Name System 
            // www.kdj.com >> 도메인 등록, IP가 아닌 string 값으로 관리, IP를 적어넣으면 하드코딩 

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0]; // google 같이 많이 접속하는 사이트는 여러개의 IP를 가지고 있다. 그래서 list로 받음 
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return new GameSession(); });

            while (true)
            {
                //임시로 서버 계속 실행시키기 위한 무한루프 
            }
        }
    }
}
