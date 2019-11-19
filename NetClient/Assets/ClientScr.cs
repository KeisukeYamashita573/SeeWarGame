using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

public class ClientScr : MonoBehaviour
{
    [SerializeField]
    private GameObject cube = default;
    private int port = 2001;

    enum DataType
    {
        Position,
        Rotation,
        Scale
    }

    void Start()
    {
        Thread mThread = new Thread(new ThreadStart(ConnectAsClient));
        mThread.Start();
    }

    void Update()
    {
        
    }

    private void ConnectAsClient()
    {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Parse("127.0.0.1"), port);
        NetworkStream stream = client.GetStream();
        string s = "Hello from Client";
        byte[] message = Encoding.UTF8.GetBytes(DataType.Position.ToString() + s);

        stream.Write(message, 0, message.Length);
        stream.Close();
        client.Close();
    }
}
