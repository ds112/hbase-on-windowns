from pyspark import SparkContext, SparkConf
import re

conf = SparkConf()
# To run this file with 'spark-submit' set master to 'yarn-cluster'
# conf.setMaster("local")
conf.setAppName("Regex")
# Creates SparkContext for the Main entry point of Spark functionality
sc = SparkContext(conf=conf)

# Read the input file
# For remote cluster set remote host_name:port instead of localhost:9000
lines = sc.textFile("/Data/NASA_Access_Log")

# Method to split the input line and returns the word at the index 3
def getKey(line):
	# Replace '- ' with empty value, so input lines are separated only by white space
	str = line.replace("- "," ")
	# Replace multiple spaces by single space
	str = ' '.join(str.split())
	# Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
	str = re.findall('\[[^\]]*\]|\"[^\"]*\"|\S+', str)
	return str[3]

# Filters the input line that contains HTTP code "200"
pairs = lines.filter(lambda s: "200" in getKey(s))
# Saves the filtered lines in text file
pairs.saveAsTextFile("/Data/Output/RegexPython");