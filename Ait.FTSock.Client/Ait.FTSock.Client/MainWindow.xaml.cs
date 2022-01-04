using Ait.FTSock.Client.Class.Helpers;
using Ait.FTSock.Client.Class.Services;
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

namespace Ait.FTSock.Client
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
        FtFileService ftFileService;
        FTFolderService FTFolderService;
        Socket serverSocket;
        IPEndPoint serverEndpoint;
        #endregion
        private void cmbIPs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveConfig();

        }

        private void CmbPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveConfig();

        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ftFileService = new FtFileService();
            FTFolderService = new FTFolderService();
            StartupConfig();
        }

        private void StartupConfig()
        {
            cmbIPs.ItemsSource = IPv4Helper.GetActiveIP4s();
            for (int port = 49200; port <= 49500; port++)
            {
                cmbPorts.Items.Add(port);
            }
            AppConfig.GetConfig(out string savedIP, out int savedPort, out string savedUsername);
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
                txtUsername.Text = savedUsername;
            }
            catch
            {
                txtUsername.Text = "";
            }

        }

        private void SaveConfig()
        {
            if (cmbIPs.SelectedItem == null) return;
            if (cmbPorts.SelectedItem == null) return;
            if (txtUsername.Text == null) return;
            string ip = cmbIPs.SelectedItem.ToString();
            int port = int.Parse(cmbPorts.SelectedItem.ToString());
            string username = txtUsername.Text;
            AppConfig.WriteConfig(ip, port, username);
        }

        private void txtUsername_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SaveConfig();

        }
    }
}
