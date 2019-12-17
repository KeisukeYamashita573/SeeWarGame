using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Server
{
    class DataSaverManager
    {
        static void CreatePlData(Dictionary<string,string> Pldata)
        {
            var ms = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(PlayerData));
            serializer.WriteObject(ms, Pldata);
        }
        static PlayerData GetPlDataFromJson(string name)
        {
            PlayerData data;
            var serializer = new DataContractJsonSerializer(typeof(PlayerData));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(name)))
            {
                data = (PlayerData)serializer.ReadObject(ms);
            }
            return data;
        }

        // 受け取ったメッセージをもとにプレイヤーの作成、ログインなどを行う
        static void CreateAndLoginProcc(NetworkStream netStream, Encoding enc, TcpClient client, string msg,ref Dictionary<string,string> plList, bool disconnect)
        {
            if (msg.Contains("CreatePlayer"))
            {
                if (!plList.ContainsValue(msg.Substring(msg.IndexOf(":") + 1)))
                {
                    var address = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    address = address.Substring(address.IndexOf('.', address.Length - 5));
                    if (!plList.ContainsKey(address))
                    {
                        plList.Add(address, msg.Substring(msg.IndexOf(":") + 1));
                    }
                    
                    if (!disconnect)
                    {
                        //クライアントにデータを送信する
                        //クライアントに送信する文字列を作成
                        //文字列をByte型配列に変換
                        var key = plList.FirstOrDefault(c => c.Value == msg.Substring(msg.IndexOf(":") + 1));
                        byte[] sendBytes = enc.GetBytes("Success ID:" + key.Key + '\n');
                        //データを送信する
                        netStream.Write(sendBytes, 0, sendBytes.Length);
                        //末尾の\nを削除
                        var name = plList[address];
                        Console.WriteLine("CreatePlayer : " + name);
                        Console.WriteLine("SendID : " + plList.FirstOrDefault(c => c.Value == name).Key);
                    }
                }
                else
                {
                    if (!disconnect)
                    {
                        //クライアントにデータを送信する
                        //クライアントに送信する文字列を作成
                        //文字列をByte型配列に変換
                        byte[] sendBytes = enc.GetBytes("既に存在します。" + '\n');
                        //データを送信する
                        netStream.Write(sendBytes, 0, sendBytes.Length);
                        Console.WriteLine("Filed CreatePlayer..");
                    }
                }
            }
            else if (msg.Contains("LogInPlayer"))
            {
                var tmpStrAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                tmpStrAddress = tmpStrAddress.Substring(tmpStrAddress.IndexOf('.', tmpStrAddress.Length - 5));
                if (plList.ContainsKey(tmpStrAddress))
                {
                    if (plList[tmpStrAddress] == msg.Substring(msg.IndexOf(":") + 1))
                    {
                        if (!disconnect)
                        {
                            //クライアントにデータを送信する
                            //クライアントに送信する文字列を作成
                            //文字列をByte型配列に変換
                            var key = plList.FirstOrDefault(c => c.Value == msg.Substring(msg.IndexOf(":") + 1));
                            byte[] sendBytes = enc.GetBytes("Success ID:" + key.Key + '\n');
                            //データを送信する
                            netStream.Write(sendBytes, 0, sendBytes.Length);
                            var name = plList[tmpStrAddress];
                            Console.WriteLine("LogInPlayer : " + name);
                            Console.WriteLine("SendID : " + plList.FirstOrDefault(c => c.Value == name).Key);
                        }
                    }
                    else
                    {
                        if (!disconnect)
                        {
                            //クライアントにデータを送信する
                            //クライアントに送信する文字列を作成
                            //文字列をByte型配列に変換
                            byte[] sendBytes = enc.GetBytes("プレイヤー名が一致しません。" + '\n');
                            //データを送信する
                            netStream.Write(sendBytes, 0, sendBytes.Length);
                            Console.WriteLine("Filed LogInPlayer..");
                        }
                    }
                }
                else
                {
                    if (!disconnect)
                    {
                        //クライアントにデータを送信する
                        //クライアントに送信する文字列を作成
                        //文字列をByte型配列に変換
                        byte[] sendBytes = enc.GetBytes("存在しないプレイヤーです。" + '\n');
                        //データを送信する
                        netStream.Write(sendBytes, 0, sendBytes.Length);
                        Console.WriteLine("Not Fined PlayerKey..");
                    }
                }
            }
            else
            {
                if (!disconnect)
                {
                    //クライアントにデータを送信する
                    //クライアントに送信する文字列を作成
                    //文字列をByte型配列に変換
                    var key = plList.FirstOrDefault(c => c.Value == msg.Substring(msg.IndexOf(":") + 1));
                    byte[] sendBytes = enc.GetBytes(msg + "ID:" + key.Key + '\n');
                    //データを送信する
                    netStream.Write(sendBytes, 0, sendBytes.Length);
                }

                //末尾の\nを削除
                Console.WriteLine(plList[((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()]);
                Console.WriteLine("ID : ");
                msg = msg.TrimEnd('\n');
                Console.WriteLine(msg);
            }
        }

        static void Main(string[] args)
        {
            //ListenするIPアドレス
            string ipString = "127.0.0.1";
            IPAddress ipAdd = IPAddress.Parse(ipString);
            // first IPアドレスの下3桁、second プレイヤー名
            Dictionary<string, string> AddressToPlayer = new Dictionary<string, string>();
            if (GetPlDataFromJson("PlayerData") == null)
            {
                AddressToPlayer = GetPlDataFromJson("PlayerData").PlLists;
            }
            else
            {
                CreatePlData(AddressToPlayer);
            }
            
            //ホスト名からIPアドレスを取得する時は、次のようにする
            //string host = "localhost";
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.GetHostEntry(host).AddressList[0];
            //.NET Framework 1.1以前では、以下のようにする
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.Resolve(host).AddressList[0];

            //Listenするポート番号
            int port = 2001;

            //TcpListenerオブジェクトを作成する
            TcpListener listener =
                new TcpListener(IPAddress.Any, port);

            //Listenを開始する
            label1:
            listener.Start();
            Console.WriteLine("Listenを開始しました({0}:{1})。",
                ((IPEndPoint)listener.LocalEndpoint).Address,
                ((IPEndPoint)listener.LocalEndpoint).Port);

            //接続要求があったら受け入れる
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("クライアント({0}:{1})と接続しました。",
                ((IPEndPoint)client.Client.RemoteEndPoint).Address,
                ((IPEndPoint)client.Client.RemoteEndPoint).Port);
            //var addressStr = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            //var idx = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString().IndexOf('.', addressStr.Length - 5); ;
            //var tmpstr = addressStr.Substring(8);
            //if (!AddressToPlayer.ContainsKey(addressStr))
            //{
            //    AddressToPlayer.Add(addressStr, "Player" + tmpstr);
            //}

            //NetworkStreamを取得
            NetworkStream ns = client.GetStream();

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            //ns.ReadTimeout = 10000;
            //ns.WriteTimeout = 10000;

            string resMsg = "";

            //クライアントから送られたデータを受信する
            Encoding enc = Encoding.UTF8;
            bool disconnected = false;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] resBytes = new byte[256];
            int resSize = 0;
            do
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                //Readが0を返した時はクライアントが切断したと判断
                if (resSize == 0)
                {
                    disconnected = true;
                    Console.WriteLine("クライアントが切断しました。");
                    break;
                }
                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                // 受信を続ける
            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
            //受信したデータを文字列に変換
            resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();

            // 受け取ったメッセージを処理
            CreateAndLoginProcc(ns, enc, client, resMsg,ref AddressToPlayer, disconnected);

            //閉じる
            ns.Close();
            client.Close();
            Console.WriteLine("クライアントとの接続を閉じました。");

            //リスナを閉じる
            listener.Stop();
            Console.WriteLine("Listenerを閉じました。");

            if (resMsg != "Quit")
            {
                goto label1;
            }

            Console.ReadLine();
        }
    }
}
