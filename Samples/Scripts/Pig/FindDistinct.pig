--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Finding the distinct value of COMPANYNAME in the given data
-- loading Customers data from its location
Customers = load '/Data/Customers' using PigStorage(',') as (CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

-- Generating COMPANYNAME alone
generateData = foreach Customers generate COMPANYNAME;

--Finding the distinct value of COMPANYNAME
result = DISTINCT generateData;

--View the Result
dump result;