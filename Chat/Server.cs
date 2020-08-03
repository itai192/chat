using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Chat
{
    class Server
    {
        public static void StartServer()
        {
            try
            {
                //pre init
                const int port = 25565;
                IPAddress ip = IPAddress.Parse("192.168.1.15");
                IPEndPoint hostEndpoint = new IPEndPoint(ip, port);
                Socket listner = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listner.Bind(hostEndpoint);
                List<Socket> listners = new List<Socket>();
                listners.Add(listner);
                List<Socket> accepted = new List<Socket>();
                List<Socket> acceptable;
                List<Socket> messegesfrom;
                List<Socket> messegesTo;
                byte[] data = new byte[listner.ReceiveBufferSize];
                listner.Listen(10);
                while (true)
                {
                    //accept acceptable sockets and add to list

                    acceptable = listners.ToArray().ToList<Socket>();
                    Socket.Select(acceptable, null, null, 1000);
                    foreach (Socket ToAccept in acceptable)
                    {
                        if (ToAccept != null)
                        {

                            accepted.Add(ToAccept.Accept());
                            Console.WriteLine("accepted");
                        }
                    }
                    for(int i =accepted.Count-1;i>=0;i--)
                    {
                        if(!SocketAvalible(accepted[i]))
                        {
                            accepted[i].Shutdown(SocketShutdown.Both);
                            accepted.RemoveAt(i);
                        }
                    }
                    //broadcast
                    if (accepted.Count>0)
                    {

                        foreach (Socket rec in accepted)
                        {
                            if (rec != null&&rec.Available>0)
                            {
                                int recbytes = rec.Receive(data);
                                messege(accepted, rec, data,recbytes);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message},{e.StackTrace}");
                Console.ReadKey();
            }
        }
        public static void messege(List<Socket> to, Socket from,byte[] data,int recbytes)
        {
            byte[] dttosend = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(data,0,recbytes));
            foreach(Socket tosend in to)
            {
                if (tosend != from)
                {
                    tosend.Send(dttosend);
                }
            }
            
        }
        public static bool SocketAvalible(Socket s)
        {
            return !(s.Poll(1000, SelectMode.SelectRead) && s.Available == 0);
        }
    }
}
