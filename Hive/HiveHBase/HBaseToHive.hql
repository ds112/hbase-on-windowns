-- This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Please ensure whether HBase services are in running state before executing this script.
-- NOTE: Please execute the HBase script "InsertValue" in HBase Module before executing this script.
-- Query to create a table in Hive and it loads data from HBase table
Create external table if not exists HBaseToHiveCustomers(key string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string) 
STORED BY 'org.apache.hadoop.hive.hbase.HBaseStorageHandler'
WITH SERDEPROPERTIES ("hbase.columns.mapping" = ":key,Info:companyName,Info:contactName,Info:contactTIT,Info:address,Info:city,Info:region,Info:postalCode,Info:country,Info:phone,Info:fax")
TBLPROPERTIES ("hbase.table.name" = "Customers");

-- Query to fetch data from table
select * from HBaseToHiveCustomers LIMIT 5;

SELECT * FROM TACGIA
SELECT * FROM tac c JOIN ORDERS o
ON (c.ID = o.CUSTOMER_ID);