using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedBump
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        string tempName = Properties.Settings.Default.username;
        string tempPass = Properties.Settings.Default.password;
        string tempServer = Properties.Settings.Default.server;
        string tempDatabase = Properties.Settings.Default.database;
        public UserControl1()
        {
            InitializeComponent();

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            tempName = Properties.Settings.Default.username;
            tempPass = Properties.Settings.Default.password;
            tempServer = Properties.Settings.Default.server;
            tempDatabase = Properties.Settings.Default.database;
            this.Visibility = Visibility.Collapsed;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.username = tempName;
            Properties.Settings.Default.database = tempDatabase;
            Properties.Settings.Default.server = tempServer;
            Properties.Settings.Default.password = tempPass;
            this.Visibility = Visibility.Collapsed;
        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.username = tempName;
            Properties.Settings.Default.database = tempDatabase;
            Properties.Settings.Default.server = tempServer;
            Properties.Settings.Default.password = tempPass;
            this.Visibility = Visibility.Collapsed;
        }

        private void Rectangle_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.username = tempName;
            Properties.Settings.Default.database = tempDatabase;
            Properties.Settings.Default.server = tempServer;
            Properties.Settings.Default.password = tempPass;
            this.Visibility = Visibility.Collapsed;
        }
    }
}
