// sc is an existing SparkContext.
val spark = new org.apache.spark.sql.hive.HiveContext(sc)

spark.sql("create External table if not exists recommend_Ratings1(criticid string,movieid string,rating double) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Ratings/'")

// Queries are expressed in HiveQL
spark.sql("FROM recommend_Ratings1 SELECT criticid,movieid,rating").collect().foreach(println)