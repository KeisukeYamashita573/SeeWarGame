using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class DataClientManager : MonoBehaviour
{
    [SerializeField]
    private InputField ipField = default;
    [SerializeField]
    private GameObject cube = default;

    public void OnSendButton()
    {
        var ipOrHost = ipField.text;
        var port = 2001;

        var tcp = new TcpClient(ipOrHost, port);
        var ns = tcp.GetStream();

        Encoding enc = Encoding.UTF8;
        byte[] sendBytes = ConvertVector3ToByteArray(cube.transform.position);
        ns.Write(sendBytes, 0, sendBytes.Length);

        MemoryStream ms = new MemoryStream();
        byte[] resBytes = new byte[256];
        int resSize = 0;
        do
        {
            resSize = ns.Read(resBytes, 0, resBytes.Length);
            if (resSize == 0)
            {
                break;
            }
            ms.Write(resBytes, 0, resSize);
        } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
        string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        ms.Close();

        ns.Close();
        tcp.Close();
    }

    private byte[] ConvertVector3ToByteArray(Vector3 pos)
    {
        byte[] tmp = new byte[3];
        tmp[0] = (byte)pos.x;
        tmp[1] = (byte)pos.y;
        tmp[2] = (byte)pos.z;
        return tmp;
    }
}
