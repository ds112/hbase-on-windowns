--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Load the Customers data from its location
Customers = LOAD '/Data/Customers' using PigStorage(',') as (CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY:chararray,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

--Filtered the data by the city
Data_filter = FILTER Customers by (CITY matches 'Berlin');

--View the Result
Dump Data_filter;