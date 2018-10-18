#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Syncfusion.ThriftHive.Base;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                //Initializing the hive server connection
                HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);
				
				//To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                con.Open();

                //Creating the AdventureWorks_Person_Contact table in Hive Database
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();
                HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);

                //Fetches the data from Hive Database
                HqlDataReader reader = command.ExecuteReader();
                reader.FetchSize = int.MaxValue;
                HiveResultSet result = reader.FetchResult();

                //Binding the fetched result to the grid
                gridData1.ItemsSource = result;


                //closing the hive connection
                con.Close();
            }
            catch(HqlConnectionException)
            {
                if (MessageBox.Show("Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.", "Could not establish a connection to the HiveServer", MessageBoxButton.OK,MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Maximize(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Visible;
            btnMaximize.Visibility = Visibility.Collapsed;
            this.WindowState = WindowState.Maximized;
        }
        private void Restore(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Collapsed;
            btnMaximize.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                btnRestore.Visibility = Visibility.Visible;
                btnMaximize.Visibility = Visibility.Collapsed;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                btnRestore.Visibility = Visibility.Collapsed;
                btnMaximize.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
            }
        }
    }
}
