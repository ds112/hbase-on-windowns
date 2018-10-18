--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Generating certain number of Company Names from the given data
-- loading Customers data from its location
Customers = load '/Data/Customers' using PigStorage(',') as 
(CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

-- Generating Company name column alone
CompanyName = foreach Customers generate COMPANYNAME;

--Limiting the Company name values
limitValues = LIMIT CompanyName 10;

--View the Result
Dump limitValues;