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
using Syncfusion.Windows.Tools.Controls;


namespace Iteration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HqlConnection con;
        HqlDataReader reader;
        HiveResultSet result;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //Initializing the hive server connection
                con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);
				
				//To initialize a Hive server connection with secured cluster
                //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

                //To initialize a Hive server connection with Azure cluster
                //HqlConnection con = new HqlConnection("<FQDN name of Azure cluster>", 8004, HiveServer.HiveServer2,"<username>","<password>");

                con.Open();

                //Query to create AdventureWorks_Person_Contact table in Hive Database
                HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
                createCommand.ExecuteNonQuery();

                //Creating query to fetch data from Hive Database
                HqlCommand command = new HqlCommand("Select * from AdventureWorks_Person_Contact", con);
                reader = command.ExecuteReader();

                //Assigning number of records to be fetched from Hive database
                reader.FetchSize = 500;

                //Fetches the result from the reader and store it in a object
                result = reader.FetchResult();

                //Closing the hive connection
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Creating new table
            TableAdv table = new TableAdv();
            table.BorderThickness = 0;
            //Creating a row
            TableRowAdv row = new TableRowAdv();
            //Creating cell
            TableCellAdv cell = new TableCellAdv();

            #region Adding the header text
            ParagraphAdv paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            SpanAdv span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "Contact Id";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);
          
            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "Full Name";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);
          
            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "Age";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);
          
            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "Email Address";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);
          
            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "Phone No";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);

            cell = new TableCellAdv();
            paragraph = new ParagraphAdv();
            paragraph.BeforeSpacing = 5;
            span = new SpanAdv();
            span.FontWeight = FontWeights.Bold;
            span.FontSize = 10;
            span.Text = "Modified Date";
            paragraph.Inlines.Add(span);
            cell.Blocks.Add(paragraph);
            row.Cells.Add(cell);
           
            table.Rows.Add(row);
            #endregion

            //Reading each rows from the fetched result
            foreach (HiveRecord rows in result)
            {
                //Creating new row
                row = new TableRowAdv();
               
                //Reading each data from the rows
                foreach (Object fields in rows)
                {
                    //Creating a Cell
                     cell = new TableCellAdv();

                    //Creating a paragraph
                    paragraph = new ParagraphAdv();
                    paragraph.BeforeSpacing = 5;
                    string records = fields.ToString();
                    span = new SpanAdv() { Text = records };
                    span.FontSize = 8;
                    paragraph.Inlines.Add(span);
                    
                    //Adding field value to cell
                    cell.Blocks.Add(paragraph);
                   
                    //Adding the cell to row
                    row.Cells.Add(cell);
                }

                //Adding the row to table
                table.Rows.Add(row);
            }

            //Adding  table to section
            richTextBox1.Document.Sections[0].Blocks.Add(table);
            richTextBox1.UpdateEditorLayout();
            
            //Adding Scrollbar to RichTextEditor
            richTextBox1.VerticalScrollBarVisibility = true;
            richTextBox1.HorizontalScrollBarVisibility = true;
              
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
            if(this.WindowState==WindowState.Maximized)
            {
                textBlock1.Width=1100;
                btnRestore.Visibility = Visibility.Visible;
                btnMaximize.Visibility = Visibility.Collapsed;
                this.WindowState = WindowState.Maximized;
            }
            else if(this.WindowState==WindowState.Normal)
            {
                textBlock1.Width=705;
                btnRestore.Visibility = Visibility.Collapsed;
                btnMaximize.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
            }
        }
    }
}
