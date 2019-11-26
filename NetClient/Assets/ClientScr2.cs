using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class ClientScr2 : MonoBehaviour
{
    [SerializeField]
    private InputField ipField = default;
    [SerializeField]
    private InputField field = default;

    public void OnSendButton()
    {
        if (field.text == null || field.text.Length == 0) return;
        var ipOrHost = ipField.text;
        var port = 2001;

        var tcp = new TcpClient(ipOrHost, port);
        var ns = tcp.GetStream();
        Encoding enc = Encoding.UTF8;
        byte[] sendBytes = enc.GetBytes(field.text + '\n');
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
}
