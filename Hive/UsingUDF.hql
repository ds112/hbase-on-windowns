--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--The UDF used here performs the string split operation and returns the first string from the input
--loading the UDF JAR from local directory
ADD JAR ..\..\..\Samples\UDF\StringOperationsUDF.jar;

--creating an alias for the Java classname
CREATE TEMPORARY FUNCTION first_name AS 'HiveStringOperations.StringSplit';
--
--creating external table 
CREATE EXTERNAL TABLE IF NOT EXISTS Customers(CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string) 
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers';

--selecting first name of a company
SELECT first_name(COMPANYNAME) FROM Customers;