import spark.implicits._

// creates a DataFrame based on the content of a JSON file
val dataFrame = spark.read.json("/Data/Spark/Resources/People.json")

// Displays the content of the DataFrame
dataFrame.show()

// Print the schema in a tree format
dataFrame.printSchema()

// Select only the "name" column
dataFrame.select("name").show()

// Select everybody, but increment the age by 1
dataFrame.select(dataFrame("name"), dataFrame("age") + 1).show()

// Select people older than 21
dataFrame.filter(dataFrame("age") > 21).show()

// Count people by age
dataFrame.groupBy("age").count().show()