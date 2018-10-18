--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Finding Average Unit Price in each category
--Load the Products dataset from its location
Products = LOAD '/Data/Products' using PigStorage(',') as (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPE,UNITPRICE,UNITSINSTO,UNITSONORD,REORDERLEV,DISCONTINU);

group_data = GROUP Products by CATEGORYID;

Average_unit_price = FOREACH group_data generate group,AVG(Products.UNITPRICE);

DUMP Average_unit_price;