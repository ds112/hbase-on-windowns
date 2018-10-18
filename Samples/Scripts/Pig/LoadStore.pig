--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Load the Customers data from it's location
Customers = LOAD '/Data/Customers' using PigStorage(',') as 
(CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

-- To store the data into the location, use the below statement
-- STORE Customers into '/Data/Output' using PigStorage(',');
-- Ensure that the specified folder 'Output' not exists under /Data
-- View the contents of the loaded file.
Dump Customers;