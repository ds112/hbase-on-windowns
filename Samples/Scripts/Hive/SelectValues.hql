--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- creating external table 
create external table IF NOT EXISTS Customers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers' ;

-- Query to fetch data from table
select CUSTOMERID,COMPANYNAME,CONTACTNAME from Customers;