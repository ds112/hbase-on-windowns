using Syncfusion.ThriftHBase.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebSampleBrowser
{
    public class CustomerData
    {
        public class Customers
        {
            public string ContactId { get; set; }
            public string FullName { get; set; }
            public string Age { get; set; }
            public string EmailId { get; set; }
            public string PhoneNumber { get; set; }
            public string ModifiedDate { get; set; }
        }

       public static IList<Customers> list(string tableName,HBaseConnection con)
        {
            List<Customers> results = new List<Customers>();

            //Initializing the fetch size
           HBaseOperation.FetchSize = 12;

            //Fetches the given amount of data from the HBase
           HBaseResultSet table = HBaseOperation.ScanTable(tableName, con);
           //  HSchema a = HBaseOperation.DescribeTable(tableName, con);
           //HBaseOperation.
           List<Customers> resultTable = new List<Customers>();
           
            //Read each row from the fetched result
            foreach (HBaseRecord cellValue in table)
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
    }
}