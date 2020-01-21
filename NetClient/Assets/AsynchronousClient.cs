using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net.Sockets;

public class AsynchronousClient : MonoBehaviour
{
    void Start()
    {
        var soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
}
