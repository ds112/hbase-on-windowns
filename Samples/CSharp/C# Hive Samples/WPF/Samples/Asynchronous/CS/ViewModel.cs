#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Controls.Grid;
using System.Collections.ObjectModel;
using Syncfusion.Windows.Data;
using System.ComponentModel;
using Syncfusion.Windows.Shared;
using Syncfusion.ThriftHive.Base;
using System.Threading.Tasks;
using System.Windows;

namespace Asynchronous
{
    class ViewModel : NotificationObject
    {
        string connectionString = string.Empty;
        HqlDataReader reader = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            try
            {
                //Initialize hive server connection
                HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

                //To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                con.Open();

                //Creating AdventureWorks_Person_Contact table in Hive Database
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();
                
                //Fetching data from Hive Database
                HqlCommand command = new HqlCommand("select * from AdventureWorks_Person_Contact", con);
                reader = command.ExecuteReader();

                //Assigning number of rows to be fetched from Hive Database
                reader.FetchSize = 5000;
                this.DataReader = reader;
                
            }
            catch(HqlConnectionException)
            {
                if (MessageBox.Show("Could not establish a connection to the HiveServer. Please run HiveServer2 from the Syncfusion service manager dashboard.", "Could not establish a connection to the HiveServer", MessageBoxButton.OK,MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }
        internal Dictionary<int, HiveResultSet> ResultCollection = new Dictionary<int, HiveResultSet>();
        private HqlDataReader _DataReader;
        /// <summary>
        /// Gets or sets the order details.
        /// </summary>
        /// <value>The order details.</value>
        public HqlDataReader DataReader
        {
            get
            {
                return _DataReader;
            }
            set
            {
                _DataReader = value;
                RaisePropertyChanged("OrderDetails");
            }
        }

        private HiveResultSet _OrderDetails;
        /// <summary>
        /// Gets or sets the order details.
        /// </summary>
        /// <value>The order details.</value>
        public HiveResultSet OrderDetails
        {
            get
            {
                return _OrderDetails;
            }
            set
            {
                _OrderDetails = value;
                RaisePropertyChanged("OrderDetails");
            }
        }
    }
}