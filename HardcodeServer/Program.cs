using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HardcodeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 13000;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10);
            Console.WriteLine("listener запущен");
            while (true)
            {
                var listener = tcpSocket.Accept();
                var buffer = new byte[256];
                var size = 0;
                //контейнер запроса/ответа от клиента
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                } while (listener.Available > 0);


                Console.WriteLine("Client: \r\n{0}", data);

                //обратный ответ
                string toClientMessage = "HTTP / 1.1 200 OK\r\nDate: Fri, 17 Apr 2020 17:39:45 GMT\r\nServer: Apache / 1.3.42(Unix) mod_deflate / 1.0.21\r\nSet - Cookie: red1 = 1; expires = Wed, 14 - Oct - 2020 17:39:45 GMT\r\nConnection: close\r\nTransfer - Encoding: chunked\r\nContent - Type: text / html; charset = windows - 1251\r\nX - Pad: avoid browser bug\r\n\r\n<h1>HELLO WORLD</h1>";
                listener.Send( Encoding.UTF8.GetBytes(toClientMessage));
                Console.WriteLine("Server: \r\n{0}", toClientMessage);

                //закрываем наше подключение
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
