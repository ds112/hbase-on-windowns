-- Create table 'Users' using avro schema literal property.
CREATE TABLE if not EXISTS Users ROW FORMAT SERDE 'org.apache.hadoop.hive.serde2.avro.AvroSerDe' 
STORED as INPUTFORMAT 'org.apache.hadoop.hive.ql.io.avro.AvroContainerInputFormat' 
OUTPUTFORMAT 'org.apache.hadoop.hive.ql.io.avro.AvroContainerOutputFormat' 
TBLPROPERTIES ('avro.schema.literal'='
{"namespace": "example.avro",
 "type": "record",
 "name": "User",
 "fields": [
     {"name": "name", "type": "string"},
     {"name": "favorite_color", "type": ["string", "null"]}
 ]
}
');

-- Create table 'Users' using avro schema file's url property.
-- CREATE EXTERNAL TABLE if not EXISTS Users ROW FORMAT SERDE 'org.apache.hadoop.hive.serde2.avro.AvroSerDe' 
-- STORED as INPUTFORMAT 'org.apache.hadoop.hive.ql.io.avro.AvroContainerInputFormat' 
-- OUTPUTFORMAT 'org.apache.hadoop.hive.ql.io.avro.AvroContainerOutputFormat' 
-- TBLPROPERTIES ('avro.schema.url'='hdfs:///Data/Spark/Resources/User.avsc');

-- load 'Users' table with /Data/Spark/Resources/Users.avro file.
load data inpath '/Data/Spark/Resources/Users.avro' into table Users;

-- View the table details.
select * from Users;