using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Chat_Client
{
    class Client
    {
        public static void StartClient()
        {
            
            const int port = 25565;
            IPAddress hostip = IPAddress.Parse("46.120.2.20");
            IPEndPoint hostEnd = new IPEndPoint(hostip, port);
            Socket conn = new Socket(hostip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            conn.Connect(hostEnd);
            Byte[] data = new Byte[1024];
            string msgTosend = "";
            while (true)
            {
                if(Console.KeyAvailable)
                {
                    ConsoleKeyInfo key =Console.ReadKey();
                    switch(key.Key)
                    {
                        case ConsoleKey.Enter:
                            {
                                byte[] msgbyt = Encoding.ASCII.GetBytes(msgTosend);
                                conn.Send(msgbyt);
                                msgTosend = "";
                                Console.WriteLine("");
                                Process.Start("chrome.exe", "https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                                break;
                            }
                        case ConsoleKey.Backspace:
                        {
                            if(msgTosend.Length>0)
                            {
                                msgTosend = msgTosend.Remove(msgTosend.Length-1);
                            }
                                break;
                        }
                        default:
                            {
                                msgTosend += key.KeyChar;
                                break;
                            }

                                
                    }

                    
                }
                while (conn.Available>0)
                {
                    string msg = "";
                    int size = conn.Receive(data);
                    string Rec = Encoding.ASCII.GetString(data, 0, size);
                    msg += Rec;
                    Console.WriteLine(msg);
                    msg = "";
                }
                
            }
        }
    }
}
