using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestAPIHBase
{
    public class HBaseOperation
    {
        #region Varible Declaration

        private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region ClusterVersion

        /// <summary>
        /// Version of HBase running on this cluster
        /// </summary>
        /// <param name="url">HBase Rest UrlHBase Rest Url</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ClusterVersion(string url, string userName = "", string passWord = "")
        {
            string version = Get(url + "version/cluster", userName, passWord);
            return version;
        }

        #endregion

        #region ClusterStatus
        /// <summary>
        /// Status of HBase running on this cluster
        /// </summary>
        /// <param name="url">HBase Rest UrlHBase Rest Url</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ClusterStatus(string url, string userName = "", string passWord = "")
        {
            string clusterStatus = Get(url + "status/cluster", userName, passWord);
            return clusterStatus;
        }

        #endregion

        #region ListAllTables

        /// <summary>
        /// List of all nonsystem tables in HBase
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ListAllTables(string url, string userName = "", string passWord = "")
        {
            string tableList = Get(url, userName, passWord);
            return tableList;
        }

        #endregion

        #region ListAllNamespace

        /// <summary>
        /// List all namespaces in HBase
        /// </summary>
        /// <param name="url">HBase Rest UrlHBase Rest Url</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ListAllNamespace(string url, string userName = "", string passWord = "")
        {
            string namespaceList = Get(url + "namespaces/", userName, passWord);
            return namespaceList;
        }

        #endregion

        #region DescribeNamespace

        /// <summary>
        /// Describe a specific namespace
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="namespaceName">Name of the Namespace</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DescribeNamespace(string url, string namespaceName, string userName = "", string passWord = "")
        {
            string namespaceDetail = Get(url + "namespaces/" + namespaceName, userName, passWord);
            return namespaceDetail;
        }

        #endregion

        #region NewNamespace

        /// <summary>
        /// Create a new namespace
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="namespaceName">Name of the Namespace</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string NewNamespace(string url, string namespaceName, string userName = "", string passWord = "")
        {
            string namespaceCreate = Post(url + "namespaces/" + namespaceName, "", userName, passWord, "");
            return namespaceCreate;
        }

        #endregion

        #region DescribeAllTablesInNamespace

        /// <summary>
        /// List all tables in a specific namespace
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="namespaceName">Name of the Namespace</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DescribeAllTablesInNamespace(string url, string namespaceName, string userName = "", string passWord = "")
        {
            string tablesListInNamespace = Get(url + "namespaces/" + namespaceName + "/tables", userName, passWord);
            return tablesListInNamespace;
        }

        #endregion

        #region DeleteNamespace

        /// <summary>
        /// Delete a namespace. The namespace must be empty.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="namespaceName">Name of the Namespace</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DeleteNamespace(string url, string namespaceName, string userName = "", string passWord = "")
        {
            string deleteNamespace = Delete(url + "namespaces/" + namespaceName, userName, passWord);
            return deleteNamespace;
        }

        #endregion

        #region DescribeTableSchema

        /// <summary>
        /// Describe the schema of the specified table
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DescribeTableSchema(string url, string tableName, string userName = "", string passWord = "")
        {
            string tableSchema = Get(url + tableName + "/schema", userName, passWord);
            return tableSchema;
        }

        #endregion

        #region CreateTable

        /// <summary>
        /// Create a new table, or replace an existing table's schema with the provided schema
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnFamily"></param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string CreateTable(string url, string tableName, string columnFamily, string userName = "", string passWord = "")
        {
            string createQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            createQuery += "<TableSchema name=\"" + tableName + "\">";
            createQuery += "<ColumnSchema name=\"" + columnFamily + "\"/>";
            createQuery += "</TableSchema>";
            string createTable = Post(url + tableName + "/schema", createQuery, userName, passWord);
            return createTable;
        }

        #endregion

        #region UpdateTableSchema

        /// <summary>
        /// Update an existing table with the provided schema fragment
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnFamily">Name of the Column Family</param>
        /// <param name="columnSchema">Column schema value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string UpdateTableSchema(string url, string tableName, string columnFamily, string columnSchema, string userName = "", string passWord = "")
        {
            string updateQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            updateQuery += "<TableSchema name=\"" + tableName + "\">";
            updateQuery += "<ColumnSchema name=\"" + columnFamily + "\" " + columnSchema + " />";
            updateQuery += "</TableSchema>";
            string updateTable = Post(url + tableName + "/schema", updateQuery, userName, passWord);
            return updateTable;
        }

        #endregion

        #region InsertRow

        /// <summary>
        /// Insert the values in given HBase Table
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="rowkey">Row key value</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnFamily">Name of the Column Family</param>
        /// <param name="columnName">Name of the Column</param>
        /// <param name="cellValue">Cell value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string InsertRow(string url, string rowkey, string tableName, string columnFamily, string columnName, string cellValue, string userName = "", string passWord = "")
        {
            string insertQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            insertQuery += "<CellSet>";
            insertQuery += "<Row key=\"" + Encode(rowkey) + "\">";
            insertQuery += "<Cell column=\"" + Encode(columnFamily + ":" + columnName) + "\">" + Encode(cellValue) + "</Cell>";
            insertQuery += "</Row>";
            insertQuery += "</CellSet>";
            string insertTableRow = Put(url + tableName + "/Row1", insertQuery, userName, passWord);
            return insertTableRow;
        }

        #endregion

        #region DeleteTable

        /// <summary>
        /// Delete the table
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DeleteTable(string url, string tableName, string userName = "", string passWord = "")
        {
            string deleteTable = Delete(url + tableName + "/schema", userName, passWord);
            return deleteTable;
        }

        #endregion

        #region ListTableRegion

        /// <summary>
        /// List the table regions
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ListTableRegion(string url, string tableName, string userName = "", string passWord = "")
        {
            string regionTable = Get(url + tableName + "/regions", userName, passWord);
            return regionTable;
        }

        #endregion

        #region GetRow

        /// <summary>
        /// Get the value of a single row
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rowKey">Row key value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string GetRow(string url, string tableName, string rowKey, string userName = "", string passWord = "")
        {
            string output = Get(url + tableName + "/" + rowKey, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            if (!string.IsNullOrEmpty(output))
            {
                JToken key, cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out cell);
                for (int i = 0; i < cell.Count(); i++)
                {
                    jsonvalue = JObject.Parse(((JArray)cell)[i].ToString());
                    jsonvalue.TryGetValue("column", out column);
                    jsonvalue.TryGetValue("$", out dollar);
                    list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                }
            }
            string result = string.Empty;
            foreach (var decodeOutput in list)
            {
                result += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return result;
        }

        #endregion

        #region ScanTable

        /// <summary>
        /// To scan the specific table in HBase
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ScanTable(string url, string tableName, string userName = "", string passWord = "")
        {
            string scannerQuery = "<Scanner batch=\"1\">";
            scannerQuery += "</Scanner>";
            string scannerResponse = Put(url + tableName + "/scanner", scannerQuery, userName, passWord, true);
            scannerResponse = ReplaceOriginalUrl(url, scannerResponse); //ToDo BIGDATA-10089:Azure loadbalancer redirection url
            string output = Get(scannerResponse, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            while (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                output = Get(scannerResponse, userName, passWord, "application/json");
            }
            string scannerOutput = string.Empty;
            foreach (var decodeOutput in list)
            {
                scannerOutput += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return scannerOutput;
        }
        /// <summary>
        /// Replace loadbalancer url in originalurl
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="responseUrl"></param>
        /// <returns></returns>
        private static string ReplaceOriginalUrl(string originalUrl, string responseUrl)
        {
            var splitOriginalUrl = originalUrl.Split('/');
            var splitResponseUrl = responseUrl.Split('/');

            splitResponseUrl[0] = splitOriginalUrl[0];
            splitResponseUrl[2] = splitOriginalUrl[2];

            string result = string.Empty;
            foreach (var urlString in splitResponseUrl)
            {
                result = result + urlString + "/";
            }

            return result.TrimEnd('/');
        }
        #endregion

        #region ScanRowPrefixValues

        /// <summary>
        /// Open a scanner for a given prefix. That is all rows will have the specified
        /// prefix. No other rows will be returned.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="rowKey">Row key</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ScanRowPrefixValues(string url, string rowKey, string tableName, string userName = "", string passWord = "")
        {
            string scannerQuery = "<Scanner batch=\"1\">";
            scannerQuery += "<filter> {\"type\": \"PrefixFilter\",\"value\": \"" + rowKey + "\"}</filter>";
            scannerQuery += "</Scanner>";
            string scannerResponse = Put(url + tableName + "/scanner", scannerQuery, userName, passWord, true);
            scannerResponse = ReplaceOriginalUrl(url, scannerResponse); //ToDo BIGDATA-10089:Azure loadbalancer redirection url
            string output = Get(scannerResponse, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            while (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                output = Get(scannerResponse, userName, passWord, "application/json");
            }
            string scannerOutput = string.Empty;
            foreach (var decodeOutput in list)
            {
                scannerOutput += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return scannerOutput;
        }

        #endregion

        #region ScanColumnPrefixValues

        /// <summary>
        /// Open a scanner for a given prefix. That is all columns will have the specified
        /// prefix. No other columns will be returned.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ScanColumnPrefixValues(string url, string tableName, string columnName, string userName = "", string passWord = "")
        {
            string scannerQuery = "<Scanner batch=\"1\">";
            scannerQuery += "<filter> {\"type\": \"ColumnPrefixFilter\",\"value\": \"" + Encode(columnName) + "\"}</filter>";
            scannerQuery += "</Scanner>";
            string scannerResponse = Put(url + tableName + "/scanner", scannerQuery, userName, passWord, true);
            scannerResponse = ReplaceOriginalUrl(url, scannerResponse); //ToDo BIGDATA-10089:Azure loadbalancer redirection url
            string output = Get(scannerResponse, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            while (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                output = Get(scannerResponse, userName, passWord, "application/json");
            }
            string scannerOutput = string.Empty;
            foreach (var decodeOutput in list)
            {
                scannerOutput += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return scannerOutput;
        }

        #endregion

        #region ScanRange

        /// <summary>
        /// Get a scanner on the current table starting and stopping at the specified rows.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="startRow">Start Row</param>
        /// <param name="endRow">Stop Row</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ScanRange(string url, string tableName, string startRow, string endRow, string userName = "", string passWord = "")
        {
            string scannerQuery = "<Scanner startRow=\"" + Encode(startRow) + "\" endRow=\"" + Encode(endRow) + "\" batch=\"1\">";
            scannerQuery += "</Scanner>";
            string scannerResponse = Put(url + tableName + "/scanner", scannerQuery, userName, passWord, true);
            scannerResponse = ReplaceOriginalUrl(url, scannerResponse); //ToDo BIGDATA-10089:Azure loadbalancer redirection url
            string output = Get(scannerResponse, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            while (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                output = Get(scannerResponse, userName, passWord, "application/json");
            }
            string scannerOutput = string.Empty;
            foreach (var decodeOutput in list)
            {
                scannerOutput += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return scannerOutput; ;
        }

        #endregion

        #region ScanRangeWithColumn

        /// <summary>
        /// Get a scanner on the current table starting and stopping at the specified rows.
        /// Return the specified columns.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="startRow">Start Row</param>
        /// <param name="endRow">Stop Row</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ScanRangeWithColumn(string url, string tableName, string columnName, string startRow, string endRow, string userName = "", string passWord = "")
        {
            string scannerQuery = "<Scanner startRow=\"" + Encode(startRow) + "\" endRow=\"" + Encode(endRow) + "\" batch=\"1\">";
            scannerQuery += "<filter> {\"type\": \"ColumnPrefixFilter\",\"value\": \"" + Encode(columnName) + "\"}</filter>";
            scannerQuery += "</Scanner>";
            string scannerResponse = Put(url + tableName + "/scanner", scannerQuery, userName, passWord, true);
            scannerResponse = ReplaceOriginalUrl(url, scannerResponse); //ToDo BIGDATA-10089:Azure loadbalancer redirection url
            string output = Get(scannerResponse, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            while (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                output = Get(scannerResponse, userName, passWord, "application/json");
            }
            string scannerOutput = string.Empty;
            foreach (var decodeOutput in list)
            {
                scannerOutput += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return scannerOutput;
        }

        #endregion

        #region ScanValues

        /// <summary>
        /// Get a scanner on the current table starting at the specified rows and ending at the last row in the table.
        /// Return the specified columns.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnName">Name of the column</param>
        /// <param name="startRow">Start Row</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string ScanValues(string url, string tableName, string columnName, string startRow, string userName = "", string passWord = "")
        {
            string scannerQuery = "<Scanner startRow=\"" + Encode(startRow) + "\" batch=\"1\">";
            scannerQuery += "<filter> {\"type\": \"ColumnPrefixFilter\",\"value\": \"" + Encode(columnName) + "\"}</filter>";
            scannerQuery += "</Scanner>";
            string scannerResponse = Put(url + tableName + "/scanner", scannerQuery, userName, passWord, true);
            scannerResponse = ReplaceOriginalUrl(url, scannerResponse); //ToDo BIGDATA-10089:Azure loadbalancer redirection url
            string output = Get(scannerResponse, userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            while (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                output = Get(scannerResponse, userName, passWord, "application/json");
            }
            string scannerOutput = string.Empty;
            foreach (var decodeOutput in list)
            {
                scannerOutput += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return scannerOutput;
        }

        #endregion

        #region GetRows

        /// <summary>
        /// Get the value of a list of rows
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rowKey">List of Row key value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string GetRows(string url, string tableName, List<string> rowKeys, string userName = "", string passWord = "")
        {
            string result = string.Empty;
            for (int row = 0; row < rowKeys.Count; row++)
            {
                string output = Get(url + tableName + "/" + rowKeys[row], userName, passWord, "application/json");
                List<Values> list = new List<Values>();
                if (!string.IsNullOrEmpty(output))
                {
                    JToken key, cell, column, dollar;
                    JObject jsonvalue = JObject.Parse(output);
                    JArray rowValue = (JArray)jsonvalue["Row"];
                    jsonvalue = JObject.Parse(rowValue[0].ToString());
                    jsonvalue.TryGetValue("key", out key);
                    jsonvalue.TryGetValue("Cell", out cell);
                    for (int i = 0; i < cell.Count(); i++)
                    {
                        jsonvalue = JObject.Parse(((JArray)cell)[i].ToString());
                        jsonvalue.TryGetValue("column", out column);
                        jsonvalue.TryGetValue("$", out dollar);
                        list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                    }
                }
                
                foreach (var decodeOutput in list)
                {
                    result += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
                }
            }
            return result;
        }

        #endregion

        #region DeleteRow

        /// <summary>
        /// Delete the specified single row from table
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rowKey">List of Row key value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DeleteRow(string url, string tableName, string rowKey, string userName = "", string passWord = "")
        {
            string deleteTable = Delete(url + tableName + "/" + rowKey, userName, passWord);
            return deleteTable;
        }

        #endregion

        #region DeleteRowWithColumn

        /// <summary>
        /// Delete the specified column value from single row
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rowKey">List of Row key value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DeleteRowWithColumn(string url, string tableName, string rowKey, string columnFamily, string columnName, string userName = "", string passWord = "")
        {
            string deleteTable = Delete(url + tableName + "/" + rowKey + "/" + columnFamily + ":" + columnName, userName, passWord);
            return deleteTable;
        }

        #endregion

        #region GetCell

        /// <summary>
        /// Get a single Cell for the specified table, row, and column.
        /// Returns an empty list if no such value exists. Return value for specified row/column.
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="rowkey">Row key value</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnFamily">Name of the Column Family</param>
        /// <param name="columnName">Name of the Column</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string GetCell(string url, string tableName, string rowKey, string columnFamily, string columnName, string userName = "", string passWord = "")
        {
            string output = Get(url + tableName + "/" + tableName + "/" + columnFamily + ":" + columnName, userName, passWord, "application/json");
            string cellValue = string.Empty;
            if (!string.IsNullOrEmpty(output))
            {
                JToken key, Cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                jsonvalue = JObject.Parse(rowValue[0].ToString());
                jsonvalue.TryGetValue("key", out key);
                jsonvalue.TryGetValue("Cell", out Cell);
                jsonvalue = JObject.Parse(((JArray)Cell)[0].ToString());
                jsonvalue.TryGetValue("column", out column);
                jsonvalue.TryGetValue("$", out dollar);
                cellValue = Decode(key.ToString()) + " " + Decode(column.ToString()) + " " + Decode(dollar.ToString());
            }
            return cellValue;
        }

        #endregion

        #region IsTableExists

        /// <summary>
        /// Checks whether the given table is exist or not
        /// </summary>
        /// <param name="url">HBase Rest UrlHBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static bool IsTableExists(string url, string tableName, string userName = "", string passWord = "")
        {
            try
            {
                string isTableExists = Get(url + tableName + "/exists", userName, passWord);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        #endregion

        #region GetTable

        /// <summary>
        /// To view all rows and values in the table
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string GetTable(string url, string tableName, string userName = "", string passWord = "")
        {
            string output = Get(url + tableName + "/*", userName, passWord, "application/json");
            List<Values> list = new List<Values>();
            if (!string.IsNullOrEmpty(output))
            {
                JToken key, cell, column, dollar;
                JObject jsonvalue = JObject.Parse(output);
                JArray rowValue = (JArray)jsonvalue["Row"];
                for (int r = 0; r < rowValue.Count(); r++)
                {
                    jsonvalue = JObject.Parse(rowValue[r].ToString());
                    jsonvalue.TryGetValue("key", out key);
                    jsonvalue.TryGetValue("Cell", out cell);
                    for (int i = 0; i < cell.Count(); i++)
                    {
                        jsonvalue = JObject.Parse(((JArray)cell)[i].ToString());
                        jsonvalue.TryGetValue("column", out column);
                        jsonvalue.TryGetValue("$", out dollar);
                        list.Add(new Values() { key = Decode(key.ToString()), column = Decode(column.ToString()), dollar = Decode(dollar.ToString()) });
                    }
                }
            }
            string result = string.Empty;
            foreach (var decodeOutput in list)
            {
                result += "\n" + decodeOutput.key.ToString() + " " + decodeOutput.column.ToString() + " " + decodeOutput.dollar.ToString();
            }
            return result;
        }

        #endregion

        #region ListNameSpaceTable

        /// <summary>
        /// Returns table names of the provided namespace
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="namespaceName">Name of the Namespace</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static IList ListNameSpaceTable(string url, string namespaceName, string userName = "", string passWord = "")
        {
            string tableList = Get(url, userName, passWord);
            string[] tableNames = null;
            List<string> tables = new List<string>();
            tableNames = tableList.TrimEnd('\n').Split('\n');
            string[] table = new string[tableNames.Length];
            for (int i = 0; i < tableNames.Length; i++)
            {
                if (!namespaceName.Equals("default") && tableNames[i].Contains(":"))
                {
                    if (tableNames[i].Contains(":"))
                    {
                        table = tableNames[i].Split(':');
                        if (table[0].Equals(namespaceName))
                        {
                            if (table[1] != null)
                            {
                                if (table[0].Equals(namespaceName))
                                {
                                    tables.Add(table[1]);
                                }
                            }
                        }
                    }

                }
                else if (namespaceName.Equals("default") && !tableNames[i].Contains(":"))
                {
                    tables.Add(tableNames[i]);
                }
            }
            return tables;
        }

        #endregion

        #region DeleteCell

        /// <summary>
        /// Delete the value in specified column
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rowkey">Row key value</param>
        /// <param name="columnFamily">Name of the Column Family</param>
        /// <param name="columnName">Name of the Column</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string DeleteCell(string url, string tableName, string rowKey, string columnFamily, string columnName, string userName = "", string passWord = "")
        {

            string deleteCell = Delete(url + tableName + "/" + rowKey + "/" + columnFamily + ":" + columnName, userName, passWord);
            return deleteCell;
        }

        #endregion

        #region InsertRowWithTs

        /// <summary>
        /// Insert the row with specified timestamp in HBase table
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="rowkey">Row key value</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnFamily">Name of the Column Family</param>
        /// <param name="columnName">Name of the Column</param>
        /// <param name="cellValue">Cell value</param>
        /// <param name="dateTime">Date time value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string InsertRowWithTs(string url, string rowkey, string tableName, string columnFamily, string columnName, string cellValue, DateTime dateTime, string userName = "", string passWord = "")
        {
            long timeStamp = ConvertToTimestamp(dateTime);
            string insertQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            insertQuery += "<CellSet>";
            insertQuery += "<Row key=\"" + Encode(rowkey) + "\">";
            insertQuery += "<Cell column=\"" + Encode(columnFamily + ":" + columnName) + "\" timestamp = \"" + timeStamp + "\">" + Encode(cellValue) + "</Cell>";
            insertQuery += "</Row>";
            insertQuery += "</CellSet>";
            string insertTableRow = Put(url + tableName + "/Row1", insertQuery, userName, passWord);
            return insertTableRow;
        }

        #endregion

        #region InsertRows

        /// <summary>
        /// Insert the values in given set of rows
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rows">Contains the Column family, column name and value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string InsertRows(string url, string tableName, Dictionary<string, IList<HMutation>> rows, string userName = "", string passWord = "")
        {
            string insertQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            insertQuery += "<CellSet>";
            foreach (var row in rows)
            {
                insertQuery += "<Row key=\"" + Encode(row.Key) + "\">";
                for (int i = 0; i < row.Value.Count; i++)
                {
                    insertQuery += "<Cell column=\"" + Encode(row.Value[i].ColumnFamily + ":" + row.Value[i].ColumnName) + "\">" + Encode(row.Value[i].Value) + "</Cell>";
                }
                insertQuery += "</Row>";
            }
            insertQuery += "</CellSet>";
            string insertTableRow = Put(url + tableName + "/Row1", insertQuery, userName, passWord);
            return insertTableRow;
        }

        #endregion

        #region InsertRowsWithTs

        /// <summary>
        /// Insert the values in given set of rows with specified timestamp
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="rows">Contains the Column family, column name and value</param>
        /// <param name="dateTime">Date time value</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        public static string InsertRowsWithTs(string url, string tableName, Dictionary<string, IList<HMutation>> rows, DateTime dateTime, string userName = "", string passWord = "")
        {
            long timeStamp = ConvertToTimestamp(dateTime);
            string insertQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            insertQuery += "<CellSet>";
            foreach (var row in rows)
            {
                insertQuery += "<Row key=\"" + Encode(row.Key) + "\">";
                for (int i = 0; i < row.Value.Count; i++)
                {
                    insertQuery += "<Cell column=\"" + Encode(row.Value[i].ColumnFamily + ":" + row.Value[i].ColumnName) + "\" timestamp = \"" + timeStamp + "\">" + Encode(row.Value[i].Value) + "</Cell>";
                }
                insertQuery += "</Row>";
            }
            insertQuery += "</CellSet>";
            string insertTableRow = Put(url + tableName + "/Row1", insertQuery, userName, passWord);
            return insertTableRow;
        }

        #endregion

        #region Private Methods

        #region Delete

        /// <summary>
        /// Delete method operation in web request
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        private static string Delete(string url, string userName = "", string passWord = "")
        {
            var httpWReq = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            httpWReq.Timeout = Timeout.Infinite;
            httpWReq.Method = "DELETE";
            httpWReq.ContentType = "text/xml";
            httpWReq.Accept = "text/xml";

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                httpWReq.Credentials = new NetworkCredential(userName, passWord);

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string responseStr = new StreamReader(responseStream).ReadToEnd();
            httpWReq.Abort();
            return responseStr;
        }

        #endregion

        #region Get

        /// <summary>
        /// Get Method operation in webrequest
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <returns></returns>
        private static string Get(string url, string userName = "", string passWord = "", string acceptType = "")
        {
            var httpWReq = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            httpWReq.Timeout = Timeout.Infinite;
            httpWReq.Method = "GET";
            httpWReq.ContentType = "text/xml";
            httpWReq.Accept = acceptType;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                httpWReq.Credentials = new NetworkCredential(userName, passWord);

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string responseStr = new StreamReader(responseStream).ReadToEnd();
            httpWReq.Abort();
            return responseStr;
        }

        #endregion

        #region Post

        /// <summary>
        /// Post method opeation in Web request
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="requestMessage">Request Message</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <param name="contentType">Content Type for web request</param>
        /// <returns></returns>
        private static string Post(string url, string requestMessage, string userName = "", string passWord = "", string contentType = "text/xml")
        {
            var httpWReq = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            httpWReq.Timeout = Timeout.Infinite;
            httpWReq.Method = "POST";
            httpWReq.ContentType = contentType;
            httpWReq.Accept = "text/xml";
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                httpWReq.Credentials = new NetworkCredential(userName, passWord);
            var encoding = new ASCIIEncoding();
            byte[] data;
            if (!string.IsNullOrEmpty(requestMessage))
            {
                data = encoding.GetBytes(requestMessage);
                httpWReq.ContentLength = data.Length;
            }
            else
                data = encoding.GetBytes(string.Empty);

            Stream newStream = httpWReq.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

          

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string responseStr = new StreamReader(responseStream).ReadToEnd();
            httpWReq.Abort();
            return responseStr;
        }

        #endregion

        #region Put

        /// <summary>
        /// Put method operation in web request
        /// </summary>
        /// <param name="url">HBase Rest Url</param>
        /// <param name="requestMessage">Request Message</param>
        /// <param name="userName">UserName for the cluster</param>
        /// <param name="passWord">Password for the cluster</param>
        /// <param name="isScanner"></param>
        /// <returns></returns>
        private static string Put(string url, string requestMessage, string userName = "", string passWord = "", bool isScanner = false)
        {
            var httpWReq = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            httpWReq.Timeout = Timeout.Infinite;
            httpWReq.Method = "PUT";
            httpWReq.ContentType = "text/xml";
            httpWReq.Accept = "text/xml";
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                httpWReq.Credentials = new NetworkCredential(userName, passWord);
            var encoding = new ASCIIEncoding();
            byte[] data;
            if (!string.IsNullOrEmpty(requestMessage))
            {
                data = encoding.GetBytes(requestMessage);
                httpWReq.ContentLength = data.Length;
            }
            else
                data = encoding.GetBytes(string.Empty);

            Stream newStream = httpWReq.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

           

            if (isScanner)
            {
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                string responseStr = response.Headers.Get("Location");
                httpWReq.Abort();
                return responseStr;
            }
            else
            {
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string responseStr = new StreamReader(responseStream).ReadToEnd();
                httpWReq.Abort();
                return responseStr;
            }
        }

        #endregion

        #region Encode

        /// <summary>
        /// Encode the value to bytes
        /// </summary>
        /// <param name="value">Value to be encode</param>
        /// <returns></returns>
        private static string Encode(string value)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }

        #endregion

        #region Decode

        /// <summary>
        /// Decode the value to string
        /// </summary>
        /// <param name="value">Value to be decode</param>
        /// <returns></returns>
        private static string Decode(string value)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        }

        #endregion

        #region TimeStampConverter

        /// <summary>
        /// Convert the datetime into timestamp value
        /// </summary>
        /// <param name="dateTime">Date time value</param>
        /// <returns></returns>
        private static long ConvertToTimestamp(DateTime dateTime)
        {
            TimeSpan elapsedTime = dateTime - unixEpoch;
            return (long)elapsedTime.TotalMilliseconds;
        }

        #endregion

        #endregion

    }

    class Values
    {
        public string key;
        public string column;
        public string dollar;
    }

    public class HMutation
    {
        public string ColumnFamily { get; set; }
        public string ColumnName { get; set; }
        public string Value { get; set; }
    }
}
