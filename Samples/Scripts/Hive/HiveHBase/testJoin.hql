-- This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Please ensure whether HBase services are in running state before executing this script.
-- NOTE: Please execute the HBase script "InsertValue" in HBase Module before executing this script.
-- Query to create a table in Hive and it loads data from HBase table
Create external table if not exists TACGIAtoHbase(key string,MSTG string,TENTG string,SDT string,EMAIL string) 
STORED BY 'org.apache.hadoop.hive.hbase.HBaseStorageHandler'
WITH SERDEPROPERTIES ("hbase.columns.mapping" = ":key,TG:MSTG,TG:TENTG,TG:SDT,TG:EMAIL")
TBLPROPERTIES ("hbase.table.name" = "TACGIA");

Create external table if not exists TACGIASACHtoHbase(key string,MSTG string,MSS string) 
STORED BY 'org.apache.hadoop.hive.hbase.HBaseStorageHandler'
WITH SERDEPROPERTIES ("hbase.columns.mapping1" = ":key,TGS:MSTG,TGS:MSS")
TBLPROPERTIES ("hbase.table.name1" = "TACGIASACH");



SELECT * FROM TACGIAtoHbase;
SELECT * FROM TACGIASACHtoHbase;

SELECT *  FROM TACGIAtoHbase c INNER JOIN TACGIASACHtoHbase o ON c.MSTG=o.MSTG;





