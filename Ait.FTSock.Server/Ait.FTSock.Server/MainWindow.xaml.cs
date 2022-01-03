using Ait.FTSock.Server.Class.Helpers;
using Ait.FTSock.Server.Class.Services;
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
        DirectoryService directoryService;
        FileService fileService;
        Socket serverSocket;
        IPEndPoint serverEndpoint;
        bool serverOnline = false;
        #endregion

        #region eventHandlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            directoryService = new DirectoryService();
            fileService = new FileService();
            StartupConfig();
        }
        private void BtnStartServer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStopServer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmbPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbIPs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion


        #region methods
        private void StartupConfig()
        {
            cmbIPs.ItemsSource = IPv4Helper.GetActiveIP4s();
            for (int port = 49200; port <= 49500; port++)
            {
                cmbPorts.Items.Add(port);
            }
            AppConfig.GetConfig(out string savedIP, out int savedPort);
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
            btnStartServer.Visibility = Visibility.Visible;
            btnStopServer.Visibility = Visibility.Hidden;
        }
        #endregion


    }
}
