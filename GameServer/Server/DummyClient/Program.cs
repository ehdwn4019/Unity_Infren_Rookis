using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0]; // google 같이 많이 접속하는 사이트는 여러개의 IP를 가지고 있다. 그래서 list로 받음 
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            while(true)
            {
                //클라 설정 
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    //서버 접속
                    socket.Connect(endPoint);
                    Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");

                    //보냄 
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World");
                    int sendBytes = socket.Send(sendBuff);

                    //받음 
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"[From Server] {recvData}");

                    //퇴장 
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());

                }

                Thread.Sleep(100);
            }
        }
    }
}
