using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    class AsynchronousServer
    {
        private class AsyncStateObject
        {
            public Socket Socket;
            public byte[] ReceiveBuff;
            public MemoryStream ReceivedData;

            public AsyncStateObject(Socket soc)
            {
                this.Socket = soc;
                this.ReceiveBuff = new byte[1024];
                this.ReceivedData = new MemoryStream();
            }
        }

        static void Main(string[] args)
        {
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            StartReceive(soc);
        }

        private static void StartReceive(Socket soc)
        {
            // BeginReceiveで非同期受信を開始する
            AsyncStateObject stateObj = new AsyncStateObject(soc);
            soc.BeginReceive(stateObj.ReceiveBuff, 0, stateObj.ReceiveBuff.Length, SocketFlags.None, new AsyncCallback(ReceiveDataCallBack), stateObj);
        }

        private static void ReceiveDataCallBack(IAsyncResult ar)
        {
            // 状態オブジェクト取得
            AsyncStateObject stateObj = (AsyncStateObject)ar.AsyncState;
            int len = 0;
            try
            {
                len = stateObj.Socket.EndReceive(ar);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Closed...");
                return;
            }

            // lenが0の場合は切断されたと判断
            if (len <= 0)
            {
                Console.WriteLine("Disconnected...");
                stateObj.Socket.Close();
                return;
            }

            // 受信データの貯める
            stateObj.ReceivedData.Write(stateObj.ReceiveBuff, 0, len);
            if (stateObj.Socket.Available == 0)
            {
                // 受信したデータを文字列に変換
                string str = Encoding.UTF8.GetString(stateObj.ReceivedData.ToArray());
                // 表示
                Console.WriteLine(str);
                stateObj.ReceivedData.Close();
                stateObj.ReceivedData = new MemoryStream();
            }

            // 受信再開
            stateObj.Socket.BeginReceive(stateObj.ReceiveBuff, 0, stateObj.ReceiveBuff.Length,
                SocketFlags.None, new AsyncCallback(ReceiveDataCallBack), stateObj);
        }
    }
}
