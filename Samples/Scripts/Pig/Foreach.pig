--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Generating the particular columns from the table
--Load the Customers dataset from its location
Customers = LOAD '/Data/Customers' using PigStorage(',') as (CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

NeededColumns = FOREACH Customers GENERATE CUSTOMERID,COMPANYNAME,CONTACTNAME,PHONE,COUNTRY;

--View the result
DUMP NeededColumns;