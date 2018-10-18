--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- creating external table and table is created in /user/hive/warehouse
CREATE EXTERNAL TABLE IF NOT EXISTS Customers1(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' -- Location where the DATA is available
LOCATION '/Data/Customers';

--Create internal table and table is created in /user/hive/warehouse
CREATE TABLE IF NOT EXISTS Customers2(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ','  -- Location where the DATA is available
LOCATION '/Data/Customers';