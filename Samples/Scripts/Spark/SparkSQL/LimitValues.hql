--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Limiting the values of a column
--creating table with the required fields
create external table if not exists Customers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers';

--Query to find the limit the value of a column
Select CONTACTNAME from Customers LIMIT 5;