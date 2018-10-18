-- Create table "Users' using /Data/Spark/Resources/Users.avro file.
CREATE TABLE Users USING com.databricks.spark.avro OPTIONS (path "/Data/Spark/Resources/Users.avro");

-- View the table details.
SELECT * FROM Users;