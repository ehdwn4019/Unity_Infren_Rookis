using ServerCore;
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

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); });

            while(true)
            {

                try
                {
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
