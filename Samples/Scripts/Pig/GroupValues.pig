--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Load the Customers data from its location
Customers = LOAD '/Data/Customers' using PigStorage(',') as 
(CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

--Group the data by company name
group1  = GROUP Customers BY COMPANYNAME;

--Generate the Group alone
group2 = FOREACH group1 generate group;

--Display the Group
Dump group2;