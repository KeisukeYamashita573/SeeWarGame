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
    private static Socket soc;
    void Start()
    {
        soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        StartAccept(soc);
    }

    private static void StartAccept(Socket server)
    {
        server.BeginAccept(new AsyncCallback(AcceptCallback), server);
    }

    private static void AcceptCallback(IAsyncResult ar)
    {
        Socket server = (Socket)ar.AsyncState;

        Socket client = null;
        try
        {
            client = server.EndAccept(ar);
        }
        catch
        {
            Console.WriteLine("Closed...");
            return;
        }

        client.Send(Encoding.UTF8.GetBytes("Hello"));
        client.Shutdown(SocketShutdown.Both);
        client.Close();

        server.BeginAccept(new AsyncCallback(AcceptCallback), server);
    }

    private void Connect(string host, int port)
    {
        Debug.Log("Connect" + "ThreadID:" + Thread.CurrentThread.ManagedThreadId);
        IPEndPoint ipEnd = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
        soc.Connect(ipEnd);
        soc.BeginConnect(ipEnd, new AsyncCallback(ConnectCallback), soc);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        Debug.Log("ConnectCallback" + "ThreadID:" + Thread.CurrentThread.ManagedThreadId);
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            Debug.Log("Socket Connected to " + client.RemoteEndPoint.ToString());
            StartReceive();
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    private void StartReceive()
    {
        Debug.Log("StartReceive ThreadID:" + Thread.CurrentThread.ManagedThreadId);
        int len = -1;
        
    }

    public void Send(string str)
    {
        Encoding enc;
        Debug.Log("Send ThreadID:" + Thread.CurrentThread.ManagedThreadId);

    }
}
