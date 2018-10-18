--This sample script has been included along with the Syncfusion Big Data Studio to make getting started  easier.
--The UDF used here performs the string split operation and returns the first string from the input
--register the UDF JAR from local directory
REGISTER ../../../Samples/UDF/StringOperationsUDF.jar

--loading Customers data from its location
Customers = LOAD '/Data/Customers' using PigStorage(',') as (CUSTOMERID:chararray,COMPANYNAME:chararray,CONTACTNAME:chararray,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

--using UDF 
first_name = FOREACH Customers GENERATE PigStringOperations.StringSplit(COMPANYNAME);

--display the first name of a company
DUMP first_name;