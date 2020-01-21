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
    private InputField playerNameField = default;

    private GameObject cube = default;
    private int port = 2001;

	private string ipOrHost = "";
	private TcpClient tcp;
	private NetworkStream ns;
	private Encoding enc;
	private string id;
	private byte[] resBytes;
	private MemoryStream ms;
	private int resSize;
	private string resMsg;
	public void Start()
    {
        ipField.text = "172.31.5.7";
    }

	public void OnSendButton()
    {
		cube = GameObject.FindGameObjectWithTag("wahu-");
		ipOrHost = ipField.text;

		tcp = new TcpClient(ipOrHost, port);
		ns = tcp.GetStream();

		enc = Encoding.UTF8;
		id = cube.GetComponent<koma>().GetFieldID.ToString() + '\n';
		byte[] sendBytes = enc.GetBytes(id);

		ns.Write(sendBytes, 0, sendBytes.Length);

		ms = new MemoryStream();
		resBytes = new byte[256];
		resSize = 0;
		do
		{
			resSize = ns.Read(resBytes, 0, resBytes.Length);
			if (resSize == 0)
			{
				break;
			}
			ms.Write(resBytes, 0, resSize);
		} while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
		resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
		Debug.Log(resMsg);
		foreach (GameObject masu in GameObject.FindObjectOfType<field>().GetMasuLists)
		{
			if (masu.GetComponent<button>().GetID == int.Parse(resMsg.Substring(resMsg.IndexOf(":") + 1)))
			{
				GameObject.FindObjectOfType<koma>().SetPos = masu.GetComponent<button>().GetPos;
				break;
			}
		}
		ms.Close();
		ns.Close();
		tcp.Close();
	}

    public void CreatePlayer()
    {
        var tcp = new TcpClient(ipField.text, port);
        var ns = tcp.GetStream();

        Encoding enc = Encoding.UTF8;
        string sendData = "CreatePlayer:" + playerNameField.text + '\n';
        byte[] sendBytes = enc.GetBytes(sendData);

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
        Debug.Log(resMsg);
        ms.Close();
        ns.Close();
        tcp.Close();
    }

    public void LoginPlayer()
    {
        var tcp = new TcpClient(ipField.text, port);
        var ns = tcp.GetStream();

        Encoding enc = Encoding.UTF8;
        string sendData = "LogInPlayer:" + playerNameField.text + '\n';
        byte[] sendBytes = enc.GetBytes(sendData);

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
