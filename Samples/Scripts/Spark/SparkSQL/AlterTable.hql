--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Deleting CustomersTable if exists
Drop TABLE CustomersTable;

-- creating external table 
CREATE EXTERNAL TABLE IF NOT EXISTS Customers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers' ;

--Query to alter table. Here we renamed the table name.
Alter table Customers RENAME to CustomersTable;