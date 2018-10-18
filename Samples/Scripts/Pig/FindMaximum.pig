--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--Finding Maximum Unit Price in each category
--Load the Products dataset from its location
Products = LOAD '/Data/Products' using PigStorage(',') as (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPE,UNITPRICE:double,UNITSINSTO,UNITSONORD,REORDERLEV,DISCONTINU);

group_data = GROUP Products by CATEGORYID;

High_unit_price = FOREACH group_data generate group,MAX(Products.UNITPRICE);

DUMP High_unit_price;