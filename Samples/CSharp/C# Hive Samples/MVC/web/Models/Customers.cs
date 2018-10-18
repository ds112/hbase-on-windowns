namespace MVCSampleBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Syncfusion.ThriftHive.Base;
    using System.Collections;
    using System.ComponentModel;
    using MVCSampleBrowser.Models;

    public class CustomersData
    {
        public static List<PersonDetail> list()
        {
            List<PersonDetail> results = new List<PersonDetail>();

            //Initialize the connection
            HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

            //To initialize a Hive server connection with secured cluster
            //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

            
            //Open the HiveServer connection
            con.Open();

            //Create table for adventure person contacts
            HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
            createCommand.ExecuteNonQuery();
            HqlCommand command = new HqlCommand("select * from AdventureWorks_Person_Contact limit 100", con);

            //Execute the query
            HqlDataReader reader = command.ExecuteReader();
            HiveResultSet result = new HiveResultSet();

            while (reader.HasRows)
            {
                reader.FetchSize = 1000;
                //Fetches the result from the reader and store it in a HiveResultSet
                result = reader.FetchResult();
                //Read each row from the fetched result
                foreach (HiveRecord row in result)
                {
                    PersonDetail person = new PersonDetail();
                    person.ContactId = row["contactid"].ToString();
                    person.FullName = row["fullname"].ToString();
                    person.Age = row["age"].ToString();
                    person.EmailId = row["emailaddress"].ToString();
                    person.PhoneNumber = row["phoneno"].ToString();
                    person.ModifiedDate = row["modifieddate"].ToString();
                    results.Add(person);
                }

            }
            //Closing the hive connection
            con.Close();
            return results;
        }
       
    }
   

    public partial class StronglyBindedCustomersData
    {
        public BindingList<PersonDetail> list()
        {
            //Initialize the connection
            HqlConnection con = new HqlConnection("localhost", 10000, HiveServer.HiveServer2);

            //To initialize a Hive server connection with secured cluster
            //HqlConnection con = new HqlConnection("<Secured cluster Namenode IP>", 10000, HiveServer.HiveServer2,"<username>","<password>");

            
            //Open the HiveServer connection
            con.Open();

            //Create table for adventure person contacts
            HqlCommand createCommand = new HqlCommand("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'", con);
            createCommand.ExecuteNonQuery();
            HqlCommand command = new HqlCommand("select * from AdventureWorks_Person_Contact limit 1000", con);

            //Executing the query
            HqlDataReader reader = command.ExecuteReader();

            HiveResultSet result = new HiveResultSet();
         
            //Initialize the list to add elements in each row
            BindingList<PersonDetail> resultlist = new BindingList<PersonDetail>();
            while (reader.HasRows)
            {
                reader.FetchSize = int.MaxValue;

                //Fetches the result from the reader and store it in a HiveResultSet
                result = reader.FetchResult();

                //Read each row from the fetched result
                foreach (HiveRecord row in result)
                {
                    resultlist = new BindingList<PersonDetail>(result.Select(rowvalue => new PersonDetail
                    {
                        ContactId = rowvalue["contactid"].ToString(),
                        FullName = rowvalue["fullname"].ToString(),
                        Age = rowvalue["age"].ToString(),
                        EmailId = rowvalue["emailaddress"].ToString(),
                        PhoneNumber = rowvalue["phoneno"].ToString(),
                        ModifiedDate = rowvalue["modifieddate"].ToString()
                    }).ToList());
                }

            }
            //Closing the hive connection
            con.Close();
            return resultlist;
        }

    }
    public class PersonDetail
    {
        public string ContactId { get; set; }
        public string FullName { get; set; }
        public string Age { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string ModifiedDate { get; set; }

    }
}
