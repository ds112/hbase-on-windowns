from pyspark import SparkContext

# Read the input file
text_file = sc.textFile("/Data/WarPeace.txt")

# Splits each word by space and collect the words using flatMap function
# Creates the Key-Value pair with each word as the key and the integer 1 as the value using map function
# Reduces the Key-Value pair with each word as the key and the corresponding word count as the value using reduceByKey function
counts = text_file.flatMap(lambda line: line.split(" ")).map(lambda word: (word, 1)).reduceByKey(lambda a, b: a + b)

# Saves the created Key-Value pair in text file
counts.saveAsTextFile("/Data/Output/WordCountPython")