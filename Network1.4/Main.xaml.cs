using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace Network1._4
{
    public partial class Main : Page
    {
        string   username;
        private int sta = 1, ipStatus = 1; //Status For  Recieve Chat
        private DispatcherTimer receiveChatTimer = new DispatcherTimer(); //เวลา
        private DispatcherTimer clearePlayerTimer = new DispatcherTimer();
        private DispatcherTimer broadcastIPTimer = new DispatcherTimer();
        private Thread childSocketThread;
        private IPEndPoint ipc;
        private Socket socket;
        private Socket client;
        private Thread requireStartThread;
        private IPEndPoint ip;
        private Socket server;
        public Main(string username)
        {
            InitializeComponent();
            this.username = username;
            listBoxChat.Items.Add("สวัสดี " + username + " ขอให้สนุกนะ!!");
            receiveChatTimer.Tick += receiveChat; //เลือกเมดตอด
            receiveChatTimer.Interval = new TimeSpan(0, 0, 0, 0, 1); //จำนวนที่ให้รอ
            receiveChatTimer.Start();
            serverStart();
            clearePlayerTimer.Tick += clearPlayer;
            clearePlayerTimer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            clearePlayerTimer.Start();
            sendBroadcast(username + " เข้าสู่ระบบแล้ว");
            broadcastIPTimer.Tick += broadcastIP;
            broadcastIPTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            broadcastIPTimer.Start();
        }
        private string getLocalIpAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        private void clearPlayer(object sender, object e)
        {
            listBoxPlayer.Items.Clear();
        }
        private void broadcastIP(object sender, object e)
        {
            sendBroadcast(":" + username + "@" + getLocalIpAddress());
        }
        private static void sendBroadcast(string a)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);
            byte[] bytes = Encoding.UTF8.GetBytes(a);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
        private void receiveChat(object sender, object e)
        {
            if (sta == 1)
            {
                childSocketThread = new Thread(() =>
                {

                    sta = 0;
                    Socket sock = new Socket(AddressFamily.InterNetwork,
                               SocketType.Dgram, ProtocolType.Udp);
                    IPEndPoint iep = new IPEndPoint(IPAddress.Any, 15000);
                    sock.Bind(iep);
                    EndPoint ep = (EndPoint)iep;
                    byte[] data = new byte[1024];
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string message = Encoding.UTF8.GetString(data, 0, recv);
                    sock.Close();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        sta = 1;
                        if (message.StartsWith(":"))
                        {
                            message = message.Replace(":", null);
                            int count, sum = 0;
                            int.TryParse(listBoxPlayer.Items.Count.ToString(), out count);
                            for (int i = 0; i < count; i++)
                            {
                                if (message == listBoxPlayer.Items.GetItemAt(i).ToString())
                                {
                                    sum++;
                                }
                            }
                            if (sum == 0 && !message.EndsWith(getLocalIpAddress()) && ipStatus == 1)
                            {
                                listBoxPlayer.Items.Add(message);
                            }
                        }
                        else
                        {
                            listBoxChat.Items.Add(message);
                            if (VisualTreeHelper.GetChildrenCount(listBoxChat) > 0)
                            {
                                Border border = (Border)VisualTreeHelper.GetChild(listBoxChat, 0);
                                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                                scrollViewer.ScrollToBottom();
                            }
                        }
                    }));
                });
                childSocketThread.SetApartmentState(ApartmentState.STA);
                childSocketThread.IsBackground = true;
                childSocketThread.Start();
            }
        }
        private void requireStart(Socket server, string enemyUsername)
        {

            requireStartThread = new Thread(() =>
            {

                this.Dispatcher.Invoke((Action)(() =>
                {
                    ipStatus = 0;
                    listBoxPlayer.Items.Clear();
                    listBoxPlayer.Items.Add("Wait For Response");
                }));
                string res = null;
                server.Connect(ip);
                try
                {
                    byte[] data = new byte[1024];
                    int receivedDataLength = server.Receive(data);
                    res = Encoding.UTF8.GetString(data, 0, receivedDataLength);
                }
                catch
                {
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (res == "1")
                    {
                        this.NavigationService.Navigate(new Game(2, username, enemyUsername, server));
                        receiveChatTimer.Stop();
                        clearePlayerTimer.Stop();
                        broadcastIPTimer.Stop();
                    }
                    else {

                        listBoxPlayer.Items.Clear();
                        ipStatus = 1;
                    }
                }));
            });
            requireStartThread.SetApartmentState(ApartmentState.STA);
            requireStartThread.IsBackground = true;
            requireStartThread.Start();
        }
        private void serverStart()
        {
            var childSocketThread = new Thread(() =>
            {
                ipc = new IPEndPoint(IPAddress.Any, 15000);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipc);
                socket.Listen(10);
                client = socket.Accept();
                IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
                this.Dispatcher.Invoke((Action)(() =>
                {
                    int count;
                    string enemyName = null; ;
                    IPEndPoint remoteIpEndPoint = client.RemoteEndPoint as IPEndPoint;
                    string ip = remoteIpEndPoint.Address.ToString();
                    int.TryParse(listBoxPlayer.Items.Count.ToString(), out count);
                    for (int i = 0; i < count; i++)
                    {
                        if (listBoxPlayer.Items.GetItemAt(i).ToString().EndsWith(ip))
                        {
                            enemyName = listBoxPlayer.Items.GetItemAt(i).ToString().Split('@')[0].ToString();
                        }
                    }
                    MessageBoxResult ans = MessageBox.Show(enemyName + " ต้องการเล่นเกมกับคุณ จะเล่นด้วยหรือไม่", enemyName + " ต้องการเล่นเกมกับคุณ", MessageBoxButton.OKCancel);
                    if (ans == MessageBoxResult.OK)
                    {
                        client.Send(Encoding.UTF8.GetBytes("1"));
                        this.NavigationService.Navigate(new Game(1, username, enemyName, client));
                        receiveChatTimer.Stop();
                        clearePlayerTimer.Stop();
                        broadcastIPTimer.Stop();
                        socket.Close();
                    }
                    else {
                        client.Send(Encoding.UTF8.GetBytes("0"));
                        socket.Close();
                        restartServer();
                    }
                }));
            });
            childSocketThread.SetApartmentState(ApartmentState.STA);
            childSocketThread.IsBackground = true;
            childSocketThread.Start();
        }
        private void restartServer()
        {
            serverStart();
        }
        private void btnSend(object sender, RoutedEventArgs e)
        {
            string text = username + " : " + textBoxChat.Text;
            broadcastIPTimer.Stop();
            sendBroadcast(text);
            broadcastIPTimer.Start();
            textBoxChat.Text = null;
        }
        private void listBoxPlayer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(listBoxPlayer, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                string ipc = item.ToString();
                string enemyUsername;
                ipc = ipc.Replace("System.Windows.Controls.ListBoxItem:", null);
                enemyUsername = ipc.Split('@')[0].ToString();
                ipc = ipc.Split('@')[1].ToString();
                ip = new IPEndPoint(IPAddress.Parse(ipc), 15000);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                requireStart(server, enemyUsername);
            }
        }
        private void textBoxChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string text = username + " : " + textBoxChat.Text;
                broadcastIPTimer.Stop();
                sendBroadcast(text);
                broadcastIPTimer.Start();
                textBoxChat.Text = null;
            }
        }
        private void textBoxChat_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBoxChat.Text = null;
        }
    }
}
