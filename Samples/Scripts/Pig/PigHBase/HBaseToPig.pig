-- This sample script has been included along with the Syncfusion Big Data Studio to make getting started easier.
-- Please ensure whether HBase services are in running state before executing this script.
-- NOTE: Please execute the HBase script "InsertValue" in HBase Module before executing this script.
-- Load data from the HBase Table 'Customers' into 'HBaseToPig'
HBaseToPig = LOAD 'hbase://Customers' USING org.apache.pig.backend.hadoop.hbase.HBaseStorage('Info:companyName,Info:contactName,Info:contactTIT,Info:address,Info:city,Info:region,Info:postalCode,Info:country,Info:phone,Info:fax,','-loadKey=true') as (CUSTOMERID,COMPANYNAME,CONTACTNAME,CONTACTTIT,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,PHONE,FAX);

-- View the Result
dump HBaseToPig;