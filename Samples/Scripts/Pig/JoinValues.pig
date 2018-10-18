--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Join the Customers and Orders dataset
--Load the Customers dataset from its location
Customers = LOAD '/Data/Customers' using PigStorage(',') as (CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

--Load the Orders dataset from its location
Orders = LOAD '/Data/Orders' USING PigStorage(',') as (ORDERID,CUSTOMERID,EMPLOYEEID,ORDERDATE,REQUIREDDA,SHIPPEDDAT,SHIPVIA,FREIGHT,SHIPNAME,SHIPADDRES,SHIPCITY,SHIPREGION,SHIPPOSTAL,SHIPCOUNTR);

--Join the above datasets by common column CUSTOMERID
Join_data = JOIN Customers by CUSTOMERID,Orders by CUSTOMERID;

--Generate the needed column from the joined dataset
Final_data = FOREACH Join_data generate $0,$1,$2,$11,$13,$14,$15,$16;

--View the Result
dump Final_data;