create database IF NOT EXISTS hivetest;
create external table hivetest.test(Transaction_date string,Product varchar(20),Price bigint,Payment_Type varchar(20),Name varchar(20),City varchar(20),State varchar(20),Country varchar(20),Account_Created bigint,Last_Login string,Latitude double,Longitude double) 
row format delimited 
fields terminated by ','
stored as textfile
location '${INPUT}';
insert overwrite directory '${OUTPUT}' select Name,Last_Login from hivetest.test order by Last_Login;

