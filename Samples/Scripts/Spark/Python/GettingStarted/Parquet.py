from pyspark import SparkContext
from pyspark.sql import SQLContext

sqlContext = SQLContext(sc)

people = sqlContext.read.format("json").load("/Data/Spark/Resources/People.json")

# DataFrames can be saved as Parquet files, maintaining the schema information.
people.write.parquet("people.parquet")

# Read in the Parquet file created above.  Parquet files are self-describing so the schema is preserved.
# The result of loading a parquet file is also a DataFrame.
parquetFile = sqlContext.read.parquet("people.parquet")

# Parquet files can also be registered as tables and then used in SQL statements.
parquetFile.registerTempTable("parquetFile");
teenagers = sqlContext.sql("SELECT name FROM parquetFile WHERE age >= 13 AND age <= 19")
teenNames = teenagers.rdd.map(lambda p: "Name: " + p.name)
for teenName in teenNames.collect():
	print (teenName)