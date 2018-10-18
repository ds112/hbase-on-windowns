import requests
import sys
import os
from pyspark.sql import SparkSession

def MongoDB():
    # Pass the valid MongoDB uri. format: mongodb://local(or)RemoteIP/databaseName.collectionName
    spark = SparkSession \
    .builder \
	.master("local") \
	.appName("MongoDB_Python") \
    .config("spark.mongodb.output.uri", "mongodb://127.0.0.1/db.col") \
    .getOrCreate()
    characters = spark.createDataFrame([("Bilbo Baggins",50), ("Gandalf",1000), ("Thorin",195), ("Balin",178), ("Kili",77), ("Dwalin",169)],["name", "age"])
    characters.write.format("com.mongodb.spark.sql.DefaultSource").mode("overwrite").save()
    documents = spark.read.format("com.mongodb.spark.sql.DefaultSource").option("uri","mongodb://127.0.0.1/db.col").load()
    documents.show()
	
if "__main__":
    MongoDB()
