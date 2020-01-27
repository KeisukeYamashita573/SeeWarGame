using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class AsynchronousClient : MonoBehaviour
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int bufferSize = 512;
        public byte[] buffer = new byte[bufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    private const int port = 2001;
    private static ManualResetEvent connectDone = new ManualResetEvent(false);
    private static ManualResetEvent sendDone = new ManualResetEvent(false);
    private static ManualResetEvent receiveDone = new ManualResetEvent(false);

    private static string response = string.Empty;

    private static void StartClient()
    {
        try
        {
            IPHostEntry ipHost = Dns.GetHostEntry("172.31.5.7");
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream,
                ProtocolType.Tcp);

            client.BeginConnect(remoteEP, new AsyncCallback(connectCallback), client);
            connectDone.WaitOne();

            Send(client, "This is a test<EOF>");
            sendDone.WaitOne();

            Receive(client);
            receiveDone.WaitOne();

            Console.WriteLine("Response receive : {0}", response);

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void connectCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

            connectDone.Set();
        }catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Send(Socket client, string str)
    {
        byte[] byteData = Encoding.UTF8.GetBytes(str);
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            int byteSent = client.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to server", byteSent);
            sendDone.Set();
        }catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Receive(Socket client)
    {
        try
        {
            StateObject state = new StateObject();
            state.workSocket = client;
            client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            int byteRead = client.EndReceive(ar);
            if (byteRead > 0)
            {
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, byteRead));
                client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }else
            {
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                receiveDone.Set();
            }
        }catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
