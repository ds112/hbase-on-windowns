from pyspark.sql import HiveContext

sqlContext = HiveContext(sc)

#Queries are expressed in HiveQL
sqlContext.sql(("create External table if not exists recommend_Ratings1(criticid string,movieid string,rating double) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Ratings/'"))
output=sqlContext.sql("FROM recommend_Ratings1 SELECT criticid,movieid,rating")

output.show()