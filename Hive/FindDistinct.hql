--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Finding the distinct value of country name from the input data
--creating table with the required fields
CREATE EXTERNAL TABLE IF NOT EXISTS Customers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers';

--Query to find the distinct value of COMPANYNAME from Customers
select DISTINCT(COMPANYNAME) from Customers;