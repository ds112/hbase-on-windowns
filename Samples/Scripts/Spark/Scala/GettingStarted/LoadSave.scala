val dataLoad = spark.read.format("json").load("/Data/Spark/Resources/People.json")
dataLoad.select("name", "age").write.format("json").save("namesAndAges.parquet")