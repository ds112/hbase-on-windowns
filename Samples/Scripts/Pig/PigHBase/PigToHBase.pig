-- This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Please ensure whether HBase services are in running state before executing this script.
-- NOTE: Please execute the HBase script "NewTable" in HBase Module before executing this script.
-- Load the Customers dataset from its location
Customers = load '/Data/Customers' using PigStorage(',') as 
(CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

-- Limiting the Customers values
limitValues = LIMIT Customers 10;

-- Storing the values in the 'Customers' into the HBase table 'Customers'
store Customers into 'hbase://Customers' using org.apache.pig.backend.hadoop.hbase.HBaseStorage('Info:companyName,Info:contactName,Info:contactTIT,Info:address,Info:city,Info:region,Info:postalCode,Info:country,Info:phone,Info:fax');

-- Please refer HBase Shell in HBase Module for 'Customers' table.