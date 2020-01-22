using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class AsynchronousServer
    {
        private static ManualResetEvent SocketEvent = new ManualResetEvent(false);
        private static IPEndPoint ipEndPoint;
        private static Socket soc;
        private static string port = "2001";
        private static Thread tMain;

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
            try
            {
                Console.WriteLine("Main ThreadID:" + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Program ThreadID:" + Thread.CurrentThread.ManagedThreadId);
                IPAddress myIP = IPAddress.Parse("127.0.0.1");
                ipEndPoint = new IPEndPoint(myIP, Int32.Parse(port));
                Console.WriteLine("Init ThreadID:" + Thread.CurrentThread.ManagedThreadId);
                soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                soc.Bind(ipEndPoint);
                soc.Listen(10);
                tMain = new Thread(new ThreadStart(Round));
                tMain.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error");
            }
            
        }

        private static void Round()
        {
            Console.WriteLine("Round ThreadID:" + Thread.CurrentThread.ManagedThreadId);
            while (true)
            {
                SocketEvent.Reset();
                soc.BeginAccept(new AsyncCallback(StartReceive),soc);
                SocketEvent.WaitOne();
            }
        }

        private static void StartReceive(IAsyncResult ar)
        {
            // BeginReceiveで非同期受信を開始する
            Console.WriteLine("StartReceive ThreadID:" + Thread.CurrentThread.ManagedThreadId);
            AsyncStateObject stateObj = new AsyncStateObject(soc);
            soc.BeginReceive(stateObj.ReceiveBuff, 0, stateObj.ReceiveBuff.Length, SocketFlags.None, new AsyncCallback(ReceiveDataCallBack), stateObj);
        }

        private static void ReceiveDataCallBack(IAsyncResult ar)
        {
            // 状態オブジェクト取得
            Console.WriteLine("ReceiveDataCallback ThreadID:" + Thread.CurrentThread.ManagedThreadId);
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
