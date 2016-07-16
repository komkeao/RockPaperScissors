using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    public partial class Game : Page
    {
        private String name = null; //for Game
        private int type; //Client /Server
        private Socket server;
        private bool exitFlag=true;
        private String enemyName;
        public Game(int type, String name, String enemyName, Socket server)

        {//Type 1 == SERVER 2==CLINET

            InitializeComponent();
            this.server = server;
            this.type = type;
            textBlockEnemyName.Text = enemyName;
            textBoxMyName.Text = name;
            this.name = name;
            this.enemyName = enemyName;

        }
        private string getLocalIpAddress()
        {
            IPHostEntry host;
            string localIP = null;
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
        private void checkProgress() // ProgressBar
        {
            if (myEnegy.Value == 0 || enemyEnegy.Value == 0)
            {
                if (enemyEnegy.Value == 0)
                {
                    MessageBox.Show("คุณชนะแล้ว!!!!", "คุณชนะแล้ว");
                }
                else {
                    MessageBox.Show("คุณแพ้แล้ว!!!!", "คุณแพ้แล้ว");
                }
                myEnegy.Value = 100;
                enemyEnegy.Value = 100;
            }
        }
        private void getResult(String my, String enemy)
        {
            if (my == "1" && enemy == "1")
            {
                textResult.Text = "เสมอ";

            }
            else if (my == "1" && enemy == "2")
            {
                enemyEnegy.Value -= 20;
                textResult.Text = "ชนะ";
                checkProgress();

            }
            else if (my == "1" && enemy == "3")
            {
                myEnegy.Value -= 20;
                textResult.Text = "แพ้";
                checkProgress();

            }
            else if (my == "2" && enemy == "1")
            {
                myEnegy.Value -= 20;
                textResult.Text = "แพ้";
                checkProgress();

            }
            else if (my == "2" && enemy == "2")
            {
                textResult.Text = "เสมอ";
            }
            else if (my == "2" && enemy == "3")
            {
                enemyEnegy.Value -= 20;
                textResult.Text = "ชนะ";
                checkProgress();

            }
            else if (my == "3" && enemy == "1")
            {
                enemyEnegy.Value -= 20;
                textResult.Text = "ชนะ";
                checkProgress();
            }
            else if (my == "3" && enemy == "2")
            {
                myEnegy.Value -= 20;
                textResult.Text = "แพ้";
                checkProgress();
            }
            else if (my == "3" && enemy == "3")
            {
                textResult.Text = "เสมอ";
                checkProgress();
            }
            else {
                exitFlag = false;
                MessageBox.Show(enemyName + " ได้ออกจากเกมแล้ว");
                Application.Current.Shutdown();
            }
            Rock.Visibility = Visibility.Visible;
            Paper.Visibility = Visibility.Visible;
            Sca.Visibility = Visibility.Visible;
        }
        private void pickPicture(int pick)
        {
            ansRock1.Visibility = Visibility.Hidden;
            ansSca1.Visibility = Visibility.Hidden;
            ansPaper1.Visibility = Visibility.Hidden;
            if (pick == 1)
            {

                ansRock1.Visibility = Visibility.Visible;
            }
            else if (pick == 2)
            {
                ansSca1.Visibility = Visibility.Visible;
            }
            else if (pick == 3)
            {
                ansPaper1.Visibility = Visibility.Visible;
            }
        }
        private void servdPicture(String pick)
        {
            ansRock2.Visibility = Visibility.Hidden;
            ansSca2.Visibility = Visibility.Hidden;
            ansPaper2.Visibility = Visibility.Hidden;
            if (pick == "1")
            {
                ansRock2.Visibility = Visibility.Visible;
            }
            else if (pick == "2")
            {
                ansSca2.Visibility = Visibility.Visible;
            }
            else if (pick == "3")
            {
                ansPaper2.Visibility = Visibility.Visible;
            }
            else {

            }
        }
        private void ansSendReceive(String myAns) //Game Play
        {
            Rock.Visibility = Visibility.Collapsed;
            Paper.Visibility = Visibility.Collapsed;
            Sca.Visibility = Visibility.Collapsed;
            pickPicture(Int32.Parse(myAns));
            Thread childSocketThread;
            if (type == 1)
            {
                childSocketThread = new Thread(() =>
                {
                    String res = null;
                    try
                    {
                        server.Send(Encoding.UTF8.GetBytes(myAns));
                        byte[] data = new byte[1024];
                        int receivedDataLength = server.Receive(data);
                        res = Encoding.UTF8.GetString(data, 0, receivedDataLength);
 
                    }
                    catch
                    {
                        res = "";
                    }
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                     
                        servdPicture(res);
                        getResult(myAns, res);
                    }));
                });
            }
            else {
                childSocketThread = new Thread(() =>
                {
                    string res;
                    try
                    {
                        byte[] data = new byte[1024];
                        data = new byte[1024];
                        int receivedDataLength = server.Receive(data);
                        res = Encoding.UTF8.GetString(data, 0, receivedDataLength);
                        data = Encoding.UTF8.GetBytes(myAns);
                        server.Send(data, data.Length, SocketFlags.None);
                    }
                    catch {
                        res = "";
                    }
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        if (res == null)
                        {
                            
                        }
                        servdPicture(res);
                        getResult(myAns, res);
                    }));

                });
            }
            childSocketThread.SetApartmentState(ApartmentState.STA);
            childSocketThread.IsBackground = true;
            childSocketThread.Start();
        }
        private void ansRock_MouseDown(object sender, MouseButtonEventArgs e)
        { 
            ansSendReceive("1");
        }
        private void ansSca_MouseDown(object sender, MouseButtonEventArgs e)
        {
        ansSendReceive("2");
         }
        private void ansPaper_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ansSendReceive("3");
        }
        ~Game() {
            if (exitFlag==true) {
                byte[] data = new byte[1024];
                data = new byte[1024];
                data = Encoding.UTF8.GetBytes("exit");
                server.Send(data, data.Length, SocketFlags.None);
            }  
        }
    }
}
