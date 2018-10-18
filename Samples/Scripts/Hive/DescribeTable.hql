--This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
--To get the description of the specified table
create database if not exists sample_database;

use sample_database;

create table if not exists sample_table(datas string);

describe sample_table;