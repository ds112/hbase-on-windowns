-- This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Please ensure whether HBase services are in running state before executing this script.
-- NOTE: Please execute the HBase script "NewTable" in HBase Module before executing this script.
-- This sample script includes HiveHBase Integration.
-- Query to create a table
CREATE EXTERNAL TABLE IF NOT EXISTS HiveCustomers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers';

-- Query to create table both in Hive 
Create EXTERNAL table if not exists HiveToHBaseCustomers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string) 
STORED BY 'org.apache.hadoop.hive.hbase.HBaseStorageHandler'
WITH SERDEPROPERTIES ("hbase.columns.mapping" = ":key,Info:COMPANYNAME,Info:CONTACTNAME,Info:CONTACTTIT,Info:ADDRESS,Info:CITY,Info:REGION,Info:POSTALCODE,Info:COUNTRY,Info:PHONE,Info:FAX")
TBLPROPERTIES ("hbase.table.name" = "Customers");

-- Query to fetch data from table
select * from HiveToHBaseCustomers LIMIT 10;

-- Data will be loaded from previously created Hive table 'HiveCustomers' into new HBase table 'Customers' and new Hive table 'HiveToHBaseCustomers'
INSERT OVERWRITE TABLE HiveToHBaseCustomers SELECT * FROM HiveCustomers;

-- Query to fetch data from Hive table.
-- Please refer HBase Shell in HBase Module for 'Customers' table.
Select COMPANYNAME,CONTACTNAME from HiveToHBaseCustomers LIMIT 10;