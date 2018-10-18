using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIHBase
{
    class Program
    {
        //  public static string userName = ""; // For Secure (add domain with user name e.g. syncbigdata@SYNCTEST.COM) & Azure Cluster (e.g. admin)
        //  public static string password = ""; // For Secure & Azure Cluster
        static void Main(string[] args)
        {
            try
            {
                // For Cluster aasign host="<ipaddress/hostname>";
                // For Azure cluster assign host="<FQDN Name of Azure cluster>"
                // Use FQN(host name with domain name) for secure cluster
                string host = "localhost";  

                int port = 10005; // For Azure cluster assign port =8006
                int i;
                do
                {
                    Console.WriteLine("\nEnter any operation");
                    Console.WriteLine("1. NewTable");
                    Console.WriteLine("2. DeleteTable");
                    Console.WriteLine("3. InsertValue");
                    Console.WriteLine("4. GetSingleRow");
                    Console.WriteLine("5. ScanTable");
                    Console.WriteLine("6. Exit");
                    i = int.Parse(Console.ReadLine());
                    switch (i)
                    {
                        case 1:     // Create a new table
                            if (HBaseOperation.IsTableExists("http://" + host + ":" + port + "/", "Customer")) // Add UserName & Password for secure/ Azure Cluster.
                                HBaseOperation.DeleteTable("http://" + host + ":" + port + "/", "Customer");
                            Console.WriteLine("Creating Table....");
                            HBaseOperation.CreateTable("http://" + host + ":" + port + "/", "Customer", "Info");
                            //// For Azure basic Authentication cluster - HBaseOperation.CreateTable("https://" + host + ":" + port + "/", "Customer", "Info", userName, password);
                            //// For Secured Cluster - HBaseOperation.CreateTable("https://" + host + ":" + port + "/", "Customer", "Info", userName, password);
                            Console.WriteLine("Table Created");
                            break;
                        case 2:     // Delete the table
                            if (!HBaseOperation.IsTableExists("http://" + host + ":" + port + "/", "Customer")) // Add UserName & Password for secure/ Azure Cluster.
                                HBaseOperation.CreateTable("http://" + host + ":" + port + "/", "Customer", "Info");
                            Console.WriteLine("Deleting Table....");
                            HBaseOperation.DeleteTable("http://" + host + ":" + port + "/", "Customer");
                            ////For Azure basic Authentication cluster -  HBaseOperation.DeleteTable("https://" + host + ":" + port + "/", "Customer",userName, password); 
                            ////For Secured Cluser -  HBaseOperation.DeleteTable("https://" + host + ":" + port + "/", "Customer", userName , password); 
                            Console.WriteLine("Table Deleted");
                            break;
                        case 3:     // Insert the row values in the table
                            Console.WriteLine("Inserting values into the table....");
                            PopulateTable(host, port);
                            Console.WriteLine("Value Inserted");
                            break;
                        case 4:     // Get the single row from the table
                            Console.WriteLine("Fetching single row from the table....");
                            PopulateTable(host, port);
                            Console.WriteLine(HBaseOperation.GetRow("http://" + host + ":" + port + "/", "Customer", "FRANR"));
                            ////For Azure basic Authentication cluster -  HBaseOperation.GetRow("https://" + host + ":" + port + "/", "Customer", "FRANR",userName, password);
                            ////For Secured Cluster - HBaseOperation.GetRow("https://" + host + ":" + port + "/", "Customer", "FRANR", userName, password); 
                            break;
                        case 5:     // Scan the table
                            Console.WriteLine("Fetching all the rows from the table....");
                            PopulateTable(host, port);
                            Console.WriteLine(HBaseOperation.ScanTable("http://" + host + ":" + port + "/", "Customer"));
                            ////For Azure basic Authentication cluster - > HBaseOperation.ScanTable("https://" + host + ":" + port + "/", "Customer", userName, password); 
                            ////For Secured Cluster - >HBaseOperation.ScanTable("https://" + host + ":" + port + "/", "Customer", userName, password); 
                            break;
                        case 6:
                            break;
                        default:
                            Console.WriteLine("Enter valid Option");
                            break;
                    }
                } while (i != 6);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Create a new table and insert the value on it    
        /// </summary>
        /// <param name="host">Host name</param>
        /// <param name="port">Port number</param>
        private static void PopulateTable(string host, int port)
        {
            if (HBaseOperation.IsTableExists("http://" + host + ":" + port + "/", "Customer"))
                HBaseOperation.DeleteTable("http://" + host + ":" + port + "/", "Customer");
            HBaseOperation.CreateTable("http://" + host + ":" + port + "/", "Customer", "Info");
            ////Row 1
            HbaseTableInsertRow(host, port, "ALFKI", "CompanyName", "Alfreds Futterkiste");
            HbaseTableInsertRow(host, port, "ALFKI", "contactName", "Maria Anders");
            HbaseTableInsertRow(host, port, "ALFKI", "contactTIT", "Sales Representative");
            HbaseTableInsertRow(host, port, "ALFKI", "country", "Germany");
            ////Row 2
            HbaseTableInsertRow(host, port, "DRACD", "CompanyName", "Drachenblut Delikatessen");
            HbaseTableInsertRow(host, port, "DRACD", "contactName", "Sven Ottlieb");
            HbaseTableInsertRow(host, port, "DRACD", "contactTIT", "Order Administrator");
            HbaseTableInsertRow(host, port, "DRACD", "country", "Germany");
            ////Row 3
            HbaseTableInsertRow(host, port, "BONAP", "CompanyName", "Bon app");
            HbaseTableInsertRow(host, port, "BONAP", "contactName", "Laurence Lebihan");
            HbaseTableInsertRow(host, port, "BONAP", "contactTIT", "Owner");
            HbaseTableInsertRow(host, port, "BONAP", "country", "France");
            ////Row 4
            HbaseTableInsertRow(host, port, "BOTTM", "CompanyName", "Bottom-Dollar Markets");
            HbaseTableInsertRow(host, port, "BOTTM", "contactName", "Elizabeth Lincoln");
            HbaseTableInsertRow(host, port, "BOTTM", "contactTIT", "Accounting Manager");
            HbaseTableInsertRow(host, port, "BOTTM", "country", "Canada");
            ////Row 5
            HbaseTableInsertRow(host, port, "FRANR", "CompanyName", "France restauration");
            HbaseTableInsertRow(host, port, "FRANR", "contactName", "Carine Schmitt");
            HbaseTableInsertRow(host, port, "FRANR", "contactTIT", "Marketing Manager");
            HbaseTableInsertRow(host, port, "FRANR", "country", "Austria");
         }

        /// <summary>
        /// Insert rows values in HBase table
        /// </summary>
        /// <param name="host">host name of running HBase rest server</param>
        /// <param name="port">port number</param>
        /// <param name="rowKey">HBase table row value</param>
        /// <param name="columnFamily">column name</param>
        /// <param name="cellValue">cell value</param>
        private static void HbaseTableInsertRow(string host, int port, string rowKey, string columnFamily, string cellValue)
        {
            ////For Azure basic Authentication cluster -> HBaseOperation.InsertRow("https://" + host + ":" + port + "/", rowKey,"Customer" ,"Info", columnFamily, cellValue, userName, password);
            ////For Secured Cluster -> HBaseOperation.InsertRow("https://" + host + ":" + port + "/", rowKey,"Customer" ,"Info", columnFamily, cellValue, userName, password); 
            HBaseOperation.InsertRow("http://" + host + ":" + port + "/", rowKey, "Customer", "Info", columnFamily, cellValue);
        }
    }
}
