namespace MVCSampleBrowser.Models
{
    using Syncfusion.ThriftHBase.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    public class FilterData
    {
        #region fields
        public static List<List<Customers>> filterResults = new List<List<Customers>>();
        public static List<Customers> results = new List<Customers>();        
        #endregion fields

        #region methods
        public static List<List<Customers>> list(String path)
        {

            #region creating connection

            HBaseConnection con = new HBaseConnection("localhost", 10003);
            con.Open();

            #endregion creating connection

            #region parsing csv input file

            csv csvObj = new csv();
            object[,] cells;
            cells = null;

            cells = csvObj.Table(path, false, ',');

            #endregion parsing csv input file

            #region creating table
            String tableName = "AdventureWorks_Person_Contact";
            List<string> columnFamilies = new List<string>();
            columnFamilies.Add("info");
            columnFamilies.Add("contact");
            columnFamilies.Add("others");
            if (!HBaseOperation.IsTableExists(tableName, con))
            {
                if (columnFamilies.Count > 0)
                {
                    HBaseOperation.CreateTable(tableName, columnFamilies, con);
                }
                else
                {
                    throw new HBaseException("ERROR: Table must have at least one column family");
                }
            }

            # endregion

            #region Inserting Values
            string[] column = new string[] { "CONTACTID", "FULLNAME", "AGE", "EMAILID", "PHONE", "MODIFIEDDATE" };
            Dictionary<string, IList<HMutation>> rowCollection = new Dictionary<string, IList<HMutation>>();
            string rowKey;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                List<HMutation> mutations = new List<HMutation>();
                rowKey = cells[i, 0].ToString();
                for (int j = 1; j < column.Length; j++)
                {
                    HMutation mutation = new HMutation();
                    mutation.ColumnFamily = j < 3 ? "info" : j < 5 ? "contact" : "others";
                    mutation.ColumnName = column[j];
                    mutation.Value = cells[i, j].ToString();
                    mutations.Add(mutation);
                }
                rowCollection[rowKey] = mutations;
            }
            HBaseOperation.InsertRows(tableName, rowCollection, con);
            #endregion Inserting Values

            #region scan values
            HBaseOperation.FetchSize = 100;
            var rowFilter = new FilterString().RowFilter(CompareOperator.Equal, Comparator.BinaryPrefix("1002")).Query();
            HBaseResultSet rowFilterResult = HBaseOperation.ScanTable(tableName, rowFilter, con);
            filterResults.Add(GetCollection(rowFilterResult));
            HBaseOperation.FetchSize = 100;
            var singlecolumnvalue = new FilterString().SingleColumnValueFilter("info", "FULLNAME", CompareOperator.Equal, Comparator.BinaryPrefix("Katherine")).Query();
            HBaseResultSet singleColumnFilterResult = HBaseOperation.ScanTable(tableName, singlecolumnvalue, con);
            filterResults.Add(GetCollection(singleColumnFilterResult));
            #endregion scan values

            con.Close();
            return filterResults;
        }

        private static List<Customers> GetCollection(HBaseResultSet table)
        {
            results = new List<Customers>();

            foreach (HBaseRecord cellValue in table.ToList())
            {

                Customers customer = new Customers();
                customer.ContactId = cellValue["rowKey"] != null ? cellValue["rowKey"].ToString() : "";
                customer.FullName = cellValue["info:FULLNAME"] != null ? cellValue["info:FULLNAME"].ToString() : "";
                customer.Age = cellValue["info:AGE"] != null ? cellValue["info:AGE"].ToString() : "";
                customer.EmailId = cellValue["contact:EMAILID"] != null ? cellValue["contact:EMAILID"].ToString() : "";
                customer.PhoneNumber = cellValue["contact:PHONE"] != null ? cellValue["contact:PHONE"].ToString() : "";
                customer.ModifiedDate = cellValue["others:MODIFIEDDATE"] != null ? cellValue["others:MODIFIEDDATE"].ToString() : "";
                results.Add(customer);
            }
            return results;
        }
        #endregion methods
    }


    public class CustomersData
    {
        public static List<Customers> list(String path)
        {
            List<Customers> results = new List<Customers>();

            #region creating connection

            HBaseConnection con = new HBaseConnection("localhost", 10003);
            con.Open();

            #endregion creating connection

            #region parsing csv input file

            csv csvObj = new csv();
            object[,] cells;
            cells = null;

            cells = csvObj.Table(path, false, ',');

            #endregion parsing csv input file

            #region creating table
            String tableName = "AdventureWorks_Person_Contact";
            List<string> columnFamilies = new List<string>();
            columnFamilies.Add("info");
            columnFamilies.Add("contact");
            columnFamilies.Add("others");
            if (!HBaseOperation.IsTableExists(tableName, con))
            {
                if (columnFamilies.Count > 0)
                {
                    HBaseOperation.CreateTable(tableName, columnFamilies, con);
                }
                else
                {
                    throw new HBaseException("ERROR: Table must have at least one column family");
                }
            }
          
            # endregion

            #region Inserting Values
            string[] column = new string[] { "CONTACTID", "FULLNAME", "AGE", "EMAILID", "PHONE", "MODIFIEDDATE" };
            Dictionary<string, IList<HMutation>> rowCollection = new Dictionary<string, IList<HMutation>>();
            string rowKey;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                List<HMutation> mutations = new List<HMutation>();
                rowKey = cells[i, 0].ToString();
                for (int j = 1; j < column.Length; j++)
                {
                    HMutation mutation = new HMutation();
                    mutation.ColumnFamily = j < 3 ? "info" : j < 5 ? "contact" : "others";
                    mutation.ColumnName = column[j];
                    mutation.Value = cells[i, j].ToString();
                    mutations.Add(mutation);
                }
                rowCollection[rowKey] = mutations;
            }
            HBaseOperation.InsertRows(tableName, rowCollection, con);
            #endregion Inserting Values

            #region scan values

            HBaseOperation.FetchSize = 100;
            HBaseResultSet table = HBaseOperation.ScanTable(tableName, con);
        
            foreach (HBaseRecord cellValue in table.ToList())
            {
                Customers customer = new Customers();
                customer.ContactId = cellValue["rowKey"] != null ? cellValue["rowKey"].ToString() : "";
                customer.FullName = cellValue["info:FULLNAME"] != null ? cellValue["info:FULLNAME"].ToString() : "";
                customer.Age = cellValue["info:AGE"] != null ? cellValue["info:AGE"].ToString() : "";
                customer.EmailId = cellValue["contact:EMAILID"] != null ? cellValue["contact:EMAILID"].ToString() : "";
                customer.PhoneNumber = cellValue["contact:PHONE"] != null ? cellValue["contact:PHONE"].ToString() : "";
                customer.ModifiedDate = cellValue["others:MODIFIEDDATE"] != null ? cellValue["others:MODIFIEDDATE"].ToString() : "";
                results.Add(customer);
            }

            #endregion scan values

            con.Close();
            return results;
        }
    }

    public partial class StronglyBindedCustomersData
    {
        public BindingList<Customers> list(string path)
        {
            #region creating connection

            HBaseConnection con = new HBaseConnection("localhost", 10003);
            con.Open();

            #endregion creating connection

            #region parsing csv input file

            csv csvObj = new csv();
            object[,] cells;
            cells = null;
            cells = csvObj.Table(path,false, ',');

            #endregion parsing csv input file

            #region creating table
            List<Customers> results = new List<Customers>();
            String tableName = "AdventureWorks_Person_Contact";
            List<string> columnFamilies = new List<string>();
            columnFamilies.Add("info");
            columnFamilies.Add("contact");
            columnFamilies.Add("others");
            if (!HBaseOperation.IsTableExists(tableName, con))
            {
                if (columnFamilies.Count > 0)
                {
                    HBaseOperation.CreateTable(tableName, columnFamilies, con);
                }
                else
                {
                    throw new HBaseException("ERROR: Table must have at least one column family");
                }
            }
           
            # endregion

            #region Inserting Values

            string[] column = new string[] { "CONTACTID", "FULLNAME", "AGE", "EMAILID", "PHONE", "MODIFIEDDATE" };
            Dictionary<string, IList<HMutation>> rowCollection = new Dictionary<string, IList<HMutation>>();
            string rowKey;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                List<HMutation> mutations = new List<HMutation>();
                rowKey = cells[i, 0].ToString();
                for (int j = 1; j < column.Length; j++)
                {
                    HMutation mutation = new HMutation();
                    mutation.ColumnFamily = j < 3 ? "info" : j < 5 ? "contact" : "others";
                    mutation.ColumnName = column[j];
                    mutation.Value = cells[i, j].ToString();
                    mutations.Add(mutation);
                }
                rowCollection[rowKey] = mutations;
            }
            HBaseOperation.InsertRows(tableName, rowCollection, con);

            #endregion Inserting Values

            #region scan values
            HBaseOperation.FetchSize = 100;
            BindingList<Customers> resultlist = new BindingList<Customers>();
            HBaseResultSet table = HBaseOperation.ScanTable(tableName, con);
            foreach (HBaseRecord row in table)
            {
                resultlist = new BindingList<Customers>(table.Select(rowValue => new Customers
                {
                    ContactId = rowValue["rowKey"] != null ? rowValue["rowKey"].ToString() : "",
                    FullName = rowValue["info:FULLNAME"] != null ? rowValue["info:FULLNAME"].ToString() : "",
                    Age = rowValue["info:AGE"] != null ? rowValue["info:AGE"].ToString() : "",
                    EmailId = rowValue["contact:EMAILID"] != null ? rowValue["contact:EMAILID"].ToString() : "",
                    PhoneNumber = rowValue["contact:PHONE"] != null ? rowValue["contact:PHONE"].ToString() : "",
                    ModifiedDate = rowValue["others:MODIFIEDDATE"] != null ? rowValue["others:MODIFIEDDATE"].ToString() : ""
                }).ToList());
            }

            #endregion scan values

            //Closing the hive connection
            con.Close();
            return resultlist;
        }
    }

    public class Customers
    {
        public string ContactId { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string ModifiedDate { get; set; }
    }

    #region reading csv file

    public class csv
    {
        #region Variable Declaration

        //Declare the object array to store the values
        private static object[,] cells;

        #endregion Variable Declaration

        #region Properties

        /// <summary>
        /// Get the RowCount of the Table
        /// </summary>
        public static int RowCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the ColumnCount of the Table
        /// </summary>
        public static int ColumnCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Get and set the ColumnNames of the Table
        /// </summary>
        public static string[] ColumnNames
        {
            get;
            internal set;
        }

        #endregion Properties

        public object[,] Table(string filepath, bool containsHeaderRow, char separator)
        {
            string[] Lines = File.ReadAllLines(filepath);
            RowCount = Lines.Length - 1;

            ColumnCount = Lines[0].Split(new[] { separator }).Length;
            //Initialize string array to add column names
            if (ColumnNames == null && containsHeaderRow)
                ColumnNames = new string[ColumnCount];
            //Parse the csv file
            cells = ReadCsv(filepath, separator, containsHeaderRow);
            return cells;
        }

        #region Method to read the data from CSV

        /// <summary>
        ///  Method to parse a CSV file
        /// </summary>
        /// <param name="filepath"> file path</param>
        /// <param name="separator"> separator to be used</param>
        /// <param name="header"> boolean header</param>

        private static object[,] ReadCsv(string filepath, char separator, bool header)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                //Read file
                FileStream csvStream = new FileStream((filepath), FileMode.Open, FileAccess.Read);
                //Create stream reader
                StreamReader reader = new StreamReader(csvStream);

                //Flag to read column
                bool isColumn = header;
                //initialize row index
                int rowindex = 0;
                //Loop if not end of stream
                while (!reader.EndOfStream)
                {
                    //Read line
                    string line = reader.ReadLine();
                    //Split line using separator. Default is ","
                    string[] fields = line.Split(new[] { separator });
                    ColumnCount = fields.Length;

                    //Checks whether the cells is null and initializes if it is null
                    if(cells == null)
                    {
                        cells = new object[RowCount, ColumnCount];
                    }
                    //Take first row csv values as column names
                    if (isColumn)
                    {     
                        //Add column names if header is true
                        if (header)
                        {
                            //Copy columnNames
                            for (int i = 0; i < ColumnCount; i++)
                            {
                                ColumnNames[i] = fields[i];
                            }
                        }
                        //Set flag to false to indicate columns are already copied
                        isColumn = false;
                    }
                    else
                    {
                        //Checks if the rowindex exceeds the cells maximum size limit
                        if (rowindex != RowCount)
                        {
                            for (int i = 0; i < ColumnCount; i++)
                                cells[rowindex, i] = fields[i];
                            rowindex++;
                        }                     
                    }
                }
            }
            return cells;
        }

        #endregion Method to read the data from CSV
    }

    #endregion reading csv file
}