using Ait.FTSock.Server.Class.Entities;
using Ait.FTSock.Server.Class.Helpers;
using Ait.FTSock.Server.Class.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

namespace Ait.FTSock.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Global variables
        FTFolderService directoryService;
        FtFileService fileService;
        Socket serverSocket;
        IPEndPoint serverEndpoint;
        bool serverOnline = false;
        #endregion

        #region Event Handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtPath.Text = "C:\\Howest";
            directoryService = new FTFolderService();
            fileService = new FtFileService();
            StartupConfig();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnStopServer_Click(null, null);
        }
        private void CmbIPs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveConfig();
        }
        private void CmbPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveConfig();
        }
        private void txtPath_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SaveConfig();
        }
        private void BtnStartServer_Click(object sender, RoutedEventArgs e)
        {
            btnStartServer.Visibility = Visibility.Hidden;
            btnStopServer.Visibility = Visibility.Visible;
            cmbIPs.IsEnabled = false;
            cmbPorts.IsEnabled = false;
            txtPath.IsEnabled = false;
            grdUsers.ItemsSource = null;
            DisplayData();

            StartTheServer();
            StartListening();

        }
        private void BtnStopServer_Click(object sender, RoutedEventArgs e)
        {
            btnStartServer.Visibility = Visibility.Visible;
            btnStopServer.Visibility = Visibility.Hidden;
            cmbIPs.IsEnabled = true;
            cmbPorts.IsEnabled = true;
            txtPath.IsEnabled = true;

            CloseTheServer();
        }
        #endregion

        #region Methods
        private void DisplayData()
        {
            grdUsers.ItemsSource = null;
            grdUsers.ItemsSource = fileService.Files;
        }
        private void StartTheServer()
        {
            serverOnline = true;
        }
        private void CloseTheServer()
        {
            serverOnline = false;
            try
            {
                if (serverSocket != null)
                    serverSocket.Close();
            }
            catch
            { }
            serverSocket = null;
            serverEndpoint = null;
        }
        private void StartListening()
        {
            IPAddress ip = IPAddress.Parse(cmbIPs.SelectedItem.ToString());
            int port = int.Parse(cmbPorts.SelectedItem.ToString());
            serverEndpoint = new IPEndPoint(ip, port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(serverEndpoint);
                serverSocket.Listen(int.MaxValue);
                while (serverOnline)
                {
                    DoEvents();
                    if (serverSocket != null)
                    {
                        if (serverSocket.Poll(200000, SelectMode.SelectRead))
                        {
                            HandleClientCall(serverSocket.Accept());
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void HandleClientCall(Socket clientCall)
        {
            byte[] clientRequest = new Byte[1024];
            string instruction = null;

            while (true)
            {
                int numByte = clientCall.Receive(clientRequest);
                instruction += Encoding.ASCII.GetString(clientRequest, 0, numByte);
                if (instruction.IndexOf("##EOM") > -1)
                    break;
            }
            string serverResponseInText = ProcessClientCall(instruction);
            if (serverResponseInText != "")
            {
                byte[] serverResponse = Encoding.ASCII.GetBytes(serverResponseInText);
                clientCall.Send(serverResponse);
            }
            clientCall.Shutdown(SocketShutdown.Both);
            clientCall.Close();
        }
        private string ProcessClientCall(string instruction)
        {
            string[] parts;
            string returnValue = "";

            instruction = instruction.Replace("##EOM", "").Trim().ToUpper();

            if (instruction.Length > 6 && instruction.Substring(0, 6) == "GETALL")
            {
                returnValue = SerializeList();
                return returnValue + "##EOM";
            }
            else if (instruction.Length > 3 && instruction.Substring(0, 3) == "GET")
            {
                parts = instruction.Split('|');
                if (parts.Length != 2)
                    return "Sorry ... I don't understand you ...##EOM";
                foreach (FTFolder fTFolder in directoryService.Directories)
                {
                    if (fTFolder.Name == parts[1])
                    {
                        returnValue = SerializeObject(fTFolder);
                        break;
                    }
                }
                if (returnValue == "")
                {
                    return "Sorry ... file unknown ...##EOM";
                }
                else
                {
                    return returnValue + "##EOM";
                }

            }
            else if (instruction.Length > 3 && instruction.Substring(0, 3) == "PUT")
            {
                parts = instruction.Split('|');
                if (parts.Length != 2)
                    return "Sorry ... I don't understand you ...##EOM";

                FTFolder fTFolder = JsonConvert.DeserializeObject<FTFolder>(parts[1]);
                directoryService.AddPath(fTFolder);
                returnValue = SerializeList();
                DisplayData();
                return returnValue + "##EOM";
            }
            return "Sorry ... I don't understand you ...##EOM";

        }
        private string SerializeObject(FTFolder folder)
        {
            return JsonConvert.SerializeObject(folder);
        }
        private string SerializeList()
        {
            return JsonConvert.SerializeObject(directoryService.Directories);
        }
        private static void DoEvents()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
            catch (Exception fout)
            {
                System.Windows.Application.Current.Dispatcher.DisableProcessing();
            }
        }
        private void StartupConfig()
        {
            cmbIPs.ItemsSource = IPv4Helper.GetActiveIP4s();
            for (int port = 49200; port <= 49500; port++)
            {
                cmbPorts.Items.Add(port);
            }
            AppConfig.GetConfig(out string savedIP, out int savedPort,out string savedPath );
            try
            {
                cmbIPs.SelectedItem = savedIP;
            }
            catch
            {
                cmbIPs.SelectedItem = "127.0.0.1";
            }
            try
            {
                cmbPorts.SelectedItem = savedPort;
            }
            catch
            {
                cmbPorts.SelectedItem = 49200;
            }
            try
            {
                txtPath.Text = savedPath;
            }
            catch
            {
                txtPath.Text = "C:\\Howest";
            }
            btnStartServer.Visibility = Visibility.Visible;
            btnStopServer.Visibility = Visibility.Hidden;
        }
        private void SaveConfig()
        {
            if (cmbIPs.SelectedItem == null) return;
            if (cmbPorts.SelectedItem == null) return;
            if (txtPath.Text == null) return;
            string ip = cmbIPs.SelectedItem.ToString();
            int port = int.Parse(cmbPorts.SelectedItem.ToString());
            string path = txtPath.Text;
            AppConfig.WriteConfig(ip, port,path);
        }
        #endregion


    }
}
