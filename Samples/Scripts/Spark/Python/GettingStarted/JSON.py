from pyspark import SparkContext
from pyspark.sql import SQLContext

sqlContext = SQLContext(sc)

#A JSON dataset is pointed to by path.
#The path can be either a single text file or a directory storing text files.

path = "/Data/Spark/Resources/People.json"
people = sqlContext.read.json(path)

#The inferred schema can be visualized using the printSchema() method.
people.printSchema()

#root  (Structure of people)
# |-- age: integer (nullable = true)
# |-- name: string (nullable = true)

#Register this DataFrame as a table.
people.registerTempTable("people")

teenagers = sqlContext.sql("SELECT name FROM people WHERE age >= 13 AND age <= 19")
teenNames = teenagers.rdd.map(lambda p: "Name: " + p.name)
for teenName in teenNames.collect():
	print (teenName)

anotherPeopleRDD = sc.parallelize(['{"name":"Yin","address":{"city":"Columbus","state":"Ohio"}}'])
anotherPeople = sqlContext.read.json(anotherPeopleRDD)