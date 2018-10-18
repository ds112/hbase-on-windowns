from pyspark.sql import SQLContext
sqlContext = SQLContext(sc)

# Create the DataFrame
dataFrame = sqlContext.read.json("/Data/Spark/Resources/People.json")

# Show the content of the DataFrame
dataFrame.show()

# Print the schema in a tree format
dataFrame.printSchema()

# Select only the "name" column
dataFrame.select("name").show()

# Select everybody, but increment the age by 1
dataFrame.select(dataFrame['name'], dataFrame['age'] + 1).show()

# Select people older than 21
dataFrame.filter(dataFrame['age'] > 21).show()

# Count people by age
dataFrame.groupBy("age").count().show()